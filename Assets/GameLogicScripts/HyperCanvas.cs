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
        if (PhotonNetwork.IsMasterClient)
        {
            if (isSecond)
            {
                Debug.Log("Showing canvas " + index);
                _pages[index].transform.position += new Vector3(0, 2, 0);
                _pages[index].SetActive(true);
            }
        }
        else
        if (!isSecond)
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

    public void RevealAnswer(int index, bool isSecond, bool isDifferent)
    {
        _pages[index].SetActive(true);
        
        if (isDifferent)
        {
            if (isSecond)
            {
                _pages[index].transform.position = differentPositionRight;
                _pages[index].transform.localScale = new Vector3(.5f, .5f, 1f);
            }
            else
            {
                _pages[index].transform.position = differentPositionLeft;
                _pages[index].transform.localScale = new Vector3(.5f, .5f, 1f);
            }
        }
    }
}
