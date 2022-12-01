using System;
using System.Collections.Generic;

namespace Trivia
{
    public static class GameRunner
    {
        private static Player? _winner;
        private static readonly Queue<Player> Leaderboard = new ();

        public static void Main()
        {
            IGame<GameEnforcingMaxPlayers<GameWhichHasEnoughPlayers<Game>>> aGame =
                new GameEnforcingMaxPlayers<GameWhichHasEnoughPlayers<Game>>(
                    new GameWhichHasEnoughPlayers<Game>(
                        new Game()
                    )
                );

            aGame.Add("Ozzy");
            aGame.Add("Lemmy");
            aGame.Add("Tony");

            var rand = new Random();

            while (aGame.NumberOfPlayers >= Configuration.NombreMinimalJoueurs)
            {
                do
                {
                    aGame.Roll(rand.Next(5) + 1);

                    _winner = rand.Next(9) == 7 ? aGame.WrongAnswer() : aGame.WasCorrectlyAnswered();

                    aGame = aGame.Save().Restore();
                } while (_winner == default);

                Leaderboard.Enqueue(_winner);
                aGame = aGame.GameWithoutAPlayer(_winner);
                _winner = default;
            }

            Console.WriteLine("LEADERBOARD");
            var current = 1;
            while (Leaderboard.TryDequeue(out var player))
            {
                Console.WriteLine($"{current} - {player}");
                current++;
            }
        }
    }
}