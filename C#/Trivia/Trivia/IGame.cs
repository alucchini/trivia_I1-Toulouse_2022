namespace Trivia
{
    internal interface IGame
    {
        bool Add(string playerName);
        void Roll(int roll);
        bool WasCorrectlyAnswered();
        bool WrongAnswer();
        int HowManyPlayers();
    }
}
