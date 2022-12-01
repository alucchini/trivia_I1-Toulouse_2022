using System.Collections.Generic;
using System.Linq;
using Trivia.Questions;

namespace Trivia
{
    public partial class Game
    {
        public IMemento<IGame<Game>> Save()
            => new Memento(_players, _places, _inPenaltyBox,
                _questions, _currentPlayer, _isGettingOutOfPenaltyBox, _useTechnoInsteadOfRock);

        private class Memento : IMemento<IGame<Game>>
        {
            public readonly IMemento<Player>[] Players;
            public readonly int[] Places;
            public readonly bool[] InPenaltyBox;

            public readonly IMemento<QuestionsDeck> Questions;

            public readonly int CurrentPlayer;
            public readonly bool IsGettingOutOfPenaltyBox;
            public readonly bool UseTechnoInsteadOfRock;


            internal Memento(
                IEnumerable<Player> players, 
                IEnumerable<int> places,
                IEnumerable<bool> inPenaltyBox,
                QuestionsDeck questionsDeck,
                int currentPlayer,
                bool isGettingOutOfPenaltyBox,
                bool useTechnoInsteadOfRock)
            {
                Players = players.Select(p => p.Save()).ToArray();
                Places = places.ToArray();
                InPenaltyBox = inPenaltyBox.ToArray();

                Questions = questionsDeck.Save();

                CurrentPlayer = currentPlayer;
                IsGettingOutOfPenaltyBox = isGettingOutOfPenaltyBox;
                UseTechnoInsteadOfRock = useTechnoInsteadOfRock;
            }

            /// <inheritdoc />
            public IGame<Game> Restore() => new Game(this);
        }

        private Game(Memento memento)
        {
            _currentPlayer = memento.CurrentPlayer;
            _isGettingOutOfPenaltyBox = memento.IsGettingOutOfPenaltyBox;

            _players = new List<Player>(memento.Players.Select(playerMemento => playerMemento.Restore()));
            _places = memento.Places;
            _inPenaltyBox = memento.InPenaltyBox;

            _questions = memento.Questions.Restore();
            _useTechnoInsteadOfRock = memento.UseTechnoInsteadOfRock;
        }
    }
}
