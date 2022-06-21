using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperScissor : MonoBehaviour
{
    //public static Dictionary<int, int> towers = new Dictionary<int, int>();
    
    // this could be an ENUM
    public static Dictionary<int, string> moves = new Dictionary<int, string>();

    /// <summary>
    /// Function which computes the outcome of the Rock Paper Scissor minigame. It takes a number as input
    /// where:
    ///     - 0: Rock
    ///     - 1: Scissor
    ///     - 2: Paper
    /// </summary>
    /// <param name="moveIndex">a number corresponding to the desired move</param>
    public static int PlayRPSTurn(int firstTowerMove, int secondTowerMove)
    {

        if (secondTowerMove == firstTowerMove % 3)
        {
            print($"It's a tie!");
            return -1;
        }
        else if (secondTowerMove == (firstTowerMove + 1) % 3)
        {
            print($"First tower wins, Second tower loses: {moves[firstTowerMove]} vs {moves[secondTowerMove]}");
            return 0;
        }
        else
        {
            print($"First tower loses, Second tower wins: {moves[firstTowerMove]} vs {moves[secondTowerMove]}");
            return 1;
        }
    }


    private void Awake()
    {
        moves.Add(0, "Rock");
        moves.Add(1, "Scissor");
        moves.Add(2, "Paper");
    }

}
