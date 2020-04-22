using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace com.eonreality.pun
{
    public class UserManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields

        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        private bool IsFiring; //True, when the user is firing
        #endregion

        #region Public fields
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject localUserInstance;

        [Tooltip("The current health of our user")]
        public float userHealth = 1f;

        private PhotonView photonView;

        [Tooltip("The User's UI GameObject Prefab")]
        [SerializeField]
        public GameObject userUIPrefab;
        #endregion

        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(IsFiring); // We own this player: send the others our data
                stream.SendNext(userHealth);
            }
            else
            {
                this.IsFiring = (bool)stream.ReceiveNext(); // Network player, receive data
                this.userHealth = (float)stream.ReceiveNext();
            }
        }
        #endregion

        #region MonoBehaviour CallBacks
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            photonView = GetComponent<PhotonView>();

            if (photonView.IsMine)
            {
                UserManager.localUserInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);

            if (beams == null)
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            else
                beams.SetActive(false);
        }

        private void Start()
        {
            CameraManager _cameraMgr = this.gameObject.GetComponent<CameraManager>();

            if (_cameraMgr != null)
            {
                if (photonView.IsMine)
                    _cameraMgr.OnStartFollowing();
            }
            else
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);


            if (userUIPrefab != null)
            {
                GameObject _uiGo = Instantiate(userUIPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> userUIPrefab reference on player Prefab.", this);
            }

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif

#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
            GameObject _uiGo = Instantiate(this.userUIPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
                if (userHealth <= 0.0f)
                {
                    AppManager.Instance.LeaveRoom();
                }
            }
            
            // trigger Beams active state
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
        }
        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect Health of the Player if the collider is a beam
        /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            userHealth -= 0.1f;
        }
        /// <summary>
        /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
        /// We're going to affect health while the beams are touching the player
        /// </summary>
        /// <param name="other">Other.</param>
        void OnTriggerStay(Collider other)
        {
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
            userHealth -= 0.1f * Time.deltaTime;
        }
        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }

        #endregion

        #region Private methods
        #if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
        #endif
        #endregion
    }
}


