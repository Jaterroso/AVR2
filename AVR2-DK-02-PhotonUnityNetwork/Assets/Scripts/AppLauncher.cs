using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.eonreality.pun
{
    public class AppLauncher : MonoBehaviourPunCallbacks
    {
        [Header("Application")]
        [SerializeField]
        [Tooltip("App version number")]
        private string appVersionNumber = "1.0";
        [SerializeField]
        [Tooltip("App version number")]
        private bool isConnecting;

        [Header("UI elements")]
        [SerializeField]
        [Tooltip("Button to connect to the master server")]
        private Button connectToMasterButton;
        [SerializeField]
        [Tooltip("Form to sign in")]
        private GameObject signInForm;
        [SerializeField]
        [Tooltip("Connection panel")]
        private GameObject connectingPanel;
        [SerializeField]
        [Tooltip("User name")]
        private Text userName;
        [SerializeField]
        [Tooltip("Log console for realtime user feedback")]
        private Text logFeedbackText;
        //public Button joinRandomRoomButton;

        [Header("Room properties")]
        [SerializeField]
        [Tooltip("Maximum number of users per room")]
        private byte maxUsersPerRoom = 3;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            signInForm.SetActive(true);
            connectingPanel.SetActive(false);
        }

        private void Update()
        {

        }

        #region Public methods
        public void ConnectToMasterServer()
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.NickName = userName.text;
            PhotonNetwork.GameVersion = appVersionNumber;

            signInForm.SetActive(false);
            connectingPanel.SetActive(true);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            } else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
            }
            LogFeedback("Connecting...");
        }
        #endregion

        #region Private methods
        private void LogFeedback(string _message)
        {
            if (logFeedbackText == null)
                return;

            logFeedbackText.text += _message + System.Environment.NewLine;
        }
        private void CreateRoom(string _roomName)
        {
            int randomRoomID = Random.Range(0, 10000);
            RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxUsersPerRoom };
            PhotonNetwork.CreateRoom(_roomName + "_" + randomRoomID, roomOptions);
            LogFeedback("<Color=Green>CreateRoom</Color>: Room " + _roomName + "_" + randomRoomID + " is being created.");
        }
        #endregion

        #region PUN Callbacks
        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                LogFeedback("OnConnectedToMaster: Connected to server in " + PhotonNetwork.CloudRegion.ToUpper() + " region.");
                LogFeedback("OnConnectedToMaster: Ready to join a room.");
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Unable to join a random room.");
            CreateRoom("Room");
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            LogFeedback("<Color=Red>OnCreateRoomFailed</Color>: Unable to create room.");
            CreateRoom("Room");
        }
        public override void OnJoinedRoom()
        {
            LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " user(s).");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                LogFeedback("<Color=Green>OnJoinedRoom</Color>: Loading room for " + PhotonNetwork.CurrentRoom.PlayerCount + " user(s).");
                PhotonNetwork.LoadLevel("PunRoomFor1");
            }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            LogFeedback("<Color=Green>OnPlayerEnteredRoom</Color>: A new user " + newPlayer.NickName + " has entered the room.");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            LogFeedback("<Color=Red>OnDisconnected</Color>: " + cause.ToString());
        }
        #endregion
    }
}

