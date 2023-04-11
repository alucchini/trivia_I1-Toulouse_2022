package com.adaptionsoft.games.trivia.runner

import com.adaptionsoft.games.uglytrivia.Game
import com.adaptionsoft.games.uglytrivia.Player
import com.adaptionsoft.games.uglytrivia.errors.CouldNotReplayGameError
import java.util.*

object GameRunner {
    private var notAWinner: Boolean = false

    private var lastGame: Game? = null

    fun reset() {
        lastGame = null
    }

    fun replay() {
        if (lastGame == null) {
            throw CouldNotReplayGameError()
        }
        lastGame?.let {
            runGame(
                it,
                shouldUseRandom = true,
                shouldGoInPrison = true,
                hasCorrectAnswered = true,
                shouldStopAfterNumberOfQuestions = 50
            )
        }
    }

    fun runGame(
        game: Game,
        shouldUseRandom: Boolean = true,
        shouldGoInPrison: Boolean = true,
        hasCorrectAnswered: Boolean = true, // = shouldStayInPrison,
        shouldStopAfterNumberOfQuestions: Int = 50
    ) {
        var counter = 0
        val rand = Random()


        do {
            if (shouldUseRandom) {
                game.roll(rand.nextInt(5) + 1)
                notAWinner = if (rand.nextInt(9) == 7) {
                    game.wrongAnswer()
                } else {
                    game.wasCorrectlyAnswered()
                }
            } else {
                if (hasCorrectAnswered) {
                    game.roll(7)
                } else {
                    game.roll(2)
                }
                val secondRoll = if (shouldGoInPrison) 7 else 2
                notAWinner = if (secondRoll == 7) {
                    game.wrongAnswer()
                } else {
                    game.wasCorrectlyAnswered()
                }
                counter++
            }
        } while (notAWinner && counter < shouldStopAfterNumberOfQuestions)
    }
}

fun main(args: Array<String>) {
    val game = Game()
    game.add(arrayListOf(
        Player("Chet"),
        Player("Pat"),
        Player("Sue"),
    ))
    GameRunner.runGame(game)
}
