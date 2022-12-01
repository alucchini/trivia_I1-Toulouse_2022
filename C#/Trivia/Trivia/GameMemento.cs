using System.Collections.Generic;
using System.Linq;
using Trivia.Questions;

namespace Trivia
{
    public partial class Game
    {
        public IMemento<IGame<Game>> Save()
            => new Memento(_players, _questions, _currentPlayer, _isGettingOutOfPenaltyBox, _useTechnoInsteadOfRock, _goldsRequiredToWin);

        private class Memento : IMemento<IGame<Game>>
        {
            public readonly IMemento<Player>[] Players;

            public readonly IMemento<QuestionsDeck> Questions;

            public readonly int CurrentPlayer;
            public readonly bool IsGettingOutOfPenaltyBox;
            public readonly bool UseTechnoInsteadOfRock;
            public readonly ushort GoldsRequiredToWin;


            internal Memento(
                IEnumerable<Player> players,
                QuestionsDeck questionsDeck,
                int currentPlayer,
                bool isGettingOutOfPenaltyBox,
                bool useTechnoInsteadOfRock,
                ushort goldsRequiredToWin)
            {
                Players = players.Select(p => p.Save()).ToArray();

                Questions = questionsDeck.Save();

                CurrentPlayer = currentPlayer;
                IsGettingOutOfPenaltyBox = isGettingOutOfPenaltyBox;
                UseTechnoInsteadOfRock = useTechnoInsteadOfRock;
                GoldsRequiredToWin = goldsRequiredToWin;
            }

            /// <inheritdoc />
            public IGame<Game> Restore() => new Game(this);
        }

        private Game(Memento memento)
        {
            _currentPlayer = memento.CurrentPlayer;
            _isGettingOutOfPenaltyBox = memento.IsGettingOutOfPenaltyBox;

            _players = new List<Player>(memento.Players.Select(playerMemento => playerMemento.Restore()));

            _questions = memento.Questions.Restore();
            _useTechnoInsteadOfRock = memento.UseTechnoInsteadOfRock;
            _goldsRequiredToWin = memento.GoldsRequiredToWin;
        }
    }
}
