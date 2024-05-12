using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuKeyboard : MonoBehaviour
{
    [SerializeField] MenuController menuController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            menuController.OnLoadDemoScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            menuController.OnLoadDemoScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            menuController.OnLoadDemoScene(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            menuController.OnLoadDemoScene(4);
        }
            
    }
}
