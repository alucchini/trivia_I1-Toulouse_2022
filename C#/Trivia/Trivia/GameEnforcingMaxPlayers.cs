using System;

namespace Trivia
{
    internal class GameEnforcingMaxPlayers<TGame> : GameDecorator<GameEnforcingMaxPlayers<TGame>, TGame>
    {
        /// <inheritdoc />
        public GameEnforcingMaxPlayers(IGame<TGame> decoratedGame) : base(decoratedGame)
        {
        }

        /// <inheritdoc />
        protected override IGame<GameEnforcingMaxPlayers<TGame>> Factory(IGame<TGame> game)
            => new GameEnforcingMaxPlayers<TGame>(game);

        /// <inheritdoc />
        public override void Add(string playerName)
        {
            if (NumberOfPlayers >= Configuration.NombreMaximalJoueurs)
                throw new Exception($"Pas plus de {Configuration.NombreMaximalJoueurs} joueurs autorisés");

            base.Add(playerName);
        }
    }
}
