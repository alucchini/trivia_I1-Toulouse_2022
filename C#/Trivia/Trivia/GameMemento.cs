using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public partial class Game
    {
        public IGameMemento<Game> Save()
            => new Memento(_players, _places, _purses, _inPenaltyBox,
                _popQuestions, _scienceQuestions, _sportsQuestions, _rockQuestions,
                _currentPlayer, _isGettingOutOfPenaltyBox);

        private class Memento : IGameMemento<Game>
        {
            public readonly string[] Players;
            public readonly int[] Places;
            public readonly int[] Purses;
            public readonly bool[] InPenaltyBox;

            public readonly string[] PopQuestions;
            public readonly string[] ScienceQuestions;
            public readonly string[] SportsQuestions;
            public readonly string[] RockQuestions;

            public readonly int CurrentPlayer;
            public readonly bool IsGettingOutOfPenaltyBox;

            internal Memento(
                IEnumerable<string> players, 
                IEnumerable<int> places, 
                IEnumerable<int> purses, 
                IEnumerable<bool> inPenaltyBox,
                IEnumerable<string> popQuestions,
                IEnumerable<string> scienceQuestions,
                IEnumerable<string> sportsQuestions,
                IEnumerable<string> rockQuestions,
                int currentPlayer,
                bool isGettingOutOfPenaltyBox)
            {
                Players = players.ToArray();
                Places = places.ToArray();
                Purses = purses.ToArray();
                InPenaltyBox = inPenaltyBox.ToArray();

                PopQuestions = popQuestions.ToArray();
                RockQuestions = rockQuestions.ToArray();
                SportsQuestions = sportsQuestions.ToArray();
                ScienceQuestions = scienceQuestions.ToArray();

                CurrentPlayer = currentPlayer;
                IsGettingOutOfPenaltyBox = isGettingOutOfPenaltyBox;
            }

            /// <inheritdoc />
            public IGame<Game> Restore() => new Game(this);
        }

        private Game(Memento memento)
        {
            _currentPlayer = memento.CurrentPlayer;
            _isGettingOutOfPenaltyBox = memento.IsGettingOutOfPenaltyBox;

            _players = new List<string>(memento.Players);
            _places = memento.Places;
            _purses = memento.Purses;
            _inPenaltyBox = memento.InPenaltyBox;

            _popQuestions = new LinkedList<string>(memento.PopQuestions);
            _rockQuestions = new LinkedList<string>(memento.RockQuestions);
            _sportsQuestions = new LinkedList<string>(memento.SportsQuestions);
            _scienceQuestions = new LinkedList<string>(memento.ScienceQuestions);
        }
    }
}
