using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class HyperCanvas : MonoBehaviour
{
    [SerializeField] Vector3 sharedPosition;
    [SerializeField] Vector3 differentPositionLeft;
    [SerializeField] Vector3 differentPositionRight;
    public List<GameObject> _pages = new List<GameObject>();
    void Start()
    {
        //add all children to list
        foreach (Transform child in transform)
        {
            _pages.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }   
    }

    public void ShowCanvas(int index, bool isSecond)
    {
        if (isSecond)
        {
            Debug.Log("Showing canvas " + index);
            _pages[index].transform.position += new Vector3(0, 2, 0);
            _pages[index].SetActive(true);
        }
        else
        {
            Debug.Log("Showing canvas " + index);
            _pages[index].SetActive(true);
        }

    }

    public void HideCanvas(int index, bool isSecond)
    {
        Debug.Log("Hiding canvas " + index);

        _pages[index].transform.position = sharedPosition;
        _pages[index].transform.localScale = new Vector3(1, 1, 1);
        _pages[index].SetActive(false);
    }

    public void RevealAnswer(int firstCanvas, int secondCanvas, bool isDifferent)
    {   
        if (isDifferent)
        {
            _pages[firstCanvas].transform.position = differentPositionRight;
            _pages[firstCanvas].transform.localScale = new Vector3(.5f, .5f, 1f);
            _pages[firstCanvas].SetActive(true);

            _pages[secondCanvas].transform.position = differentPositionLeft;
            _pages[secondCanvas].transform.localScale = new Vector3(.5f, .5f, 1f);
            _pages[secondCanvas].SetActive(true);
        }
        else
        {
            _pages[firstCanvas].transform.position = sharedPosition;
            _pages[firstCanvas].transform.localScale = new Vector3(1, 1, 1);
            _pages[firstCanvas].SetActive(true);
        }
    }
}
