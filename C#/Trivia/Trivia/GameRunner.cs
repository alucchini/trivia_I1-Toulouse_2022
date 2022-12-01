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
            var rand = new Random();

            IGame<GameEnforcingMaxPlayers<GameWhichHasEnoughPlayers<Game>>> aGame =
                new GameEnforcingMaxPlayers<GameWhichHasEnoughPlayers<Game>>(
                    new GameWhichHasEnoughPlayers<Game>(
                        new Game(rand.Next() % 2 == 0)
                    )
                );

            aGame.Add("Ozzy");
            aGame.Add("Lemmy");
            aGame.Add("Tony");

            var gameAtStartBackup = aGame.Save();
            bool play;

            do
            {
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

                Console.WriteLine("Replay ?");

                play = rand.Next() % 2 == 0;

                aGame = gameAtStartBackup.Restore();
            } while (play);
        }
    }
}