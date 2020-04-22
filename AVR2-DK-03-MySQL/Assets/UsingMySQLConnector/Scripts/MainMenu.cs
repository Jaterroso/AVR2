using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UsingMySQLConnector
{

    public class MainMenu : MonoBehaviour
    {
        public Text playerDisplay;
        public Button gameButton;
        public Button loginButton;
        public Button registerButton;

        private void Start()
        {
            if (User.LoggedIn)
            {
                playerDisplay.text = "Player: " + User.username;
            }
            registerButton.interactable = !User.LoggedIn;
            loginButton.interactable = !User.LoggedIn;
            gameButton.interactable = User.LoggedIn;
        }

        public void GoToRegister()
        {
            SceneManager.LoadScene(1);
        }
        public void GoToLogin()
        {
            SceneManager.LoadScene(2);
        }
        public void GoToGame()
        {
            SceneManager.LoadScene(3);
        }
    }
}
