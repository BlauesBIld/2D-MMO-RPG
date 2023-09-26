using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreenController : MonoBehaviour
{
    public GameObject registerScreen;

    public InputField usernameField;
    public InputField passwordField;
    public InputField confirmPasswordField;
    public Text usernameErrorText;
    public Text passwordErrorText;
    public Text confirmPasswordErrorText;
    public Button registerButton;
    public Button backButton;
    public TextMeshProUGUI signingUp;
    public TextMeshProUGUI errorText;

    private bool isWaitingForResponse = false;
    private bool responseStatus = false;
    private bool registrationResult;
    private string responseMessage;

    private void Start()
    {
        errorText.text = "";
        signingUp.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (usernameField.isFocused)
            {
                passwordField.Select();
            }
            else if (passwordField.isFocused)
            {
                confirmPasswordField.Select();
            }
            else if (confirmPasswordField.isFocused)
            {
                usernameField.Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RegisterOnServer();
        }
    }

    public void RegisterOnServer()
    {
        if (PreRegistrationFieldValidation())
        {
            string clientSalt = CryptoMethods.GetRandomSalt();
            string clientHashedAndSaltedPW = CryptoMethods.GetSHA256(passwordField.text + clientSalt);

            ClientSend.RegisterUserLogin(usernameField.text, clientHashedAndSaltedPW, clientSalt);

            SwitchToWaitingScreen();
        }
        else
        {
            errorText.gameObject.SetActive(true);
        }
    }

    private bool PreRegistrationFieldValidation()
    {
        bool passedCheck = true;

        if (usernameField.text == "")
        {
            usernameErrorText.text = "Required Field!";
            passedCheck = false;
        }
        else
        {
            usernameErrorText.text = "";
        }

        if (passwordField.text == "")
        {
            passwordErrorText.text = "Required Field!";
            passedCheck = false;
        }
        else
        {
            passwordErrorText.text = "";
        }

        if (passwordField.text != confirmPasswordField.text)
        {
            confirmPasswordErrorText.text = "Passwords do not match!";
            passedCheck = false;
        }
        else
        {
            confirmPasswordErrorText.text = "";
        }

        return passedCheck;
    }

    private void SwitchToWaitingScreen()
    {
        DeactivateFieldsAndButtons();

        signingUp.gameObject.SetActive(true);

        ResetResponseStatusAndErrorText();

        StartCoroutine(StartWaitingForResponse());
    }

    private void ResetResponseStatusAndErrorText()
    {
        responseStatus = false;
        errorText.text = "";
    }

    private IEnumerator StartWaitingForResponse()
    {
        if (isWaitingForResponse)
        {
            yield break;
        }

        isWaitingForResponse = true;

        signingUp.text = "Signing up";
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {
            if (responseStatus)
            {
                HandleRegistrationResponse();

                yield break;
            }

            for (int j = 0; j < 3; j++)
            {
                signingUp.text += ".";
                yield return new WaitForSeconds(0.3f);
            }
        }

        isWaitingForResponse = false;

        ReactivateFieldsAndButtons();
        signingUp.gameObject.SetActive(false);
        errorText.text = "Failed to Connect to Server!";
    }

    private void HandleRegistrationResponse()
    {
        if (registrationResult)
        {
            ReactivateFieldsAndButtons();
            signingUp.gameObject.SetActive(false);
            BackToLoginScreen();
            isWaitingForResponse = false;
        }
        else
        {
            ReactivateFieldsAndButtons();
            signingUp.gameObject.SetActive(false);
            errorText.text = responseMessage;
            isWaitingForResponse = false;
        }
    }

    private void DeactivateFieldsAndButtons()
    {
        usernameField.gameObject.SetActive(false);
        passwordField.gameObject.SetActive(false);
        confirmPasswordField.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    private void ReactivateFieldsAndButtons()
    {
        usernameField.gameObject.SetActive(true);
        passwordField.gameObject.SetActive(true);
        confirmPasswordField.gameObject.SetActive(true);
        registerButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void BackToLoginScreen()
    {
        usernameField.text = "";
        passwordField.text = "";
        confirmPasswordField.text = "";
        gameObject.SetActive(false);
        UIManager.instance.GetLoginScreenController.informationText.text = responseMessage;
        UIManager.instance.GetLoginScreenController.loginScreen.SetActive(true);
    }

    public void ReceiveResponse(bool successful, string message)
    {
        responseStatus = true;
        registrationResult = successful;
        responseMessage = message;
    }
}