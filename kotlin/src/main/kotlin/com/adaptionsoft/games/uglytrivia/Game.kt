package com.adaptionsoft.games.uglytrivia

import com.adaptionsoft.games.uglytrivia.console.IConsole
import com.adaptionsoft.games.uglytrivia.console.SystemConsole
import com.adaptionsoft.games.uglytrivia.errors.MinimalGoldRequiredNotEnoughError
import com.adaptionsoft.games.uglytrivia.errors.PlayersNumberError
import java.lang.StringBuilder
import java.util.*
import kotlin.NoSuchElementException
import kotlin.collections.ArrayList

class Game(
    private val console: IConsole = SystemConsole(),
    private val minimalGoldRequired: Int = 6,
    private val shouldReplaceRockByTechno: Boolean = false,
    private val shouldUseExpansionPack: Boolean = false
) {
    private var players = arrayListOf<Player>()

    private var popQuestions = LinkedList<String>()
    private var scienceQuestions = LinkedList<String>()
    private var sportsQuestions = LinkedList<String>()
    private var rockQuestions = LinkedList<String>()
    private var technoQuestions = LinkedList<String>()
    private var rapQuestions = LinkedList<String>()
    private var philoQuestions = LinkedList<String>()
    private var litteratureQuestions = LinkedList<String>()
    private var geoQuestions = LinkedList<String>()

    private var currentPlayerIndex = 0
    private var isGettingOutOfPenaltyBox: Boolean = false

    private var numberOfQuestions = 0

    private lateinit var currentCategory: CAT
    private var isChoosingCategory = false

    enum class CAT {
        POP, SCIENCE, SPORT, ROCK, TECHNO, RAP, PHILO, LITTERATURE, GEO
    }

    init {
        for (i in 0..49) {
            popQuestions.addLast("Pop Question $i")
            scienceQuestions.addLast("Science Question $i")
            sportsQuestions.addLast("Sports Question $i")
            if (shouldReplaceRockByTechno) {
                technoQuestions.addLast("Techno Question $i")
            } else {
                rockQuestions.addLast("Rock Question $i")
            }
        }
    }

    fun getParameters(): String {
        val builder = StringBuilder()
        builder.append("minimalGoldRequired: $minimalGoldRequired")
        builder.append("shouldUseExpansionPack: $shouldUseExpansionPack")
        builder.append("shouldReplaceRockByTechno: $shouldReplaceRockByTechno")
        builder.append("players: ${players.map { it.name }}")
        return builder.toString()
    }

    fun add(newPlayers: ArrayList<Player>) {
        players = newPlayers
        players.forEachIndexed { index, player ->
            console.println("${player.name} was added")
            console.println("They are player number ${index + 1}")
        }
        throwIfNotPlayable()
    }

    fun roll(roll: Int) {
        console.println("${players[currentPlayerIndex].name} is the current player")
        console.println("They have rolled a $roll")

        if (players[currentPlayerIndex].isInPenaltyBox) {
            if (roll % 2 != 0) {
                isGettingOutOfPenaltyBox = true

                players[currentPlayerIndex].isInPenaltyBox = false
                console.println("${players[currentPlayerIndex].name} is getting out of the penalty box")
                players[currentPlayerIndex].place = players[currentPlayerIndex].place + roll
                if (players[currentPlayerIndex].place > 11) players[currentPlayerIndex].place = players[currentPlayerIndex].place - 12

                console.println(players[currentPlayerIndex].name
                        + "'s new location is "
                        + players[currentPlayerIndex].place)
                if (isChoosingCategory) {
                    console.println("The category is $currentCategory")
                    isChoosingCategory = false
                } else {
                    console.println("The category is " + currentCategory())
                }
                askQuestion()
            } else {
                console.println("${players[currentPlayerIndex].name} is not getting out of the penalty box")
                isGettingOutOfPenaltyBox = false
            }
        } else {
            players[currentPlayerIndex].place = players[currentPlayerIndex].place + roll
            if (players[currentPlayerIndex].place > 11) players[currentPlayerIndex].place = players[currentPlayerIndex].place - 12

            console.println(players[currentPlayerIndex].name
                    + "'s new location is "
                    + players[currentPlayerIndex].place)
            if (isChoosingCategory) {
                console.println("The category is $currentCategory")
                isChoosingCategory = false
            } else {
                console.println("The category is " + currentCategory())
            }
            askQuestion()
        }

    }

    fun wasCorrectlyAnswered(): Boolean {
        try {
            // Check if player has been removed because doesn't want to answer
            players[currentPlayerIndex]
        } catch (error: IndexOutOfBoundsException) {
            incrementCurrentPlayerIndex()
            if (players.size == 0) {
                return false
            }
            return true
        }
        if (players[currentPlayerIndex].hasJoker) {
            players[currentPlayerIndex].hasJoker = false
            incrementCurrentPlayerIndex()
            return true
        }
        if (players[currentPlayerIndex].isInPenaltyBox) {
            if (isGettingOutOfPenaltyBox) {
                console.println("Answer was correct!!!!")
                players[currentPlayerIndex].correctAnswersStrike++
                players[currentPlayerIndex].purses += players[currentPlayerIndex].correctAnswersStrike
                console.println(players[currentPlayerIndex].name
                        + " now has "
                        + players[currentPlayerIndex].purses
                        + " Gold Coins.")

                val winner = didPlayerNotWin()
                incrementCurrentPlayerIndex()

                return winner
            } else {
                incrementCurrentPlayerIndex()
                return true
            }
        } else {
            console.println("Answer was correct!!!!")
            players[currentPlayerIndex].correctAnswersStrike++
            players[currentPlayerIndex].purses += players[currentPlayerIndex].correctAnswersStrike
            console.println(players[currentPlayerIndex].name
                    + " now has "
                    + players[currentPlayerIndex].purses
                    + " Gold Coins.")

            val winner = didPlayerNotWin()
            currentPlayerIndex++
            if (currentPlayerIndex == players.size) currentPlayerIndex = 0

            return winner
        }
    }

    fun wrongAnswer(): Boolean {
        try {
            // Check if player has been removed because doesn't want to answer
            players[currentPlayerIndex]
        } catch (error: IndexOutOfBoundsException) {
            if (players.size == 0) {
                return false
            }
            incrementCurrentPlayerIndex()
            return true
        }
        if (players[currentPlayerIndex].hasJoker) {
            players[currentPlayerIndex].hasJoker = false
            incrementCurrentPlayerIndex()
            return true
        }
        console.println("Question was incorrectly answered")
        console.println(players[currentPlayerIndex].name + " was sent to the penalty box")
        players[currentPlayerIndex].correctAnswersStrike = 0
        players[currentPlayerIndex].isInPenaltyBox = true
        chooseNextCategory()

        incrementCurrentPlayerIndex()
        return true
    }

    fun chooseNextCategory() {
        isChoosingCategory = true
        /*console.println(players[currentPlayerIndex].name + " choosing category for next one : ")
        var userCATChoice = readln()
        while (!CAT.values().map { it.name }.contains(userCATChoice)) {
            console.println("Please choose between : ${CAT.values().map { it.name }}")
            userCATChoice = readln()
        }
        currentCategory = CAT.valueOf(userCATChoice)*/
        currentCategory = CAT.POP
    }

    private fun askQuestion() {
        if (players[currentPlayerIndex].hasJoker) {
            console.println("${players[currentPlayerIndex].name} uses his joker")
        } else if (players[currentPlayerIndex].doesNotWantToAnswer) {
            console.println("${players[currentPlayerIndex].name} doesn't want to answer and left the game")
            players.removeAt(currentPlayerIndex)
        } else {
            try {
                if (!isChoosingCategory) {
                    currentCategory()
                }
                when (currentCategory) {
                    CAT.POP -> console.println(popQuestions.removeFirst().toString())
                    CAT.SCIENCE -> console.println(scienceQuestions.removeFirst().toString())
                    CAT.SPORT -> console.println(sportsQuestions.removeFirst().toString())
                    CAT.ROCK -> console.println(rockQuestions.removeFirst().toString())
                    CAT.TECHNO -> console.println(technoQuestions.removeFirst().toString())
                    else -> {}
                }
            } catch (exception: NoSuchElementException) {
                fillQuestions()
                askQuestion()
            }
        }
    }

    private fun incrementCurrentPlayerIndex() {
        currentPlayerIndex++
        if (currentPlayerIndex >= players.size) {
            currentPlayerIndex = 0
        }
    }

    private fun currentCategory(): CAT {
        currentCategory = when (players[currentPlayerIndex].place) {
            0, 4, 8 -> CAT.POP
            1, 5, 9 -> CAT.SCIENCE
            2, 6, 10 -> CAT.SPORT
            else -> if (shouldReplaceRockByTechno) CAT.TECHNO else CAT.ROCK
        }
        return currentCategory
    }

    private fun throwIfNotPlayable() {
        if (players.size !in 2..6) throw PlayersNumberError()
        if (minimalGoldRequired < 6) throw MinimalGoldRequiredNotEnoughError()

    }

    private fun didPlayerNotWin(): Boolean = players[currentPlayerIndex].purses < minimalGoldRequired

    private fun fillQuestions() {
        for (i in numberOfQuestions..numberOfQuestions + 49) {
            popQuestions.addLast("Pop Question $i")
            scienceQuestions.addLast("Science Question $i")
            sportsQuestions.addLast("Sports Question $i")
            if (shouldReplaceRockByTechno) {
                technoQuestions.addLast("Techno Question $i")
            } else {
                rockQuestions.addLast("Rock Question $i")
            }
            if (shouldUseExpansionPack) {
                rapQuestions.addLast("Rap Question $i")
                philoQuestions.addLast("Philo Question $i")
                litteratureQuestions.addLast("Litterature Question $i")
                geoQuestions.addLast("Geo Question $i")
            }
        }
        numberOfQuestions += 49
    }
}