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

    [SerializeField] GameObject NetworkCapsule;

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
        HyperCanvasCollection.PrepareCanvas();
        HyperCanvasCollection.DemandShowCanvas();
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
        //Debug.Log("LeftBunny");
        //ChangeText(0, "Bunny");
    }

    public void RightBunny()
    {
        //Debug.Log("RightBunny");
        //ChangeText(1, "Bunny");
    }

    public void LeftThumbsUp()
    {
        //Debug.Log("LeftThumbsUp");
        ChangeText(0, "Thumbs Up");
    }


    public void RightThumbsUp()
    {
        //Debug.Log("RightThumbsUp");
        ChangeText(1, "Thumbs Up");
        ShowNetworkCapsuleRPC();
    }

    [PunRPC]
    void ShowNetworkCapsule()
    {
        NetworkCapsule.SetActive(true);
        Debug.Log("NetworkCapsuleShown");
    }

    public void ShowNetworkCapsuleRPC()
    {
        photonView.RPC("ShowNetworkCapsule", RpcTarget.All);
        Debug.Log("ShowNetworkCapsuleRPC");
    }

    public void LeftThumbsDown()
    {
        //Debug.Log("LeftThumbsDown");
        ChangeText(0, "Thumbs Down");
    }

    public void RightThumbsDown()
    {
        //Debug.Log("RightThumbsDown");
        ChangeText(1, "Thumbs Down");
    }

    public void RightOk()
    {
        //Debug.Log("RightOk");
        ChangeText(4, "Ok");
    }

    public void LeftOk()
    {
        //Debug.Log("LeftOk");
        ChangeText(3, "Ok");
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
