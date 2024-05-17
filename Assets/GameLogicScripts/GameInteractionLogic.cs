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

public class GameInteractionLogic : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI masterAnswerText;
    [SerializeField] TextMeshProUGUI clientAnswerText;
    [SerializeField] TextMeshProUGUI agreementText;
    [SerializeField] TextMeshProUGUI masterConfirmationText;
    [SerializeField] TextMeshProUGUI clientConfirmationText;
    [SerializeField] TextMeshProUGUI confirmationText;
    [SerializeField] TextMeshProUGUI masterStartNextText;
    [SerializeField] TextMeshProUGUI clientStartNextText;
    [SerializeField] TextMeshProUGUI correctIncorrectText;

    [SerializeField] TextMeshProUGUI[] gestureTexts = new TextMeshProUGUI[9];

    [SerializeField] RoomAffluence RoomAffluence;
    [SerializeField] HyperCanvasCollection HyperCanvasCollection;


    [SerializeField] bool playersSayCanvasesAreDifferent;
    

    [SerializeField] bool twoPlayersPresent = false;

    [SerializeField] bool nextRoundConsentGiven = false;
    [SerializeField] bool nextRoundClientConsent = false;
    [SerializeField] bool nextRoundMasterConsent = false;

    private bool isFirstRound = true;
    
    
    public GameObject NetworkCapsule;

    public PhotonView gesturePhotonView;
    public PhotonView hypercavasPhotonView;
 
    private void Start()
    {
        gestureTexts[0] = masterAnswerText;
        gestureTexts[1] = masterConfirmationText;        
        gestureTexts[2] = masterStartNextText;  
        gestureTexts[3] = clientAnswerText;
        gestureTexts[4] = clientConfirmationText;
        gestureTexts[5] = clientStartNextText;
        gestureTexts[6] = agreementText;
        gestureTexts[7] = confirmationText;
        gestureTexts[8] = correctIncorrectText;
    }

//Starting and moving onto the next round ////
    public void StartNextText()
    {
        //Round is not in progress
        Debug.Log("Is it during the round?" + HyperCanvasCollection.GetIsDuringRound());
        if (HyperCanvasCollection.GetIsDuringRound() == true) return;
        //Both Players are present and consenting
        if (!TwoPlayerConsent()) return; 
        Debug.Log("TwoPlayerConsent");
        //Both players are present and have said "Ready" 
        if (BothPlayersWantToMoveOn())
        {
            Debug.Log("BothPlayersWantToMoveOn");
            ClearTexts(true);

            if (!isFirstRound) HyperCanvasCollection.DemandHideCanvas();
            
            HyperCanvasCollection.PrepareCanvas();
            gesturePhotonView.RPC("ResetConsentFlagsRPC", RpcTarget.All);
        }
    }
    public bool TwoPlayerConsent()
    {
        //The following two bool flags are preemptory to the third bool flag.  If either of them are false, the third bool flag will be false.

        if (PhotonNetwork.IsMasterClient)
        {
            gesturePhotonView.RPC("MasterConsent", RpcTarget.All);
            Debug.Log("MasterClientWantsAgain");
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            gesturePhotonView.RPC("ClientConsent", RpcTarget.All);
            Debug.Log("ClientWantsAgain");
        }

        //this sets the bool flag in the first if statement.
        if (nextRoundMasterConsent && nextRoundClientConsent)
        {
            gesturePhotonView.RPC("BothPlayersConsentRPC", RpcTarget.All);
            return true;
        }
        // Returns false if the two players are not present.
        return false;
    }

    [PunRPC]
    public void ResetConsentFlagsRPC()
    {
        nextRoundMasterConsent = false;
        nextRoundClientConsent = false;
        nextRoundConsentGiven = false;
    }

    private bool BothPlayersWantToMoveOn()
    {
        if (masterStartNextText.text == clientStartNextText.text && masterStartNextText.text != "" && clientStartNextText.text != "")
        {
            return true;
        }
        return false;
    }

    [PunRPC]
    public void MasterConsent()
    {
        nextRoundMasterConsent = true;
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 0, "Ready");
    }

    [PunRPC]
    public void ClientConsent()
    {
        nextRoundClientConsent = true;
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 3, "Ready");

    }

    [PunRPC]
    public void BothPlayersConsentRPC()
    {
        gestureTexts[1].text = "Is the picture you are looking at the";
        gestureTexts[4].text = "Is the picture you are looking at the";
        nextRoundConsentGiven = true;
        isFirstRound = false;
    }

