using System;
using System.Collections.Generic;
using Trivia.Questions;

namespace Trivia
{
    public partial class Game : IGame<Game>
    {
        private readonly bool _useTechnoInsteadOfRock;
        private readonly List<Player> _players = new ();
        
        private readonly bool[] _inPenaltyBox = new bool[Configuration.NombreMaximalJoueurs + 1];

        private readonly QuestionsDeck _questions;

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game(bool useTechnoInsteadOfRock)
        {
            _useTechnoInsteadOfRock = useTechnoInsteadOfRock;
            _questions = new QuestionsDeck("Pop", "Science", "Sports", useTechnoInsteadOfRock ? "Techno" : "Rock");
        }

        // Constructeur copiant la partie en éliminant un joueur
        private Game(Game copied, Player playerToRemove)
        {
            var playerToRemoveId = copied._players.IndexOf(playerToRemove);

            _currentPlayer = copied._currentPlayer;
            if(_currentPlayer == playerToRemoveId) IncrementCurrentPlayer();

            _isGettingOutOfPenaltyBox = copied._isGettingOutOfPenaltyBox;

            for (var index = 0; index <= Configuration.NombreMaximalJoueurs; index++)
            {
                if(index == playerToRemoveId) continue;

                _inPenaltyBox[index] = copied._inPenaltyBox[index];
                if(copied._players.Count > index) _players.Add(copied._players[index]);
            }

            _questions = copied._questions.Save().Restore();
            _useTechnoInsteadOfRock = copied._useTechnoInsteadOfRock;
        }

        public void Add(string playerName)
        {
            _players.Add(new Player(playerName));
            _inPenaltyBox[NumberOfPlayers] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
        }

        public int NumberOfPlayers => _players.Count;
        private Player CurrentPlayer => _players[_currentPlayer % (NumberOfPlayers - 1)];

        /// <inheritdoc />
        public IGame<Game> GameWithoutAPlayer(Player playerToRemove) => new Game(this, playerToRemove);

        public void Roll(int roll)
        {
            Console.WriteLine(CurrentPlayer + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(CurrentPlayer + " is getting out of the penalty box");
                    CurrentPlayer.Move(roll);

                    Console.WriteLine(CurrentPlayer
                                      + "'s new location is "
                                      + CurrentPlayer.Place);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(CurrentPlayer + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                CurrentPlayer.Move(roll);

                Console.WriteLine(CurrentPlayer
                                  + "'s new location is "
                                  + CurrentPlayer.Place);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            Console.WriteLine(_questions.NextQuestionForCategory(CurrentCategory()));
        }

        private string CurrentCategory()
        {
            switch (CurrentPlayer.Place)
            {
                case 0:
                case 4:
                case 8:
                    return "Pop";
                case 1:
                case 5:
                case 9:
                    return "Science";
                case 2:
                case 6:
                case 10:
                    return "Sports";
                default:
                    return _useTechnoInsteadOfRock ? "Techno" : "Rock";
            }
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
                    CurrentPlayer.AddOneGold();
                    Console.WriteLine(CurrentPlayer
                                      + " now has "
                                      + CurrentPlayer.Purse
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
                CurrentPlayer.AddOneGold();
                Console.WriteLine(CurrentPlayer
                                  + " now has "
                                  + CurrentPlayer.Purse
                                  + " Gold Coins.");

                var winner = DidPlayerWin();
                IncrementCurrentPlayer();

                return winner;
            }
        }

        public Player? WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(CurrentPlayer + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            IncrementCurrentPlayer();
            return null;
        }

        private Player? DidPlayerWin()
        {
            return CurrentPlayer.Purse == 6 ? CurrentPlayer : default;
        }
    }

}
