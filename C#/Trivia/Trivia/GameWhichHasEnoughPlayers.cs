using System;

namespace Trivia
{
    internal class GameWhichHasEnoughPlayers : IGame
    {
        private readonly IGame _decoratedGame;

        public GameWhichHasEnoughPlayers(IGame decoratedGame)
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

        private bool IsPlayable()
        {
            return (HowManyPlayers() >= Configuration.NombreMinimalJoueurs);
        }

        private void ThrowIfNotEnoughPlayers()
        {
            if (!IsPlayable())
                throw new Exception($"Au moins {Configuration.NombreMinimalJoueurs} joueurs requis.");
        }
    }
}
