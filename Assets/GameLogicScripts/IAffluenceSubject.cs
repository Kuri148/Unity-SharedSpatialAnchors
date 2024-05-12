using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffluenceSubject
{
    void RegisterObserver(IAffluenceObserver observer);
    void RemoveObserver(IAffluenceObserver observer);
    void NotifyObservers();
}

