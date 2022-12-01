namespace Trivia
{
    public class Player
    {
        private readonly string _name;

        public Player(string name)
        {
            _name = name;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _name;
        }
    }
}
