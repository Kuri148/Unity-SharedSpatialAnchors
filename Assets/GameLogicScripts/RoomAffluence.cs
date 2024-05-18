using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;




public class RoomAffluence : MonoBehaviour, IAffluenceSubject
{
    
    [SerializeField] List<IAffluenceObserver> _observers = new List<IAffluenceObserver>();
    [SerializeField] float _affluence = 0.0f;
    //write getter and setter for affluence
    public float Affluence
    {
        get { return _affluence; }
        set { _affluence = value; }
    }

    public void RegisterObserver(IAffluenceObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void RemoveObserver(IAffluenceObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateAffluence(_affluence);
        }
    }

    public void SetAffluence(bool isPositive)
    {
        if (isPositive)
        {
            _affluence++;
        }
        else
        {
            _affluence--;
        }
        
        if (_affluence < 0)
        {
            _affluence = 0;
        }
        if (_affluence > 10)
        {
            _affluence = 0;
        }
        NotifyObservers();
        //SetAffluenceRPC(isPositive);
    }
    /*
    [PunRPC]
    public void SetAffluenceRPC(bool isPositive)
    {

    }*/
}

