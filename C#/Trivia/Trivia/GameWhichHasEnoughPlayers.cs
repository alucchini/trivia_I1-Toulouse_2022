using System;

namespace Trivia
{
    internal class GameWhichHasEnoughPlayers<TGame> : GameDecorator<GameWhichHasEnoughPlayers<TGame>, TGame>
    {
        public GameWhichHasEnoughPlayers(IGame<TGame> decoratedGame) 
            : base(decoratedGame)
        {
        }

        /// <inheritdoc />
        protected override IGame<GameWhichHasEnoughPlayers<TGame>> Factory(IGame<TGame> game)
            => new GameWhichHasEnoughPlayers<TGame>(game);

        public override void Roll(int roll)
        {
            ThrowIfNotEnoughPlayers();
            base.Roll(roll);
        }
        
        public override Player? WasCorrectlyAnswered()
        {
            ThrowIfNotEnoughPlayers();
            return base.WasCorrectlyAnswered();
        }
        
        public override Player? WrongAnswer()
        {
            ThrowIfNotEnoughPlayers();
            return base.WrongAnswer();
        }

        private void ThrowIfNotEnoughPlayers()
        {
            if (NumberOfPlayers < Configuration.NombreMinimalJoueurs)
                throw new Exception($"Au moins {Configuration.NombreMinimalJoueurs} joueurs requis.");
        }
    }
}
