using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectingToServerScreenController : MonoBehaviour
{
    public GameObject gameInterface;

    public GameObject connectingToServerScreen;
    public GameObject retryButton;
    public GameObject quitButton;
    public TextMeshProUGUI textField;

    private bool isConnectingCoroutineRunning = false;

    public void SetButtonActive(bool setActive)
    {
        retryButton.SetActive(setActive);
        quitButton.SetActive(setActive);
    }

    public void SetScreenActive(bool setActive)
    {
        connectingToServerScreen.SetActive(setActive);
    }

    public void SetText(string msg)
    {
        textField.text = msg;
    }

    public void AddToText(string msg)
    {
        textField.text += msg;
    }

    public IEnumerator TryToConnectToServer()
    {
        if (isConnectingCoroutineRunning)
        {
            yield break;
        }

        isConnectingCoroutineRunning = true;

        if (!Client.instance.IsConnected())
        {
            Client.instance.ConnectToServer();
        }

        for (int i = 1; i <= 5; i++)
        {
            SetButtonActive(false);
            SetText("Connecting to Server");

            yield return new WaitForSeconds(1f);

            if (Client.instance.IsConnected())
            {
                SetScreenActive(false);
                isConnectingCoroutineRunning = false;
                UIManager.instance.GetLoginScreenController.loginScreen.SetActive(true);
                yield break;
            }

            Debug.Log($"Trying to connect attempt {i}...");
            for (int j = 0; j < 3; j++)
            {
                AddToText(".");
                yield return new WaitForSeconds(1f);
            }
        }

        isConnectingCoroutineRunning = false;

        SetText("Connection failed!");
        SetButtonActive(true);
        Debug.Log("Trying to connect failed!");
    }
}