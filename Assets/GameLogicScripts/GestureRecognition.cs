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

public class GestureRecognition : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI leftGestureText;
    [SerializeField] TextMeshProUGUI rightGestureText;
    [SerializeField] TextMeshProUGUI agreementText;
    [SerializeField] TextMeshProUGUI leftConfirmationText;
    [SerializeField] TextMeshProUGUI rightConfirmationText;
    [SerializeField] TextMeshProUGUI confirmationText;
    [SerializeField] TextMeshProUGUI leftOneMoreTimeText;
    [SerializeField] TextMeshProUGUI rightOneMoreTimeText;
    [SerializeField] TextMeshProUGUI correctIncorrectText;

    [SerializeField] TextMeshProUGUI[] gestureTexts = new TextMeshProUGUI[9];

    [SerializeField] RoomAffluence RoomAffluence;
    [SerializeField] HyperCanvasCollection HyperCanvasCollection;


    [SerializeField] bool playersSayCanvasesAreDifferent;
    [SerializeField] bool masterClientWantsToStart = false;
    [SerializeField] bool clientWantsToStart = false;
    [SerializeField] bool twoPlayersPresent = false;

    public GameObject NetworkCapsule;

    public PhotonView gesturePhotonView;
 
    private void Start()
    {
        gestureTexts[0] = leftGestureText;
        gestureTexts[1] = rightGestureText;
        gestureTexts[2] = agreementText;
        gestureTexts[3] = leftConfirmationText;
        gestureTexts[4] = rightConfirmationText;
        gestureTexts[5] = confirmationText;
        gestureTexts[6] = leftOneMoreTimeText;
        gestureTexts[7] = rightOneMoreTimeText;
        gestureTexts[8] = correctIncorrectText;
    }

    public void MasterClientEntered()
    {
        Debug.Log("MasterClient has Entered");
        masterClientWantsToStart = true;
    }

    public void NotMasterClientEntered()
    {
        Debug.Log("NOTMasterClient has Entered");
        clientWantsToStart = true;
    }

    private void CheckIfBothPeopleWantToStart()
    {
        if (masterClientWantsToStart && clientWantsToStart) twoPlayersPresent = true;
        Debug.Log("Both players are present =" + twoPlayersPresent);
    }

    public void RightOneMoreTime()
    {
        if (HyperCanvasCollection.GetIsDuringRound() == false)
        {
            ChangeText(7, "One More");
        }
        Debug.Log("OneMoreTimeRight");
        if (CheckForOneMore())
        {
            Debug.Log("OneMoreTimeRightEntered");
            ClearTexts(true);
            HyperCanvasCollection.DemandHideCanvas();
            HyperCanvasCollection.PrepareCanvas();
            HyperCanvasCollection.DemandShowCanvas();
        }

    }
    public void LeftOneMoreTime()
    {
        if (HyperCanvasCollection.GetIsDuringRound() == false)
        {
            ChangeText(6, "One More");
        }
        Debug.Log("OneMoreTimeLeft");
        if (CheckForOneMore())
        {
            Debug.Log("OneMoreTimeLeftEntered");
            HyperCanvasCollection.DemandHideCanvas();
            HyperCanvasCollection.PrepareCanvas();
            HyperCanvasCollection.DemandShowCanvas();
            ClearTexts(true);
        }
    }

    public void LeftBunny()
    {
        //This is a bool flag that is set to true when both players are present.  After it is set, the code below will not run.
        if (twoPlayersPresent)
        {
            return;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            gesturePhotonView.RPC("MasterClientLetsGoRPC", RpcTarget.All);
            Debug.Log("MasterClientWantsToStart");
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            gesturePhotonView.RPC("ClientLetsGoRPC", RpcTarget.All);
            Debug.Log("ClientWantsToStart");
        }
        if (masterClientWantsToStart && clientWantsToStart)
        {
            gesturePhotonView.RPC("BothPlayersPresentRPC", RpcTarget.All);
            HyperCanvasCollection.PrepareCanvas();
            HyperCanvasCollection.DemandShowCanvas();
        }
    }

    [PunRPC]
    public void MasterClientLetsGoRPC()
    {
        masterClientWantsToStart = true;
    }

    [PunRPC]
    public void ClientLetsGoRPC()
    {
        clientWantsToStart = true;
    }

    [PunRPC]
    public void BothPlayersPresentRPC()
    {
        twoPlayersPresent = true;
    }

    public void RightBunny()
    {
        LeftBunny();
    }

    public void LeftThumbsUp()
    {
        if(twoPlayersPresent)
        {
            ChangeText(0, "Thumbs Up");
        }
    }


    public void RightThumbsUp()
    {
        if(twoPlayersPresent)
        {
            ChangeText(1, "Thumbs Up");
            ShowNetworkCapsuleRPC();
        }
    }

    [PunRPC]
    public void ShowNetworkCapsule()
    {
        NetworkCapsule.SetActive(true);
        Debug.Log("NetworkCapsuleShown");
    }

    public void ShowNetworkCapsuleRPC()
    {
        gesturePhotonView.RPC("ShowNetworkCapsule", RpcTarget.All);
        Debug.Log("ShowNetworkCapsuleRPC");
    }

    public void LeftThumbsDown()
    {
        if(twoPlayersPresent)
        {
            ChangeText(0, "Thumbs Down");
        }
    }

    public void RightThumbsDown()
    {
        if(twoPlayersPresent)
        {
            ChangeText(1, "Thumbs Down");
        }
    }

    public void RightOk()
    {
        if(twoPlayersPresent)
        {
            ChangeText(4, "Ok");
        }
    }

    public void LeftOk()
    {
        if(twoPlayersPresent)
        {
        ChangeText(3, "Ok");
        }
    }
    //left is 0. right is 1. agreement is 2. left confirmation is 3. right confirmation is 4. total confirmation is 5.
    private void ChangeText(int textIndex, string newText)
    {
        //Debug.Log("ChangeText");
        if ((textIndex == 3 || textIndex == 4) && CheckForAgreement() && HyperCanvasCollection.GetIsDuringRound() == true)
        {
            gestureTexts[textIndex].text = newText;
            CheckForConfirmation();
            return;
        }
        gestureTexts[textIndex].text = newText;
        CheckForAgreement();
    }

    private bool CheckForOneMore()
    {
        if (leftOneMoreTimeText.text == rightOneMoreTimeText.text && leftOneMoreTimeText.text != "" && rightOneMoreTimeText.text != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckForAgreement()
    {
        if (leftGestureText.text == rightGestureText.text  && leftGestureText.text != "" && rightGestureText.text != "")
        {
            agreementText.text = "Agreement";
            return true;
        }
        else
        {
            ClearTexts(false);
            return false;
        }
    }

    private void ClearTexts(bool fullClear)
    {
            agreementText.text = "";
            gestureTexts[3].text = "";
            gestureTexts[4].text = "";
            gestureTexts[5].text = "";
            if (fullClear)
            {
                gestureTexts[0].text = "";
                gestureTexts[1].text = "";
                gestureTexts[2].text = "";
                gestureTexts[6].text = "";
                gestureTexts[7].text = "";
                gestureTexts[8].text = "";
            }
    }

    private void CheckForConfirmation()
    {
        Debug.Log("CheckForConfirmation");
        if (agreementText.text == "Agreement" && leftConfirmationText.text == "Ok" && rightConfirmationText.text == "Ok")
        {
            Debug.Log("Confirmation Entered");
            confirmationText.text = "Confirmation";
            HyperCanvasCollection.SetIsDuringRound(false);
            DefinePlayersSayCanvasesAreDifferent();
            CheckForCorrectness();
        }
    }

    private void DefinePlayersSayCanvasesAreDifferent()
    {
        if (leftGestureText.text == "Thumbs Down")
        {
            playersSayCanvasesAreDifferent = true;
        }
        else
        {
            playersSayCanvasesAreDifferent = false;
        }
    }

    private void CheckForCorrectness()
    {
        bool johnnyTheyDidIt = false;
        if (playersSayCanvasesAreDifferent == HyperCanvasCollection.GetIsDifferent())
        {
            Debug.Log("Correct");
            correctIncorrectText.text = "Correct";
            johnnyTheyDidIt = true;
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("MasterClientEntered");
            }
            else
            {
                Debug.Log("NotMasterClientEntered");
            }

        }
        else
        {
            Debug.Log("Incorrect");
            correctIncorrectText.text = "Incorrect";
            johnnyTheyDidIt = false;
        }
        RoomAffluence.SetAffluence(johnnyTheyDidIt);
    }
}
