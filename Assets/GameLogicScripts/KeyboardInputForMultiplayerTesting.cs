using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyboardInputForMultiplayerTesting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GestureRecognition GestureRecognition;
    [SerializeField] HyperCanvasCollection HyperCanvasCollection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key was pressed.");
            HyperCanvasCollection.PrepareCanvas();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("O key was pressed.");
            HyperCanvasCollection.DemandShowCanvas();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I key was pressed.");
            GestureRecognition.RightThumbsUp();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("U key was pressed.");
            GestureRecognition.LeftThumbsUp();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Y key was pressed.");
            GestureRecognition.RightThumbsDown();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T key was pressed.");
            GestureRecognition.LeftThumbsDown();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key was pressed.");
            GestureRecognition.LeftOk();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed.");
            GestureRecognition.RightOk();
        }
    }
}
