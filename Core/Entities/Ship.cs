﻿using Core.Enums;

namespace Core.Entities
{
    public abstract class Ship
    {
        public string Type { get; protected set; }
        public int Speed { get; protected set; }
        public int Length {  get; protected set; }
        public Point Position { get; private set; }
        public Direction Direction { get; private set; }
        public int AttackRange { get; protected set; }
        public int HealRange { get; protected set; }
        public HealthStage Health { get; set; }

        protected Ship(int speed, int length, Point position, Direction direction, int attackRange, int healRange) 
        {
            Speed = speed;
            Length = length;
            Position = position;
            Direction = direction;
            AttackRange = attackRange;
            HealRange = healRange;
            Health = HealthStage.FullHealth;
        }
    }
}
