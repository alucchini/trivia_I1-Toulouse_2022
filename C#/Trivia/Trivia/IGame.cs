namespace Trivia
{
    public interface IGame
    {
        bool Add(string playerName);
        void Roll(int roll);
        Player? WasCorrectlyAnswered();
        Player? WrongAnswer();
        int HowManyPlayers();

        IGame GameWithoutAPlayer(Player playerToRemove);
    }
}