//End of starting sequence

//Show a debug capsule

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

//player actions chose through UI

    public void VoteForSame()
    {
        MasterClientUIDiverter(0, "Same");
        ShowNetworkCapsuleRPC();
    }

    public void VoteForDifferent()
    {
        MasterClientUIDiverter(0, "Different");
    }

    public void ConfirmAnswer()
    {
        MasterClientUIDiverter(1, "Confirmed");
    }

    //answer is 0. confirmation is 1. agreement is 2. left confirmation is 3. 

    private void MasterClientUIDiverter(int textIndex, string textString)
    {
        if (!PhotonNetwork.IsMasterClient) textIndex += 3;
            ChangeText(textIndex, textString);
    }

    //left is 0. right is 1. agreement is 2. left confirmation is 3. right confirmation is 4. total confirmation is 5.
    private void ChangeText(int textIndex, string newText)
    {
        
        if ((textIndex == 1 || textIndex == 4) && CheckForAgreement() && HyperCanvasCollection.GetIsDuringRound() == true)
        {
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, textIndex, newText);
            CheckForConfirmation();
            return;
        }
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, textIndex, newText);
        CheckForAgreement();
    }

    [PunRPC]
    public void ChangeTextRPC(int textIndex, string newText)
    {
        gestureTexts[textIndex].text = newText;
    }



    private bool CheckForAgreement()
    {
        if (masterAnswerText.text == clientAnswerText.text  && masterAnswerText.text != "" && clientAnswerText.text != "")
        {
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 6, "Confirm?");
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
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 1, "");
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 2, "");
        
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 4, "");
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 5, "");

        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 6, "");
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 7, "");
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 8, "");

        if (fullClear)
        {
            //masterAnswerText.text
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 0, "Same or diffent?");
            //clientAnswerText.text
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 3, "Different or same?");
        }
    }

    private void CheckForConfirmation()
    {
        Debug.Log("CheckForConfirmation");
        if (agreementText.text == "Confirm?" && masterConfirmationText.text == "Confirmed" && clientConfirmationText.text == "Confirmed")
        {
            Debug.Log("Confirmation Entered");
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 6, "Confirmed!");
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 1, "Hit Start/Next");
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 4, "Hit Start/Next");
            HyperCanvasCollection.SetIsDuringRound(false);
            DefinePlayersSayCanvasesAreDifferent();
            CheckForCorrectness();
        }
    }

    private void DefinePlayersSayCanvasesAreDifferent()
    {
        if (masterAnswerText.text == "Different")
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
        
        bool playersGotItRight = false;
        string feedback = "";
        string state = "";
        state = (HyperCanvasCollection.GetIsDifferent() == true)? "different" : "same";

        if (playersSayCanvasesAreDifferent == HyperCanvasCollection.GetIsDifferent())
        {
            Debug.Log("Correct");
            feedback = "Correct, " + state + "!";
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 6, feedback);
            playersGotItRight = true;
        }
        else
        {
            Debug.Log("Incorrect");
            feedback = "Incorrect, " + state + "!";
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 6, feedback);
            playersGotItRight = false;
        }
        gesturePhotonView.RPC("ChangeAffluenceValueRPC", RpcTarget.All, playersGotItRight);
        //Allow for the next round to begin
        gesturePhotonView.RPC("ResetConsentFlagsRPC", RpcTarget.All);
        //Show the only or both pictures
        HyperCanvasCollection.DemandRevealAnswer();
    }

    [PunRPC]
    public void ChangeAffluenceValueRPC(bool playersWereCorrect)
    {
        RoomAffluence.SetAffluence(playersWereCorrect);
    }
}
