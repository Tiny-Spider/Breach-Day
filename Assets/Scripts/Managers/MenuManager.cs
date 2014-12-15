using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public static MenuManager instance;

    public GameObject mainPanel;
    public GameObject enterNamePanel;
    public GameObject preventInputPanel;

    public Slider soundSlider;
    public Slider musicSlider;
    public Text soundSliderText;
    public Text musicSliderText;

    public Text nameText;

    public InputField directConnectIP;
    public InputField directConnectPort;
    public InputField directConnectPassword;

    public InputField serverName;
    public InputField serverDescription;
    public InputField serverMaxPlayers;
    public InputField serverPort;
    public InputField serverPassword;
    public Toggle serverUseNAT;

    public Text createServerFeedback;
    public Text directConnectFeedback;
    public SaveData[] loadData;
    public string nameSaveTag;

    private List<GameObject> panels = new List<GameObject>();
    private const string retryError = "retry";

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitializePanels();
        InitializeSliders();
        NameCheck();
        LoadTextData();
    }

    #region GUIInterractions

    void InitializePanels()
    {
        foreach (Transform go in gameObject.transform)
        {
            if (go.gameObject.name.Contains("Panel"))
            {
                panels.Add(go.gameObject);
                go.gameObject.SetActive(false);
            }
        }
        mainPanel.SetActive(true);
    }

    public void SwitchPanel(GameObject panel)
    {
        foreach (GameObject gameObject in panels)
        {
            gameObject.SetActive(false);
        }

        panel.SetActive(true);
    }

    public void OpenPopupPanel(GameObject panel) {
        panel.SetActive(true);
    }

    public void ClosePopupPanel(GameObject panel) {
        panel.SetActive(false);
    }

    public void OpenScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    #endregion

    #region SoundControl
    void InitializeSliders() {
        SetSoundVolume(SoundManager.instance.soundVolume);
        SetMusicVolume(SoundManager.instance.musicVolume);
        musicSlider.value = SoundManager.instance.musicVolume;
        soundSlider.value = SoundManager.instance.soundVolume;
    }

    public void SetSoundVolume(float volume) {
        SoundManager.instance.soundVolume = volume;
        soundSliderText.text = ((int)(volume * 100)).ToString();
    }

    public void SetMusicVolume(float volume) {
        SoundManager.instance.musicVolume = volume;
        musicSliderText.text = ((int)(volume * 100)).ToString();
    }
    #endregion

    #region SaveMenuData
    void NameCheck() {
        //Check for the username to initialize it into the menu scene. Also sets the name to the GameManager.
        if (PlayerPrefs.HasKey(nameSaveTag))
        {
            GameManager.instance.name = PlayerPrefs.GetString(nameSaveTag);
            nameText.text = GameManager.instance.name;
            print(GameManager.instance.name);
        }
        else
        {
            enterNamePanel.SetActive(true);
        }
    }

    public void SetName(GameObject inputField) {
        InputField temp = inputField.gameObject.GetComponent<InputField>();
        if (temp.text.Length >= 3)
        {
            PlayerPrefs.SetString(nameSaveTag,temp.text);
            GameManager.instance.name = temp.text;
            nameText.text = temp.text;
            temp.text = "";
            ClosePopupPanel(enterNamePanel);
        }
    }

    public void SaveTextData() {
        foreach (SaveData loadData in this.loadData)
        {
            PlayerPrefs.SetString(loadData.saveTag, loadData.text.text);
        }
    }

    public void LoadTextData() {
        //Loads every set text that is added in the inspector.
        foreach (SaveData loadData in this.loadData)
        {
            string temp = PlayerPrefs.GetString(loadData.saveTag, "");
            loadData.text.text = temp;
            if (temp != "")
            {
                loadData.text.text = temp;
            } 
        }
    }

    #endregion

    #region Server
    public void TryConnectToServer() {
        //Tries connecting to the given address. Also handles any errors that may occur, and sends the feedback through into the GUI.
        int port;
        ServerConnectionFeedback(directConnectFeedback, "Connecting..", Color.black);
        if (int.TryParse(directConnectPort.text, out port))
        {
            NetworkConnectionError error = Network.Connect(directConnectIP.text, port, serverPassword.text);
            ServerConnectionFeedback(directConnectFeedback, GetNetworkError(error));
        }
        else
        {
            ServerConnectionFeedback(directConnectFeedback, "Please enter valid input", Color.red);
        }
    }

    public void StartServer() {
        //Attepts to start a server with the given settings. Also handles any error feedback back into the GUI.
        ServerConnectionFeedback(createServerFeedback, "Connecting..", Color.black);
        NetworkConnectionError networkError = Network.InitializeServer(int.Parse(serverMaxPlayers.text), int.Parse(serverPort.text), serverUseNAT.isOn);
        Network.incomingPassword = serverPassword.text;
        MasterServer.RegisterHost(GameManager.instance.uniqueGameType, serverName.text, serverDescription.text);

        string error = GetNetworkError(networkError);
        if (error.Equals(retryError))
        {
            Network.Disconnect();
            StartServer();
        }
        else
        {
            ServerConnectionFeedback(createServerFeedback, error, Color.red);
        }
    }


    void OnFailedToConnect(NetworkConnectionError error) {
        ServerConnectionFeedback(directConnectFeedback, GetNetworkError(error), Color.red);
    }

    void ServerConnectionFeedback(Text infobox, string message) {
        infobox.text = message;
        infobox.color = Color.black;
    }

    void ServerConnectionFeedback(Text infobox, string message, Color color) {
        infobox.text = message;
        infobox.color = color;
    }

    string GetNetworkError(NetworkConnectionError error) {
        Debug.Log(error);
        preventInputPanel.SetActive(error.Equals(NetworkConnectionError.NoError));
        switch (error)
        {
            case NetworkConnectionError.NoError:
                return "Connecting..";
            case NetworkConnectionError.AlreadyConnectedToServer:
            case NetworkConnectionError.AlreadyConnectedToAnotherServer:
                return retryError;
            case NetworkConnectionError.ConnectionBanned:
                return "Banned from server";
            case NetworkConnectionError.InvalidPassword:
                return "Incorrect password";
            case NetworkConnectionError.TooManyConnectedPlayers:
                return "Server is full";
            case NetworkConnectionError.EmptyConnectTarget:
                return "Please enter target server";
            case NetworkConnectionError.NATPunchthroughFailed:
            case NetworkConnectionError.NATTargetConnectionLost:
            case NetworkConnectionError.NATTargetNotConnected:
            case NetworkConnectionError.InternalDirectConnectFailed:
                return "Nat error";
            default:
                return "Failed to connect";
        }
    }

    #endregion
}
[System.Serializable]
public struct SaveData{
    public string saveTag;
    public InputField text;
}



