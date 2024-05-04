using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperCanvas : MonoBehaviour
{
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
            _pages[index].transform.position += new Vector3(0, 2, 0);
        }
        _pages[index].SetActive(true);
    }
    public void HideCanvas(int index, bool isSecond)
    {
        Debug.Log("Hiding canvas " + index);
        if (isSecond)
        {
            Debug.Log("Hiding second canvas");
            _pages[index].transform.position -= new Vector3(0, 2, 0);
        }
        _pages[index].SetActive(false);
    }
}
