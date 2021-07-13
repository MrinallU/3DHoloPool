using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private GameObject instructions;

    // Start is called before the first frame update
    void Start()
    {
        instructions = GameObject.FindWithTag("instructions");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPressed()
    {
        Debug.Log("ButtonPressed");
        SceneManager.UnloadScene("Start");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    public void InstructionsPressed()
    {
        Debug.Log("InstructionsPressed");
        instructions.SetActive(!instructions.activeInHierarchy);
    }
}
