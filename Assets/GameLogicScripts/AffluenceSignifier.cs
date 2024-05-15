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

    [SerializeField] bool curState = false;
    [SerializeField] bool prevState = false;

    [SerializeField] ParticleSystem lossParticles;
    [SerializeField] ParticleSystem gainParticles;
    [SerializeField] AudioSource gainLossAudioSource;
    [SerializeField] AudioClip gainClip;
    [SerializeField] AudioClip lossClip;
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
        prevState = curState;
        Affluence = affluence;
        curState = Affluence > affluenceThreshold;
        if (prevState == false && curState == true)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("Affluence for " + gameObject.name + "is greater than threshold.");
            gainParticles.transform.position = gameObject.transform.position;
            gainLossAudioSource.transform.position = gameObject.transform.position;
            //gainLossAudioSource.clip = gainClip;
            gainParticles.Play();
            gainLossAudioSource.PlayOneShot(gainClip);
        }
        else if (prevState == true && curState == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log("Affluence for " + gameObject.name + "is lower than threshold.");
            lossParticles.transform.position = gameObject.transform.position;
            gainLossAudioSource.transform.position = gameObject.transform.position;
            //gainLossAudioSource.clip = lossClip;
            lossParticles.Play();
            gainLossAudioSource.PlayOneShot(lossClip);
        }
    }
}

