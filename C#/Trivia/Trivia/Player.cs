using System;

namespace Trivia
{
    public class Player : IEquatable<Player>
    {
        public int Purse { get; private set; }
        public int Place { get; private set; }
        public bool IsInPenaltyBox { get; private set; }

        private readonly string _name;

        public Player(string name)
        {
            _name = name;
        }

        private Player(Memento memento)
        {
            _name = memento.Name;
            Purse = memento.Purse;
            Place = memento.Place;
            IsInPenaltyBox = memento.IsInPenaltyBox;
        }

        public IMemento<Player> Save() => new Memento(_name, Purse, Place, IsInPenaltyBox);

        /// <inheritdoc />
        public override string ToString() => _name;

        public void AddOneGold()
        {
            Purse++;
        }

        public void Move(int roll)
        {
            Place += roll;
            if (Place > 11) Place -= 12;
        }

        public void PutInPenaltyBox()
        {
            IsInPenaltyBox = true;
        }

        public void FreeFromPenaltyBox()
        {
            IsInPenaltyBox = false;
        }

        private record Memento(string Name, int Purse, int Place, bool IsInPenaltyBox) : IMemento<Player>
        {
            /// <inheritdoc />
            public Player Restore() => new (this);
        }

        /// <inheritdoc />
        public bool Equals(Player? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _name == other._name;    
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Player other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => _name.GetHashCode();
        public static bool operator ==(Player? left, Player? right) => Equals(left, right);
        public static bool operator !=(Player? left, Player? right) => !Equals(left, right);
    }
}
