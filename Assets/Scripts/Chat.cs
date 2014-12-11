using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

[RequireComponent(typeof(NetworkView))]
public class Chat : MonoBehaviour {
    public InputField chatField;
    public Text text;

    private const string newLine = "\r\n";
    private string chat = "";

    public void Send() {
        networkView.RPC("Message", RPCMode.All, GameManager.instance.name + ": " + chatField.text);
        chatField.text = "";
    }

    [RPC]
    public void Message(string message) {
        chat += newLine + message;
        text.text = chat;
    }
}
