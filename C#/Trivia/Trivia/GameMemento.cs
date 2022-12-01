using System.Collections.Generic;
using System.Linq;
using Trivia.Questions;

namespace Trivia
{
    public partial class Game
    {
        public IMemento<IGame<Game>> Save()
            => new Memento(_players, _places, _purses, _inPenaltyBox,
                _questions, _currentPlayer, _isGettingOutOfPenaltyBox, _useTechnoInsteadOfRock);

        private class Memento : IMemento<IGame<Game>>
        {
            public readonly string[] Players;
            public readonly int[] Places;
            public readonly int[] Purses;
            public readonly bool[] InPenaltyBox;

            public readonly IMemento<QuestionsDeck> Questions;

            public readonly int CurrentPlayer;
            public readonly bool IsGettingOutOfPenaltyBox;
            public readonly bool UseTechnoInsteadOfRock;


            internal Memento(
                IEnumerable<string> players, 
                IEnumerable<int> places, 
                IEnumerable<int> purses, 
                IEnumerable<bool> inPenaltyBox,
                QuestionsDeck questionsDeck,
                int currentPlayer,
                bool isGettingOutOfPenaltyBox,
                bool useTechnoInsteadOfRock)
            {
                Players = players.ToArray();
                Places = places.ToArray();
                Purses = purses.ToArray();
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

            _players = new List<string>(memento.Players);
            _places = memento.Places;
            _purses = memento.Purses;
            _inPenaltyBox = memento.InPenaltyBox;

            _questions = memento.Questions.Restore();
            _useTechnoInsteadOfRock = memento.UseTechnoInsteadOfRock;
        }
    }
}
