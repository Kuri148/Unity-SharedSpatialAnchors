using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField]
    private SpeedManager _speedManager;
    public static SpeedManager SpeedManager {get {return Instance._speedManager;}}

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FirstInitialize()
    {
    Debug.Log("This message will output before Awake.");

    }
}

