using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public GameObject mainPanel;
    public GameObject createServerEditSettingsPanel;
    public AudioClip buttonClick;

    public Slider soundSlider;
    public Slider musicSlider;
    public Text soundSliderText;
    public Text musicSliderText;

    public InputField directConnectIP;
    public InputField directConnectPort;
    public InputField directConnectPassword;



    public InputField serverName;
    public InputField serverDescription;
    public InputField serverMaxPlayers;
    public InputField serverPort;
    public InputField serverPassword;
    public Toggle serverUseNAT;

    public Text serverFeedback;

    private List<GameObject> panels = new List<GameObject>();

    public SaveData[] loadData;

    

    void Start()
    {
        InitializePanels();
        InitializeSliders();
        StartCoroutine(DelayTextUpdate());
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

    void ServerConnectionFeedback(string message) {
        serverFeedback.text = message;
        serverFeedback.color = Color.black;
    }

    void ServerConnectionFeedback(string message, Color color) {
        serverFeedback.text = message;
        serverFeedback.color = color;
    }

    public void OpenServerEditSettingsPanel(GameObject editSetting) {
        createServerEditSettingsPanel.SetActive(true);
    }

    public void CloseServerEditSettingsPanel() {
        foreach (Transform go in transform)
        {
            if(go.gameObject.activeInHierarchy)
            go.gameObject.SetActive(false);
        }
        createServerEditSettingsPanel.SetActive(false);
    }

    IEnumerator DelayTextUpdate() {
        yield return new WaitForEndOfFrame();
        LoadTextData();
    }

    public void StartServer() {
       NetworkConnectionError error = Network.InitializeServer(int.Parse(serverMaxPlayers.text), int.Parse(serverPort.text), serverUseNAT.isOn);
       Network.incomingPassword = serverPassword.text;
       MasterServer.RegisterHost(GameManager.instance.uniqueGameType, serverName.text,serverDescription.text);

       switch (error)
       {
           case NetworkConnectionError.AlreadyConnectedToServer:
           case NetworkConnectionError.AlreadyConnectedToAnotherServer:
               Network.Disconnect();
               StartServer();
               break;
           case NetworkConnectionError.ConnectionBanned:
               Debug.LogError("Banned from server");
               ServerConnectionFeedback("Banned from server", Color.red);
               break;
           case NetworkConnectionError.InvalidPassword:
               Debug.LogError("Incorrect password");
               ServerConnectionFeedback("Incorrect password", Color.red);
               break;
           case NetworkConnectionError.TooManyConnectedPlayers:
               Debug.LogError("Server is full");
               ServerConnectionFeedback("Server is full");
               break;
           case NetworkConnectionError.EmptyConnectTarget:
               Debug.LogError("No target server entered");
               ServerConnectionFeedback("Please enter target server");
               break;
           case NetworkConnectionError.NATPunchthroughFailed:
           case NetworkConnectionError.NATTargetConnectionLost:
           case NetworkConnectionError.NATTargetNotConnected:
           case NetworkConnectionError.InternalDirectConnectFailed:
               Debug.LogError("NAT error");
               ServerConnectionFeedback("Nat error");
               break;
           case NetworkConnectionError.RSAPublicKeyMismatch:
           case NetworkConnectionError.ConnectionFailed:
           case NetworkConnectionError.CreateSocketOrThreadFailure:
           case NetworkConnectionError.IncorrectParameters:
               Debug.LogError("Failed to connect");
               ServerConnectionFeedback("Failed to connect");
               break;
           default: break;
       }

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

    public void SaveTextData() {
        foreach (SaveData loadData in this.loadData)
        {
            PlayerPrefs.SetString(loadData.saveTag, loadData.text.text);
            print(PlayerPrefs.GetString(loadData.saveTag));
        }
    }

    public void LoadTextData() {
        foreach (SaveData loadData in this.loadData)
        {
            string temp = PlayerPrefs.GetString(loadData.saveTag, "");
            loadData.text.text = temp;
            if (temp != "")
            {
                loadData.placeHolder.enabled = false;
                
            }
           
        }
    }

    #endregion

    #region Server
    public void TryConnectToServer() {
        int port;
        if (int.TryParse(directConnectPort.text, out port))
        {
            NetworkConnectionError error = Network.Connect(directConnectIP.text, port, serverPassword.text);

            switch (error)
            {
                case NetworkConnectionError.AlreadyConnectedToServer:
                case NetworkConnectionError.AlreadyConnectedToAnotherServer:
                    Network.Disconnect();
                    TryConnectToServer();
                    break;
                case NetworkConnectionError.ConnectionBanned:
                    Debug.LogError("Banned from server");
                    ServerConnectionFeedback("Banned from server", Color.red);
                    break;
                case NetworkConnectionError.InvalidPassword:
                    Debug.LogError("Incorrect password");
                    ServerConnectionFeedback("Incorrect password", Color.red);
                    break;
                case NetworkConnectionError.TooManyConnectedPlayers:
                    Debug.LogError("Server is full");
                    ServerConnectionFeedback("Server is full");
                    break;
                case NetworkConnectionError.EmptyConnectTarget:
                    Debug.LogError("No target server entered");
                    ServerConnectionFeedback("Please enter target server");
                    break;
                case NetworkConnectionError.NATPunchthroughFailed:
                case NetworkConnectionError.NATTargetConnectionLost:
                case NetworkConnectionError.NATTargetNotConnected:
                case NetworkConnectionError.InternalDirectConnectFailed:
                    Debug.LogError("NAT error");
                    ServerConnectionFeedback("Nat error");
                    break;
                case NetworkConnectionError.RSAPublicKeyMismatch:
                case NetworkConnectionError.ConnectionFailed:
                case NetworkConnectionError.CreateSocketOrThreadFailure:
                case NetworkConnectionError.IncorrectParameters:
                    Debug.LogError("Failed to connect");
                    ServerConnectionFeedback("Failed to connect");
                    break;
                default: break;
            }
        }
    }


    #endregion
}
[System.Serializable]
public struct SaveData{
    public string saveTag;
    public Text text;
    public Text placeHolder;


}


