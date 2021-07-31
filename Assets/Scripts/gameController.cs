using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    // apply this later
    public int score;
    public bool turn;
    public string gameStatus;
    public string playerBallType;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        turn = true;
        gameStatus = "playing"; 
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStatus == "lost") // eight scored before all of the others
        {
            // lost message message
            // reset game
        }else if(gameStatus == "won")
        {
            // congrats message
        }
    }

    void updateScore(string bt)
    {
        if(bt == playerBallType)
        {
            score++;
        }else if(bt == "eightBall" && score < 7)
        {
            gameStatus = "lost"; 
        }else if(bt == "eightBall" && score == 7)
        {
            gameStatus = "won"; 
        }
    }

    void changeTurn()
    {
        if (turn)
        {
            turn = false;
        }
        else
        {
            turn = true; 
        }
    }

    void initBallType(string bt)
    {
        playerBallType = bt; 
    }
}
