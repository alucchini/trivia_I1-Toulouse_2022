using System;
using System.Collections.Generic;

namespace Trivia
{
    public class GameRunner
    {
        private static Player? _winner;
        private static Queue<Player> _leaderboard = new Queue<Player>();

        public static void Main(string[] args)
        {
            IGame aGame = new GameWhichHasEnoughPlayers(new Game());

            aGame.Add("Ozzy");
            aGame.Add("Lemmy");
            aGame.Add("Tony");

            var rand = new Random();

            while (aGame.HowManyPlayers() >= Configuration.NombreMinimalJoueurs)
            {
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

                _leaderboard.Enqueue(_winner);
                aGame = aGame.GameWithoutAPlayer(_winner);
                _winner = default;
            }

            Console.WriteLine("LEADERBOARD");
            var current = 1;
            while (_leaderboard.TryDequeue(out var player))
            {
                Console.WriteLine($"{current} - {player}");
                current++;
            }
        }
    }
}