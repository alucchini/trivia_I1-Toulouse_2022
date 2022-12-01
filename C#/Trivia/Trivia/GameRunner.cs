using System;

namespace Trivia
{
    public class GameRunner
    {
        private static Player? _winner;

        public static void Main(string[] args)
        {
            IGame aGame = new GameWhichHasEnoughPlayers(new Game());

            aGame.Add("Ozzy");
            aGame.Add("Lemmy");
            aGame.Add("Tony");

            var rand = new Random();

            do
            {
                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    _winner = aGame.WrongAnswer();
                }
                else
                {
                    _winner = aGame.WasCorrectlyAnswered();
                }
            } while (_winner == default);

            Console.WriteLine(_winner + " a gagné.");
        }
    }
}