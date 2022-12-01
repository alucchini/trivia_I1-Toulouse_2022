namespace Trivia
{
    public interface IGameMemento<out TGame>
    {
        IGame<TGame> Restore();
    }
}
