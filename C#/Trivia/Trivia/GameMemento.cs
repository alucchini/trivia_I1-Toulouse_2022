using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public partial class Game
    {
        public IMemento<IGame<Game>> Save()
            => new Memento(_players, _places, _purses, _inPenaltyBox,
                _popQuestions, _scienceQuestions, _sportsQuestions, _rockQuestions,
                _currentPlayer, _isGettingOutOfPenaltyBox);

        private class Memento : IMemento<IGame<Game>>
        {
            public readonly string[] Players;
            public readonly int[] Places;
            public readonly int[] Purses;
            public readonly bool[] InPenaltyBox;

            public readonly IMemento<QuestionGenerator> PopQuestions;
            public readonly IMemento<QuestionGenerator> ScienceQuestions;
            public readonly IMemento<QuestionGenerator> SportsQuestions;
            public readonly IMemento<QuestionGenerator> RockQuestions;

            public readonly int CurrentPlayer;
            public readonly bool IsGettingOutOfPenaltyBox;

            internal Memento(
                IEnumerable<string> players, 
                IEnumerable<int> places, 
                IEnumerable<int> purses, 
                IEnumerable<bool> inPenaltyBox,
                QuestionGenerator popQuestions,
                QuestionGenerator scienceQuestions,
                QuestionGenerator sportsQuestions,
                QuestionGenerator rockQuestions,
                int currentPlayer,
                bool isGettingOutOfPenaltyBox)
            {
                Players = players.ToArray();
                Places = places.ToArray();
                Purses = purses.ToArray();
                InPenaltyBox = inPenaltyBox.ToArray();

                PopQuestions = popQuestions.Save();
                RockQuestions = rockQuestions.Save();
                SportsQuestions = sportsQuestions.Save();
                ScienceQuestions = scienceQuestions.Save();

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

            _popQuestions = memento.PopQuestions.Restore();
            _rockQuestions = memento.RockQuestions.Restore();
            _sportsQuestions = memento.SportsQuestions.Restore();
            _scienceQuestions = memento.ScienceQuestions.Restore();
        }
    }
}
