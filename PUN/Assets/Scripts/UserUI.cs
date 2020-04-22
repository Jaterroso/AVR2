using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.eonreality.pun
{
    public class UserUI : MonoBehaviour
    {
        #region Private Fields
        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text userNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider userHealthSlider;
        private UserManager target;

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;
        #endregion

        #region Public fields
        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();
            this.transform.SetParent(GameObject.Find("ConsoleLog").GetComponent<Transform>(), false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Reflect the Player Health
            if (userHealthSlider != null)
            {
               userHealthSlider.value = target.userHealth;
            }

            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1.0f : 0.0f;
            }

            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        #endregion


        #region Public Methods
        public void SetTarget(UserManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for UserUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
            targetTransform = target.GetComponent<Transform>();
            targetRenderer = target.GetComponent<Renderer>();
            CharacterController characterController = target.GetComponent<CharacterController>();
            // Get data from the Player that won't change during the lifetime of this Component
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }
            if (userNameText != null)
            {
                userNameText.text = target.photonView.Owner.NickName;
            }
        }

        #endregion

    }
}
