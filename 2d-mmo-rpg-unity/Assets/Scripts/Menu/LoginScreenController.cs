using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreenController : MonoBehaviour
{
    public GameObject loginScreen;

    public InputField usernameField;
    public InputField passwordField;
    public Button loginButton;
    public Button registerButton;
    public TextMeshProUGUI loggingIn;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI informationText;

    private bool isWaitingForClientSalt;
    private bool responseClientSalt;
    private string receivedClientSalt;

    private bool isWaitingForPasswordCheck;
    private bool responsePasswordCheck;
    private bool successfulPasswordCheck;
    
    private bool isWaitingForInventory;
    private bool responseInventoryCheck;

    private void Start()
    {
        errorText.text = "";
        informationText.text = "";
        loginButton.GetComponentInChildren<Text>().text = "Enter";
        passwordField.gameObject.SetActive(true);
        loggingIn.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(true);
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
                usernameField.Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoginToServer();
        }
    }

    public void LoginToServer()
    {
        errorText.text = "";
        if (PreLoginFieldValidation())
        {
            ClientSend.RequestClientSaltForUser(usernameField.text);
            
            DeactivateFieldsAndButtons();
            ActivateLoggingInText();
            ResetAllWaitingValues();
            
            StartCoroutine(StartWaitingForClientSalt());
        }

        // ClientSend.WelcomeReceived(usernameField.text);
        //
        // loginScreen.SetActive(false);
        // UIManager.instance.GetGameInterfaceController.inGameInterface.SetActive(true);
    }

    private IEnumerator StartWaitingForClientSalt()
    {
        if (isWaitingForClientSalt)
        {
            yield break;
        }

        isWaitingForClientSalt = true;

        for (int i = 0; i < 3; i++)
        {
            if (responseClientSalt)
            {
                if (receivedClientSalt == "")
                {
                    SetErrorText("Account with given username not found!\nRegister an account?");
                    ReactivateFieldsAndButtons();
                    DeactivateLoggingInText();
                }
                else
                {
                    string clientHashedPassword = CryptoMethods.GetSHA256(passwordField.text + receivedClientSalt);
                    ClientSend.RequestPasswordCheck(usernameField.text, clientHashedPassword);
                    StartCoroutine(StartWaitingForClientPassword());
                }

                isWaitingForClientSalt = false;
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }

    }

    private IEnumerator StartWaitingForClientPassword()
    {
        if (isWaitingForPasswordCheck)
        {
            yield break;
        }

        isWaitingForPasswordCheck = true;

        for (int i = 0; i < 3; i++)
        {
            if (responsePasswordCheck)
            {
                if (!successfulPasswordCheck)
                {
                    SetErrorText("Wrong Password or Username!");
                    ReactivateFieldsAndButtons();
                    DeactivateLoggingInText();
                }
                else
                {
                    ClientSend.WelcomeReceived(usernameField.text);
                    ClientSend.RequestInventory(1);
                    ClientSend.RequestLevelAndExperience(1);
                    StartCoroutine(WaitForPlayerInventory());
                }

                isWaitingForClientSalt = false;
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator WaitForPlayerInventory()
    {
        if (isWaitingForInventory)
        {
            yield break;
        }

        isWaitingForInventory = true;
        
        for (int i = 0; i < 3; i++)
        {
            if (responseInventoryCheck)
            {
                SwitchToIngameUI();

                isWaitingForInventory = false;
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private static void SwitchToIngameUI()
    {
        UIManager.instance.GetLoginScreenController.gameObject.SetActive(false);
        UIManager.instance.GetGameInterfaceController.inGameInterface.SetActive(true);
    }

    private bool PreLoginFieldValidation()
    {
        if (usernameField.text == "" || passwordField.text == "")
        {
            errorText.text = "Username or password are empty!";
            return false;
        }

        return true;
    }

    private void LoginOnServer()
    {
        //TODO: Login Request to Server
    }
    
    private void ActivateLoggingInText()
    {
        loggingIn.gameObject.SetActive(true);
    }
    
    private void DeactivateLoggingInText()
    {
        loggingIn.gameObject.SetActive(false);
    }
    
    private void DeactivateFieldsAndButtons()
    {
        usernameField.gameObject.SetActive(false);
        passwordField.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(false);
        loginButton.gameObject.SetActive(false);
    }

    private void ReactivateFieldsAndButtons()
    {
        usernameField.gameObject.SetActive(true);
        passwordField.gameObject.SetActive(true);
        registerButton.gameObject.SetActive(true);
        loginButton.gameObject.SetActive(true);
    }

    public void SetErrorText(string text)
    {
        errorText.text = text;
    }
    
    public void ToRegisterScreen()
    {
        usernameField.text = "";
        passwordField.text = "";
        loginScreen.SetActive(false);
        UIManager.instance.GetRegisterScreenContorller.registerScreen.SetActive(true);
    }

    public void ReceiveClientSalt(string clientSalt)
    {
        responseClientSalt = true;
        receivedClientSalt = clientSalt;
    }
    

    private void ResetAllWaitingValues()
    {
        responseClientSalt = false;
        receivedClientSalt = "";
        responsePasswordCheck = false;
        successfulPasswordCheck = false;
        responseInventoryCheck = false;
    }

    public void ReceivePasswordCheck(bool passwordConfirmed)
    {
        responsePasswordCheck = true;
        successfulPasswordCheck = passwordConfirmed;
    }

    public void InventoryArrived()
    {
        responseInventoryCheck = true;
    }
}