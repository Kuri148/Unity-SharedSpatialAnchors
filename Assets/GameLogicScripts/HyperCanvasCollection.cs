using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class HyperCanvasCollection : MonoBehaviour
{
    public bool isRoundFinished = true;
    public int topic;
    public bool isDifferent;
    public int firstCanvas;
    public int secondCanvas;
    int currentRound = 0;

    [SerializeField] List<HyperCanvas> _hyperCanvases = new List<HyperCanvas>();
    void Start()
    {
        //add all children to list
        foreach (Transform child in transform)
        {
            HyperCanvas hyperCanvas = child.GetComponent<HyperCanvas>();
            if (hyperCanvas != null)
            {
                _hyperCanvases.Add(hyperCanvas);
            }
        }
        
    }

    public void PrepareCanvas()
    {
        if (isRoundFinished)
            topic = Random.Range(0, _hyperCanvases.Count);
            isDifferent = Random.Range(0, 2) == 0;
            firstCanvas = Random.Range(0, 4);
            if (!isDifferent)
            {
                secondCanvas = firstCanvas;
            }
            else
            {
                Debug.Log("Different");
                int count = 0;
                while (secondCanvas == firstCanvas)
                {
                    Debug.Log("Different entered " + count + " times.");
                    secondCanvas = Random.Range(0, 4);
                    count++;
                }
            }
    }
    public void DemandShowCanvas()
    {
        if (isRoundFinished)
        {
            _hyperCanvases[topic].ShowCanvas(firstCanvas, false);
            if (isDifferent)
            {
                _hyperCanvases[topic].ShowCanvas(secondCanvas, true);
            }
        }
        isRoundFinished = false;
    }

        public void DemandHideCanvas()
        {
            Debug.Log("DemandHideCanvas");
            if (!isRoundFinished)
            {
                Debug.Log("DemandHideCanvas entered");
                _hyperCanvases[topic].HideCanvas(firstCanvas, false);
                if (isDifferent)
                {
                    _hyperCanvases[topic].HideCanvas(secondCanvas, true);
                }
            }
            isRoundFinished = true;
        }
}
