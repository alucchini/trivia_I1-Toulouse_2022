using System.Collections.Generic;
using System.Linq;

namespace Trivia.Questions
{
    internal class QuestionsDeck
    {
        private readonly IDictionary<string, QuestionGenerator> _questions
            = new Dictionary<string, QuestionGenerator>();

        public QuestionsDeck(params string[] categories)
        {
            foreach (var category in categories)
                _questions.Add(category, new QuestionGenerator(category));
        }

        private QuestionsDeck(Memento memento)
        {
            _questions = memento.Questions.ToDictionary(kv => kv.Key, kv => kv.Value.Restore());
        }

        public string NextQuestionForCategory(string category) => _questions[category].NextQuestion;

        public IMemento<QuestionsDeck> Save() => new Memento(_questions);

        private class Memento : IMemento<QuestionsDeck>
        {
            public readonly IDictionary<string, IMemento<QuestionGenerator>> Questions;

            public Memento(IDictionary<string, QuestionGenerator> questions)
            {
                Questions = questions.ToDictionary(kv => kv.Key, kv => kv.Value.Save());
            }

            /// <inheritdoc />
            public QuestionsDeck Restore() => new (this);
        }
    }
}
