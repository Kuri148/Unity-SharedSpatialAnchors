using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningAboutLists : MonoBehaviour
{
    public List<int> myIntList = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        //add variable to list
        myIntList.Add(1);
        myIntList.Add(2);
        myIntList.Add(3);

        //read varible from list
        Debug.Log("The first item in list is " + myIntList[0]);
        
        //get length of list
        Debug.Log("Count before removal is " + myIntList.Count);

        //remove variable from list
        myIntList.Remove(2);

        //get length of list
        Debug.Log("Count after removal is " + myIntList.Count);

        //list all variables in list
        for (int i = 0; i < myIntList.Count; i++)
        {
            Debug.Log("Item " + i + " in list is " + myIntList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
