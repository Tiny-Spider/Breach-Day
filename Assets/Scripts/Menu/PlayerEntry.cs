using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEntry : MonoBehaviour {

   public PlayerInfo playerInfo;
   public Text name;


   void Start() {
       playerInfo = NetworkManager.instance.connectedPlayers[Network.player];
       name.text = playerInfo.name;
   }
}
