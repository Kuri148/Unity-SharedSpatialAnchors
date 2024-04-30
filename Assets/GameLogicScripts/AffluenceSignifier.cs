using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using Photon.Realtime;
using UnityEngine;

public class AffluenceSignifier : MonoBehaviour , IAffluenceObserver
{
    [SerializeField] RoomAffluence roomAffluence;
    [SerializeField] float affluenceThreshold;

    [SerializeField] float _affluence = 0.0f;
    public float Affluence
    {
        get { return _affluence; }
        set { _affluence = value; }
    }

    void Start()
    {
        roomAffluence.RegisterObserver(this);
    }

    public void UpdateAffluence(float affluence)
    {
        Affluence = affluence;
        if (Affluence > affluenceThreshold)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("Affluence for " + gameObject.name + "is greater than threshold.");
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log("Affluence for " + gameObject.name + "is lower than threshold.");

        }
    }
}

