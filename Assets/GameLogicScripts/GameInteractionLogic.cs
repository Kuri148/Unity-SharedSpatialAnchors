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

//Starting and moving onto the next round
    public void StartNextText()
    {
        if (!AreThereTwoPlayers()) return;
        if (HyperCanvasCollection.GetIsDuringRound() == false)
        {
            MasterClientUIDiverter(2, "One More");
        }
        if (BothPlayersWantToMoveOn())
        {
            Debug.Log("BothPlayersWantToMoveOn");
            ClearTexts(true);
            HyperCanvasCollection.DemandHideCanvas();
            HyperCanvasCollection.PrepareCanvas();
            gesturePhotonView.RPC("ResetConsentFlags", RpcTarget.All);
        }
    }
    public bool AreThereTwoPlayers()
    {
        
        if (BothPlayersWantToMoveOn()) return true;
    
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
    public void ResetConsentFlags()
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
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 2, "Ready");
    }

    [PunRPC]
    public void ClientConsent()
    {
        nextRoundClientConsent = true;
        gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 5, "Ready");

    }

    [PunRPC]
    public void BothPlayersConsentRPC()
    {
        nextRoundConsentGiven = true;
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
        MasterClientUIDiverter(1, "Ok");
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
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 6, "Agreement");
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
        if (agreementText.text == "Agreement" && masterConfirmationText.text == "Ok" && clientConfirmationText.text == "Ok")
        {
            Debug.Log("Confirmation Entered");
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 7, "Confirmation");
            HyperCanvasCollection.SetIsDuringRound(false);
            DefinePlayersSayCanvasesAreDifferent();
            CheckForCorrectness();
        }
    }

    private void DefinePlayersSayCanvasesAreDifferent()
    {
        if (masterAnswerText.text == "Thumbs Down")
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
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 8, "Correct");
            johnnyTheyDidIt = true;
        }
        else
        {
            Debug.Log("Incorrect");
            gesturePhotonView.RPC("ChangeTextRPC", RpcTarget.All, 8, "Incorrect");
            johnnyTheyDidIt = false;
        }
        RoomAffluence.SetAffluence(johnnyTheyDidIt);
        //Allow for the next round to begin
        nextRoundConsentGiven = false;
    }
}
