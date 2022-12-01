namespace Trivia
{
    public interface IGame<out TGame>
    {
        bool Add(string playerName);
        void Roll(int roll);
        Player? WasCorrectlyAnswered();
        Player? WrongAnswer();
        int NumberOfPlayers { get; }

        IGame<TGame> GameWithoutAPlayer(Player playerToRemove);
        IMemento<IGame<TGame>> Save();
    }
}
