using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
    public Text ping;
    public Text clipAmount;
    public Text ammoAmount;

	void Start() {
        NetworkManager.instance.OnUpdate += OnUpdate;
        NetworkManager.instance.GetMyInfo().playerObject.playerHUD = this;
	}

    void OnDestroy() {
        NetworkManager.instance.OnUpdate -= OnUpdate;
    }

    void OnUpdate(NetworkPlayer player, PlayerInfo playerInfo, string setting) {
        if (player.Equals(Network.player)) {
            if (setting.Equals(PlayerInfo.PING)) {
                ping.text = "Ping: " + playerInfo.ping + "ms";
            }
        }
    }
}
