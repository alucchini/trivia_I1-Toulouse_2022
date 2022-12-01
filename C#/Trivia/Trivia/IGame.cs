namespace Trivia
{
    public interface IGame<out TGame>
    {
        bool Add(string playerName);
        void Roll(int roll);
        Player? WasCorrectlyAnswered();
        Player? WrongAnswer();
        int HowManyPlayers();

        IGame<TGame> GameWithoutAPlayer(Player playerToRemove);
        IGameMemento<TGame> Save();
    }
}
