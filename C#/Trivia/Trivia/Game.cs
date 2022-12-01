using System;
using System.Collections.Generic;

namespace Trivia
{
    public partial class Game : IGame<Game>
    {
        private readonly List<string> _players = new ();

        private readonly int[] _places = new int[Configuration.NombreMaximalJoueurs + 1];
        private readonly int[] _purses = new int[Configuration.NombreMaximalJoueurs + 1];

        private readonly bool[] _inPenaltyBox = new bool[Configuration.NombreMaximalJoueurs + 1];

        private readonly QuestionGenerator _popQuestions;
        private readonly QuestionGenerator _scienceQuestions;
        private readonly QuestionGenerator _sportsQuestions;
        private readonly QuestionGenerator _rockQuestions;

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game()
        {
            _popQuestions = new QuestionGenerator("Pop");
            _scienceQuestions = new QuestionGenerator("Science");
            _sportsQuestions = new QuestionGenerator("Sports");
            _rockQuestions = new QuestionGenerator("Rock");
        }

        // Constructeur copiant la partie en éliminant un joueur
        private Game(Game copied, Player playerToRemove)
        {
            var playerToRemoveId = copied._players.IndexOf(playerToRemove.ToString());

            _currentPlayer = copied._currentPlayer;
            if(_currentPlayer == playerToRemoveId) IncrementCurrentPlayer();

            _isGettingOutOfPenaltyBox = copied._isGettingOutOfPenaltyBox;

            for (var index = 0; index <= Configuration.NombreMaximalJoueurs; index++)
            {
                if(index == playerToRemoveId) continue;

                _inPenaltyBox[index] = copied._inPenaltyBox[index];
                _places[index] = copied._places[index];
                if(copied._players.Count > index) _players.Add(copied._players[index]);
                _purses[index] = copied._purses[index];
            }

            _popQuestions = copied._popQuestions.Save().Restore();
            _rockQuestions = copied._rockQuestions.Save().Restore();
            _scienceQuestions = copied._scienceQuestions.Save().Restore();
            _sportsQuestions = copied._sportsQuestions.Save().Restore();
        }

        public bool Add(string playerName)
        {
            _players.Add(playerName);
            _places[NumberOfPlayers] = 0;
            _purses[NumberOfPlayers] = 0;
            _inPenaltyBox[NumberOfPlayers] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public int NumberOfPlayers => _players.Count;

        /// <inheritdoc />
        public IGame<Game> GameWithoutAPlayer(Player playerToRemove) => new Game(this, playerToRemove);

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                    _places[_currentPlayer] += roll;
                    if (_places[_currentPlayer] > 11) _places[_currentPlayer] -= 12;

                    Console.WriteLine(_players[_currentPlayer]
                            + "'s new location is "
                            + _places[_currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                _places[_currentPlayer] += roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] -= 12;

                Console.WriteLine(_players[_currentPlayer]
                        + "'s new location is "
                        + _places[_currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                Console.WriteLine(_popQuestions.NextQuestion);
            }
            if (CurrentCategory() == "Science")
            {
                Console.WriteLine(_scienceQuestions.NextQuestion);
            }
            if (CurrentCategory() == "Sports")
            {
                Console.WriteLine(_sportsQuestions.NextQuestion);
            }
            if (CurrentCategory() == "Rock")
            {
                Console.WriteLine(_rockQuestions.NextQuestion);
            }
        }

        private string CurrentCategory()
        {
            if (_places[_currentPlayer] == 0) return "Pop";
            if (_places[_currentPlayer] == 4) return "Pop";
            if (_places[_currentPlayer] == 8) return "Pop";
            if (_places[_currentPlayer] == 1) return "Science";
            if (_places[_currentPlayer] == 5) return "Science";
            if (_places[_currentPlayer] == 9) return "Science";
            if (_places[_currentPlayer] == 2) return "Sports";
            if (_places[_currentPlayer] == 6) return "Sports";
            if (_places[_currentPlayer] == 10) return "Sports";
            return "Rock";
        }

        private void IncrementCurrentPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
        }

        public Player? WasCorrectlyAnswered()
        {

            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    _purses[_currentPlayer]++;
                    Console.WriteLine(_players[_currentPlayer]
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();
                    IncrementCurrentPlayer();
                    return winner;
                }
                else
                {
                    IncrementCurrentPlayer();
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Answer was correct!!!!");
                _purses[_currentPlayer]++;
                Console.WriteLine(_players[_currentPlayer]
                        + " now has "
                        + _purses[_currentPlayer]
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                IncrementCurrentPlayer();

                return winner;
            }
        }

        public Player? WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            IncrementCurrentPlayer();
            return null;
        }

        private Player? DidPlayerWin()
        {
            return _purses[_currentPlayer] == 6 ? new Player(_players[_currentPlayer]) : default;
        }
    }

}
