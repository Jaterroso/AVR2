using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace com.eonreality.pun
{
    public class AppManager : MonoBehaviourPunCallbacks
    {
        public static AppManager Instance;
        private GameObject instance;

        [Tooltip("The prefab to use for representing the user")]
        [SerializeField]
        private GameObject userPrefab;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("AppLauncher");
                return;
            }
            if (userPrefab == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab reference. Please set it up in GameObject 'AppManager'", this);
            }
            else
            {
                if (UserManager.localUserInstance == null)
                {
                    Debug.LogFormat("We are instantiating LocalUser from {0}", SceneManagerHelper.ActiveSceneName);

                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.userPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region PUN Callbacks
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("AppLauncher");
        }
        #endregion

        #region Private Methods
        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("PUNRoomFor" + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion

        #region Public methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion
    }
}