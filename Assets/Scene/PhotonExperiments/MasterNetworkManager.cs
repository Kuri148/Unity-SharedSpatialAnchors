using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/MasterNetworkManager")]
public class MasterNetworkManager : SingletonScriptableObject<MasterNetworkManager>
{
    private GameSettings _gameSettings;
    public GameSettings GameSettings {get {return Instance._gameSettings;}}

}
