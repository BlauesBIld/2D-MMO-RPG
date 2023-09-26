using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject inGameMenu;

    private MonoBehaviour[] playerScriptComponents;

    private ConnectingToServerScreenController connectingToServerScreenController;
    private CornerPlayerInterfaceController cornerPlayerInterfaceController;
    private GameInterfaceController gameInterfaceController;
    private LoginScreenController loginScreenController;
    private RegisterScreenController registerScreenController;
    private MinimapController minimapController;
    private InventoryController inventoryController;

    public RegisterScreenController GetRegisterScreenContorller => registerScreenController;

    public LoginScreenController GetLoginScreenController => loginScreenController;

    public ConnectingToServerScreenController GetConnectingToServerScreenController =>
        connectingToServerScreenController;

    public CornerPlayerInterfaceController GetCornerPlayerInterfaceController => cornerPlayerInterfaceController;

    public GameInterfaceController GetGameInterfaceController => gameInterfaceController;

    public InventoryController GetInventoryController => inventoryController;

    private void Awake()
    {
        if (instance == null)
        {
            InitializeController();

            instance = this;
            inGameMenu.SetActive(false);
            registerScreenController.registerScreen.SetActive(false);
            loginScreenController.loginScreen.SetActive(false);
            connectingToServerScreenController.connectingToServerScreen.SetActive(true);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        SartAttemptToConnect();
    }

    public void SartAttemptToConnect()
    {
        StartCoroutine(connectingToServerScreenController.TryToConnectToServer());
    }

    public void setPlayerScriptComponents(MonoBehaviour[] playerScriptComponents)
    {
        this.playerScriptComponents = playerScriptComponents;
    }

    public void OpenInGameMenu()
    {
        inGameMenu.SetActive(true);
        foreach (MonoBehaviour playerScriptComponent in playerScriptComponents)
        {
            playerScriptComponent.enabled = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        inGameMenu.SetActive(false);
        foreach (MonoBehaviour playerScriptComponent in playerScriptComponents)
        {
            playerScriptComponent.enabled = true;
        }
    }

    private void InitializeController()
    {
        connectingToServerScreenController = GetComponentInChildren<ConnectingToServerScreenController>();
        gameInterfaceController = GetComponentInChildren<GameInterfaceController>();
        cornerPlayerInterfaceController = GetComponentInChildren<CornerPlayerInterfaceController>();
        loginScreenController = GetComponentInChildren<LoginScreenController>();
        registerScreenController = GetComponentInChildren<RegisterScreenController>();
        minimapController = GetComponentInChildren<MinimapController>();
        inventoryController = GetComponentInChildren<InventoryController>();
    }
}