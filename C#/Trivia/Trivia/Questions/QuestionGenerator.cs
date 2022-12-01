namespace Trivia.Questions
{
    internal class QuestionGenerator
    {
        private ulong _index;
        private readonly string _category;

        public string NextQuestion => $"{_category} Question {++_index}";

        public QuestionGenerator(string category)
        {
            _category = category;
        }

        private QuestionGenerator(Memento memento)
        {
            _index = memento.Index;
            _category = memento.Category;
        }

        public IMemento<QuestionGenerator> Save() => new Memento(_category, _index);

        private class Memento : IMemento<QuestionGenerator>
        {
            public readonly ulong Index;
            public readonly string Category;

            public Memento(string category, ulong index)
            {
                Category = category;
                Index = index;
            }

            /// <inheritdoc />
            public QuestionGenerator Restore() => new(this);
        }
    }
}
