using System;

namespace Trivia
{
    internal class GameWhichHasEnoughPlayers<TGame> : IGame<GameWhichHasEnoughPlayers<TGame>>
    {
        private readonly IGame<TGame> _decoratedGame;

        public GameWhichHasEnoughPlayers(IGame<TGame> decoratedGame)
        {
            _decoratedGame = decoratedGame;
        }

        /// <inheritdoc />
        public bool Add(string playerName)
        {
            return _decoratedGame.Add(playerName);
        }

        /// <inheritdoc />
        public void Roll(int roll)
        {
            ThrowIfNotEnoughPlayers();

            _decoratedGame.Roll(roll);
        }

        /// <inheritdoc />
        public Player? WasCorrectlyAnswered()
        {
            ThrowIfNotEnoughPlayers();

            return _decoratedGame.WasCorrectlyAnswered();
        }

        /// <inheritdoc />
        public Player? WrongAnswer()
        {
            ThrowIfNotEnoughPlayers();

            return _decoratedGame.WrongAnswer();
        }

        /// <inheritdoc />
        public int HowManyPlayers() => _decoratedGame.HowManyPlayers();

        /// <inheritdoc />
        public IGame<GameWhichHasEnoughPlayers<TGame>> GameWithoutAPlayer(Player playerToRemove)
        {
            return new GameWhichHasEnoughPlayers<TGame>(_decoratedGame.GameWithoutAPlayer(playerToRemove));
        }

        /// <inheritdoc />
        public IGameMemento<GameWhichHasEnoughPlayers<TGame>> Save() => new Memento(_decoratedGame.Save());

        private bool IsPlayable()
        {
            return (HowManyPlayers() >= Configuration.NombreMinimalJoueurs);
        }

        private void ThrowIfNotEnoughPlayers()
        {
            if (!IsPlayable())
                throw new Exception($"Au moins {Configuration.NombreMinimalJoueurs} joueurs requis.");
        }

        private class Memento : IGameMemento<GameWhichHasEnoughPlayers<TGame>>
        {
            private readonly IGameMemento<TGame> _decoratedMemento;

            public Memento(IGameMemento<TGame> decoratedMemento)
            {
                _decoratedMemento = decoratedMemento;
            }

            /// <inheritdoc />
            public IGame<GameWhichHasEnoughPlayers<TGame>> Restore()
                => new GameWhichHasEnoughPlayers<TGame>(_decoratedMemento.Restore());
        }
    }
}
