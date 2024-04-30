using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GestureRecognition : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI leftGestureText;
    [SerializeField] TextMeshProUGUI rightGestureText;
    [SerializeField] TextMeshProUGUI agreementText;
    [SerializeField] TextMeshProUGUI leftConfirmationText;
    [SerializeField] TextMeshProUGUI rightConfirmationText;
    [SerializeField] TextMeshProUGUI confirmationText;

    [SerializeField] TextMeshProUGUI[] gestureTexts = new TextMeshProUGUI[6];

    [SerializeField] RoomAffluence RoomAffluence;

    private void Start()
    {
        gestureTexts[0] = leftGestureText;
        gestureTexts[1] = rightGestureText;
        gestureTexts[2] = agreementText;
        gestureTexts[3] = leftConfirmationText;
        gestureTexts[4] = rightConfirmationText;
        gestureTexts[5] = confirmationText;

    }
    public void LeftBunny()
    {
        //Debug.Log("LeftBunny");
        ChangeText(0, "Bunny");
    }

    public void RightBunny()
    {
        //Debug.Log("RightBunny");
        ChangeText(1, "Bunny");
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
    public void ChangeText(int textIndex, string newText)
    {
        //Debug.Log("ChangeText");
        if (textIndex > 2 && CheckForAgreement())
        {
            gestureTexts[textIndex].text = newText;
            CheckForConfirmation();
            return;
        }
        gestureTexts[textIndex].text = newText;
        CheckForAgreement();
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
            }
    }

    private void CheckForConfirmation()
    {
        Debug.Log("CheckForConfirmation");
        if (agreementText.text == "Agreement" && leftConfirmationText.text == "Ok" && rightConfirmationText.text == "Ok")
        {
            Debug.Log("Confirmation Entered");
            confirmationText.text = "Confirmation";
            RoomAffluence.SetAffluence();
            ClearTexts(true);
        }
    }
}
