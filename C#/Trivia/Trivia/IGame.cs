using System;

namespace Trivia
{
    internal interface IGame
    {
        bool Add(string playerName);
        void Roll(int roll);
        Player? WasCorrectlyAnswered();
        Player? WrongAnswer();
        int HowManyPlayers();
    }
}
