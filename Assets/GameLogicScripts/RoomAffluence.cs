using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

    public void SetAffluence()
    {
        _affluence++;
        NotifyObservers();
    }
}

