using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    //Public Variables
    public GameObject gameMenu;
    public bool breakAndShootBool;

    // Private Variables
    private GameObject cue;
    private GameObject triangle;
    private GameObject poolTable;
    private GameObject gamePieceHolder;
    private GameObject instructions;
    private bool adjustTableBool;

    // Start is called before the first frame update
    void Start()
    {
        cue = GameObject.FindWithTag("cue");
        triangle = GameObject.FindWithTag("triangle");
        poolTable = GameObject.FindWithTag("table");
        gamePieceHolder = GameObject.FindWithTag("gamepiece");
        instructions = GameObject.FindWithTag("instructions");
        if (gameMenu != null)
        {
            gameMenu.SetActive(false);
        }
        breakAndShootBool = false;
        adjustTableBool = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartPressed()
    {
        SceneManager.UnloadSceneAsync("Start");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    public void AboutPressed()
    {
        Application.OpenURL("https://3dhologroup.com/");
    }

    public void ExitPressed()
    {
        Debug.Log("ExitPressed");
        Application.Quit();
    }

    public void ExitGamePressed()
    {
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadScene("Start", LoadSceneMode.Additive);
    }

    public void BreakAndShootPressed()
    {

        breakAndShootBool = !breakAndShootBool;
        triangle.GetComponent<Triangle>().RemoveTriangle();
        cue.GetComponent<MoveAxisConstraintShootMode>().enabled = breakAndShootBool;
        cue.GetComponent<MoveAxisConstraint>().enabled = !breakAndShootBool;
        cue.GetComponent<CueController>().InitiateShooting(breakAndShootBool);
    }

    public void AdjustTablePressed()
    {
        adjustTableBool = !adjustTableBool;
        gamePieceHolder.SetActive(!gamePieceHolder.activeInHierarchy);
        poolTable.GetComponent<ObjectManipulator>().enabled = adjustTableBool;
    }

    public void GameMenuPressed()
    {
        Debug.Log("GameMenuPressed");
        gameMenu.SetActive(!gameMenu.activeInHierarchy);
    }

    public void InstructionsPressed()
    {
        instructions.SetActive(!instructions.activeInHierarchy);
    }

    public void ResetCuePressed()
    {
        if (breakAndShootBool)
        {
            BreakAndShootPressed();
        }
        cue.transform.localPosition = new Vector3(-28.9f, -4.8f, -26.7f);
        //cue.GetComponent<Rigidbody>().freezeRotation = false;
        cue.transform.localRotation = cue.GetComponent<CueController>().GetStartRotation();
        cue.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cue.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
