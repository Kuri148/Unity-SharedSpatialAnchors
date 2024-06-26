using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "1";
    public string GameVersion {get {return _gameVersion;}}

    private string _nickName = "Waffle";
    public string NickName 
    {
        get 
        {
            int value = Random.Range(0, 1000);
            _nickName = "Waffle" + value.ToString();
            return _nickName;
        }
    }
}
