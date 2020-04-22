using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace com.eonreality.pun
{
    public class UIManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
            {
                // Room name
                if (PhotonNetwork.CurrentRoom.Name != null)
                    this.transform.GetChild(0).GetComponent<Text>().text = "You are in room " + PhotonNetwork.CurrentRoom.Name;

                // Master
                if (PhotonNetwork.IsMasterClient)
                    this.transform.GetChild(1).GetComponent<Text>().text = "You are the master.";
                else
                    this.transform.GetChild(1).GetComponent<Text>().text = "You are not the master.";

                // Number of players
                this.transform.GetChild(2).GetComponent<Text>().text = PhotonNetwork.PlayerList.Length.ToString() + " player(s) in game.";

                // Player number
                this.transform.GetChild(3).GetComponent<Text>().text = "Player #" + PhotonNetwork.LocalPlayer.ActorNumber.ToString();

                // Player name
                this.transform.GetChild(4).GetComponent<Text>().text = "Player name: " + PhotonNetwork.LocalPlayer.NickName.ToString();

            }
        }
    }
}
