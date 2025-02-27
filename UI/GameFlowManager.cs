using BLL.Abstractions.Factories;
using BLL.Abstractions.Helpers;
using BLL.Abstractions.Services;
using Core.Entities;
using Core.Enums;
using UI.Rendering;

namespace UI
{
    public class GameFlowManager
    {
        private readonly IGameService _gameService;
        private readonly IShipFactory _shipFactory;
        private readonly IShipHelper _shipHelper;
        private readonly FieldRenderer _fieldRenderer;

        public GameFlowManager(IGameService gameService, IShipFactory shipFactory, IShipHelper shipHelper, FieldRenderer fieldRenderer)
        {
            _gameService = gameService;
            _shipFactory = shipFactory;
            _shipHelper = shipHelper;
            _fieldRenderer = fieldRenderer;
        }

        public void StartGame()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Battleship!");

            if (LoadPreviousSession())
            {
                Console.WriteLine("Previous session found. Resuming game...");
            }
            else
            {
                Console.WriteLine("No previous session found. Starting a new game...");
                SetupPlayerField(Player.Player1);
                SetupPlayerField(Player.Player2);
                _gameService.SetCurrentPlayer(Player.Player1);
            }

            PlayGameLoop();
        }

        private bool LoadPreviousSession()
        {
            var player1ShipsResult = _gameService.GetCurrentPlayerFieldService().GetAllShips();
            var player2ShipsResult = _gameService.GetOpponentFieldService().GetAllShips();

            if (!player1ShipsResult.IsSuccess || !player2ShipsResult.IsSuccess)
                return false;

            if (player1ShipsResult.Data.Any() && player2ShipsResult.Data.Any())
            {
                Console.WriteLine("Do you want to continue the previous session? (Y/N)");
                string choice = Console.ReadLine()?.Trim().ToUpper();
                if (choice == "Y")
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Starting a new game...");
                    _gameService.ClearGameData();
                    return false;
                }
            }

