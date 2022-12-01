using System;

namespace Trivia
{
    public class Player : IEquatable<Player>
    {
        private readonly string _name;

        public Player(string name)
        {
            _name = name;
        }

        public IMemento<Player> Save() => new Memento(_name);

        /// <inheritdoc />
        public override string ToString() => _name;

        private record Memento(string Name) : IMemento<Player>
        {
            /// <inheritdoc />
            public Player Restore() => new (Name);
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
