using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Oculus.Interaction.Input;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.CompilerServices;

public class HyperCanvasCollection : MonoBehaviour
{
    private bool _isDuringRound = false;
    public int topic;
    private bool isDifferent;
    public int firstCanvas;
    public int secondCanvas;
    int currentRound = 0;

    [SerializeField] List<HyperCanvas> _hyperCanvases = new List<HyperCanvas>();

    public PhotonView hyperCanvasCollectionPhotonView;

    
    public bool GetIsDifferent()
    {
        return isDifferent;
    }

    public void SetIsDuringRound(bool value)
    {
        hyperCanvasCollectionPhotonView.RPC("SetIsDuringRoundRPC", RpcTarget.All, value);
    }

    [PunRPC]
    public void SetIsDuringRoundRPC(bool value)
    {
        _isDuringRound = value;
    } 

    public bool GetIsDuringRound()
    {
        return _isDuringRound;
    }   
    
    void Start()
    {
        //add all children to list
        foreach (Transform child in transform)
        {
            HyperCanvas hyperCanvas = child.GetComponent<HyperCanvas>();
            if (hyperCanvas != null)
            {
                _hyperCanvases.Add(hyperCanvas);
            }
        }
        
    }

//-----------------------------PREPARING CANVAS START-------------------------------------------------------------
//Is called from GameInteractionLogic.cs and prepares the canvas for the players
    public void PrepareCanvas()
    {
        if (!_isDuringRound) //Make sure the players aren't in the middle of a round
        {
            topic = Random.Range(0, _hyperCanvases.Count);
            isDifferent = Random.Range(0, 2) == 0;
            firstCanvas = Random.Range(0, 4);
            
            //Show the same picture to each person
            if (!isDifferent)
            {
                secondCanvas = firstCanvas;
            }
            //Show different pictures to each person
            else
            {
                Debug.Log("Different");
                int count = 0;
                while (secondCanvas == firstCanvas)
                {
                    Debug.Log("Different entered " + count + " times.");
                    secondCanvas = Random.Range(0, 4);
                    count++;
                }
            }
            //Calls the following two RPCS to make sure everyone has the same possible images and shows the appropriate images
            hyperCanvasCollectionPhotonView.RPC("ShareCanvasPreparationRPC", RpcTarget.All, topic, isDifferent, firstCanvas, secondCanvas);
            hyperCanvasCollectionPhotonView.RPC("DemandShowCanvasRPC", RpcTarget.All);
        }
    }


    //This RPC is called to make sure everyone has the same possible images
    [PunRPC]
    public void ShareCanvasPreparationRPC(int topic, bool isDifferent, int firstCanvas, int secondCanvas)
    {
        this.topic = topic;
        this.isDifferent = isDifferent;
        this.firstCanvas = firstCanvas;
        this.secondCanvas = secondCanvas;
    }

    //This RPC is called to show the appropriate images
    [PunRPC]
    public void DemandShowCanvasRPC()
    {
        if (!_isDuringRound)
        {
            _hyperCanvases[topic].ShowCanvas(firstCanvas, false);
            if (isDifferent)
            {
                _hyperCanvases[topic].ShowCanvas(secondCanvas, true);
            }
        }
        //This sets the bool flag to true so that the players can't be interrupted by another round
        hyperCanvasCollectionPhotonView.RPC("SetIsDuringRoundRPC", RpcTarget.All, true);
    }
//-----------------------------PREPARING CANVAS END-------------------------------------------------------------

    public void DemandShowCanvas()
    {
        hyperCanvasCollectionPhotonView.RPC("DemandShowCanvasRPC", RpcTarget.All);
    }
//-----------------------------HIDING CANVAS START-------------------------------------------------------------
        public void DemandHideCanvas()
        {
            hyperCanvasCollectionPhotonView.RPC("DemandHideCanvasRPC", RpcTarget.All);
        }

        [PunRPC]
        public void DemandHideCanvasRPC()
        {
            Debug.Log("DemandHideCanvas " + topic + " " + firstCanvas + " " + secondCanvas);
            if (_isDuringRound == false)
            {
                Debug.Log("DemandHideCanvas entered");
                _hyperCanvases[topic].HideCanvas(firstCanvas, false);
                if (isDifferent)
                {
                    _hyperCanvases[topic].HideCanvas(secondCanvas, true);
                }
            }
        }
//-----------------------------HIDING CANVAS END-------------------------------------------------------------
    
    public void DemandRevealAnswer()
    {
        hyperCanvasCollectionPhotonView.RPC("RevealAnswerRPC", RpcTarget.All);
    }

    [PunRPC]
    public void RevealAnswerRPC()
    {
        if (!_isDuringRound)
        {
            _hyperCanvases[topic].ShowCanvas(firstCanvas, false);
            if (isDifferent)
            {
                _hyperCanvases[topic].ShowCanvas(secondCanvas, true);
            }
        }
        //This sets the bool flag to true so that the players can't be interrupted by another round
        hyperCanvasCollectionPhotonView.RPC("SetIsDuringRoundRPC", RpcTarget.All, false);
    }
}