            return false;
        }

        private void PlayGameLoop()
        {
            while (true)
            {
                Console.Clear();
                RenderFields();

                Console.WriteLine($"\n{_gameService.GetCurrentPlayer()}'s Turn");
                Console.WriteLine("Choose an action: ");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Heal");
                Console.WriteLine("3. End Turn");
                Console.WriteLine("4. Exit");

                string action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        PerformAttack();
                        break;
                    case "2":
                        PerformHeal();
                        break;
                    case "3":
                        EndTurn();
                        break;
                    case "4":
                        ExitGame();
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }

                SaveGameState();
            }
        }

        private void SaveGameState()
        {
            _gameService.GetCurrentPlayerFieldService().GetAllShips().Data?.ToList()
                .ForEach(s => _gameService.GetCurrentPlayerFieldService().AddShip(s, _fieldRenderer.FieldWidth, _fieldRenderer.FieldHeight));

            _gameService.GetOpponentFieldService().GetAllShips().Data?.ToList()
                .ForEach(s => _gameService.GetOpponentFieldService().AddShip(s, _fieldRenderer.FieldWidth, _fieldRenderer.FieldHeight));
        }

        private void SetupPlayerField(Player player)
        {
            Console.Clear();
            Console.WriteLine($"Player {player}, set up your fleet.");

            _gameService.SetCurrentPlayer(player);
            var fieldService = _gameService.GetCurrentPlayerFieldService();
            int fieldWidth = 10, fieldHeight = 10;

            int shipsToPlace = 3;

            for (int i = 0; i < shipsToPlace; i++)
            {
                Console.Clear();
                Console.WriteLine($"Placing ship {i + 1}...");

                Ship ship = CreateShipWithTypeSelection(fieldService);
                if (ship == null)
                {
                    Console.WriteLine("Invalid ship placement, try again.");
                    i--;
                    continue;
                }

                var addShipResult = fieldService.AddShip(ship, fieldWidth, fieldHeight);
                if (!addShipResult.IsSuccess)
                {
                    Console.WriteLine($"Error: {addShipResult.Message}. Try again.");
                    i--;
                }
                else
                {
                    Console.WriteLine("Ship placed successfully!");
                }
            }

            Console.WriteLine("All ships placed! Press any key to continue...");
            Console.ReadKey();
        }

        private Ship CreateShipWithTypeSelection(IFieldService fieldService)
        {
            Console.Clear();
            Console.WriteLine("Select ship type:");
            Console.WriteLine("1. Military (Attack Only) - 3 spaces");
            Console.WriteLine("2. Auxiliary (Heal Only) - 2 spaces");
            Console.WriteLine("3. Mixed (Attack & Heal) - 4 spaces");

            string choice = Console.ReadLine();
            int shipLength = choice switch
            {
                "1" => 3,
                "2" => 2,
                "3" => 4,
                _ => -1
            };

            if (shipLength == -1)
            {
                Console.WriteLine("Invalid choice. Try again.");
                return null;
            }

            return CreateShipWithVisuals(fieldService, shipLength);
        }

        private Ship CreateShipWithVisuals(IFieldService fieldService, int shipLength)
        {
            int x = 0, y = 0;
            Direction direction = Direction.East;
            bool placing = true;

            while (placing)
            {
                Console.Clear();
                Console.WriteLine("Use Arrow Keys to Move, R to Rotate, Enter to Confirm");
                RenderFieldWithCursor(fieldService, x, y, direction, shipLength);

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (y > 0) y--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (y < 9) y++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x > 0) x--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (x < 9) x++;
                        break;
                    case ConsoleKey.R:
                        direction = (Direction)(((int)direction + 1) % 4);
                        break;
                    case ConsoleKey.Enter:
                        var placementResult = fieldService.IsValidPlacement(x, y, direction, shipLength, 10, 10);
                        if (placementResult.IsSuccess)
                        {
                            placing = false;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid ship placement! {placementResult.Message} Try again.");
                            Console.ReadKey();
                        }
                        break;
                }
            }

            string shipType = shipLength switch
            {
                3 => "military",
                2 => "auxiliary",
                4 => "mixed",
                _ => null
            };

            if (shipType == null)
            {
                Console.WriteLine("Invalid ship selection.");
                return null;
            }

            return _shipFactory.CreateShip(shipType, new Point(x, y), direction);
        }

        private void RenderFieldWithCursor(IFieldService fieldService, int cursorX, int cursorY, Direction direction, int shipLength)
        {
            Console.Clear();
            Console.WriteLine("Use Arrow Keys to Move, R to Rotate, Enter to Confirm");

            Field field = fieldService.GetField();

            var shipsResult = fieldService.GetAllShips();
            if (!shipsResult.IsSuccess || shipsResult.Data == null)
            {
                Console.WriteLine("Error: Unable to retrieve ships.");
                return;
            }

            var occupiedPoints = new HashSet<Point>();
            foreach (var ship in shipsResult.Data)
            {
                var shipPoints = _shipHelper.GetOccupiedPoints(ship);
                foreach (var point in shipPoints)
                {
                    occupiedPoints.Add(point);
                }
            }

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    bool isShipPart = false;
                    bool isExistingShip = occupiedPoints.Contains(new Point(x, y));

                    for (int i = 0; i < shipLength; i++)
                    {
                        int projectedX = cursorX, projectedY = cursorY;
                        switch (direction)
                        {
                            case Direction.North:
                                projectedY -= i;
                                break;
                            case Direction.South:
                                projectedY += i;
                                break;
                            case Direction.East:
                                projectedX += i;
                                break;
                            case Direction.West:
                                projectedX -= i;
                                break;
                        }

                        if (x == projectedX && y == projectedY)
                        {
                            isShipPart = true;
                            break;
                        }
                    }

                    var placementCheck = fieldService.IsValidPlacement(cursorX, cursorY, direction, shipLength, 10, 10);
                    bool isValidPlacement = placementCheck.IsSuccess;

                    if (isShipPart && isValidPlacement)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("X");
                    }
                    else if (isShipPart && !isValidPlacement)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                    }
                    else if (isExistingShip)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("S");
                    }
                    else
                    {
                        Console.Write("~");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        private void RenderFields(string message = null)
        {
            Console.Clear();

            Console.WriteLine($"Current Player: {_gameService.GetCurrentPlayer()}");

            if (!string.IsNullOrEmpty(message))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(message);
                Console.ResetColor();
            }

            _fieldRenderer.RenderField(_gameService.GetCurrentPlayerFieldService().GetField(), "Your Field");
            _fieldRenderer.RenderField(_gameService.GetOpponentFieldService().GetField(), "Opponent's Field");
        }

        private void PerformAttack()
        {
            Console.WriteLine("Enter target coordinates (x y): ");
            var input = Console.ReadLine().Split();
            if (input.Length != 2 || !int.TryParse(input[0], out int x) || !int.TryParse(input[1], out int y))
            {
                Console.WriteLine("Invalid input! Please enter two numbers separated by space.");
                return;
            }

            var targetPosition = new Point(x, y);
            var attackResult = _gameService.Attack(targetPosition);

            if (!attackResult.IsSuccess)
            {
                Console.WriteLine($"Missed! {attackResult.Message}");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Hit!");
                Console.ReadKey();
            }

            if (attackResult.Message?.Contains("wins!") == true)
            {
                Console.WriteLine(attackResult.Message);
                Console.WriteLine("Game Over!");
                Console.ReadKey();

                PromptForRestartOrExit();
            }

            RenderFields();
        }

        private void PerformHeal()
        {
            Console.WriteLine("Enter heal coordinates (x y): ");
            var input = Console.ReadLine().Split();
            if (input.Length != 2 || !int.TryParse(input[0], out int x) || !int.TryParse(input[1], out int y))
            {
                Console.WriteLine("Invalid input! Please enter two numbers separated by space.");
                return;
            }

            var healPosition = new Point(x, y);
            var healResult = _gameService.Heal(healPosition);

            Console.WriteLine(healResult.Message ?? "Healing action performed.");
            Console.ReadKey();

            RenderFields();
        }

        private void EndTurn()
        {
            _gameService.SwitchTurn();
            RenderFields();
            SaveGameState();
        }

        private void PromptForRestartOrExit()
        {
            Console.Clear();
            Console.WriteLine("Would you like to start a new game? (Y/N)");

            while (true)
            {
                string choice = Console.ReadLine()?.Trim().ToUpper();

                if (choice == "Y")
                {
                    RestartGame();
                    break;
                }
                else if (choice == "N")
                {
                    ExitGame();
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 'Y' to restart or 'N' to exit.");
                }
            }
        }

        private void ExitGame()
        {
            Console.WriteLine("Thanks for playing! Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private void RestartGame()
        {
            Console.Clear();
            Console.WriteLine("Starting a new game...");

            var player1FieldService = _gameService.GetCurrentPlayerFieldService();
            var player2FieldService = _gameService.GetOpponentFieldService();

            player1FieldService.GetField().Ships.Clear();
            player2FieldService.GetField().Ships.Clear();

            SetupPlayerField(Player.Player1);
            SetupPlayerField(Player.Player2);

            PlayGameLoop();
        }
    }
}
