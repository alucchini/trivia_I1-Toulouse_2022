namespace Trivia
{
    public interface IMemento<out TSaved>
    {
        TSaved Restore();
    }
}
