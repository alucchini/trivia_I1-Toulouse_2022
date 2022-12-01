using System;

namespace Trivia
{
    internal abstract class GameDecorator<TDecorator, TGame> : IGame<TDecorator>
    {
        private readonly IGame<TGame> _decoratedGame;

        protected GameDecorator(IGame<TGame> decoratedGame)
        {
            _decoratedGame = decoratedGame;  
        }

        protected abstract IGame<TDecorator> Factory(IGame<TGame> game);

        /// <inheritdoc />
        public virtual bool Add(string playerName) => _decoratedGame.Add(playerName);

        /// <inheritdoc />
        public virtual void Roll(int roll) => _decoratedGame.Roll(roll);

        /// <inheritdoc />
        public virtual Player? WasCorrectlyAnswered() => _decoratedGame.WasCorrectlyAnswered();

        /// <inheritdoc />
        public virtual Player? WrongAnswer() => _decoratedGame.WrongAnswer();

        /// <inheritdoc />
        public int NumberOfPlayers => _decoratedGame.NumberOfPlayers;

        /// <inheritdoc />
        public IGame<TDecorator> GameWithoutAPlayer(Player playerToRemove) 
            => Factory(_decoratedGame.GameWithoutAPlayer(playerToRemove));

        /// <inheritdoc />
        public IGameMemento<TDecorator> Save() 
            => new Memento(_decoratedGame.Save(), Factory);

        private class Memento : IGameMemento<TDecorator>
        {
            private readonly IGameMemento<TGame> _decoratedMemento;
            private readonly Func<IGame<TGame>, IGame<TDecorator>> _factoryMethod;

            public Memento(IGameMemento<TGame> decoratedMemento, Func<IGame<TGame>, IGame<TDecorator>> factoryMethod)
            {
                _decoratedMemento = decoratedMemento;
                _factoryMethod = factoryMethod;
            }

            /// <inheritdoc />
            public IGame<TDecorator> Restore()
                => _factoryMethod(_decoratedMemento.Restore());
        }
    }
}
