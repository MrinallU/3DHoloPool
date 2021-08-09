using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.UI;

public class PocketController : MonoBehaviour
{
    // update tags accordingly (stripe, solid, or eightBall) 
    public int score;
    public bool turn;
    public string gameStatus;
    public string playerBallType;
    public TextMeshPro scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        turn = true;
        gameStatus = "playing";
        playerBallType = "NA";
        scoreText = GameObject.FindGameObjectWithTag("scoreText").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStatus == "lost") // eight scored before all of the others
        {
            // lost message message
            // reset game
        }
        else if (gameStatus == "won")
        {
            // congrats message
        }

        GameObject.FindGameObjectWithTag("scoreBoard").transform.localPosition = new Vector3(0, 62.5f, 0);
        Debug.Log("Initial Ball Type " + playerBallType);
        Debug.Log("gameStatus " + gameStatus);
    }

    private void OnTriggerEnter(Collider other)
    {
        updateScore(other.tag);
        if (other.tag == "stripe" || other.tag == "solid" || other.tag == "eightBall")
        {
            other.gameObject.SetActive(false);
        }
        if (gameStatus == "playing") 
        { 
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = gameStatus; // if the game has been won or lost
        }

        changeTurn();
    }

    void updateScore(string bt)
    {
        // prevent cue from being set as player ball type
        if (bt == "cue")
        {
            return;
        }

        // cue ball gets pocketted
        if(bt == "cueBall")
        {
            GameObject cueBall = GameObject.FindGameObjectWithTag("cueBall");
            cueBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            cueBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 
            cueBall.transform.localPosition = new Vector3(-1.1f, 34.14f, -13.200001f);
            return; 
        }

        if (score == 0)
        {
            initBallType(bt);
        }

        // normal update
    
        if (bt == "eightBall")
        {
            if (score < 7)
            {
                gameStatus = "lost";
            } else
            {
                gameStatus = "won";
            }
        } else if (bt == playerBallType)
        {
            score++;
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
        if(bt == "eightBall")
        {
            gameStatus = "lost"; 
            return; 
        }

        playerBallType = bt;
    }
}