using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UsingPHP
{
    public class MainMenu : MonoBehaviour
    {
        public Text playerDisplay;
        public Button gameButton;
        public Button loginButton;
        public Button registerButton;

        private void Start()
        {
            if (DBManager.LoggedIn)
            {
                playerDisplay.text = "Player: " + DBManager.username;
            }
            registerButton.interactable = !DBManager.LoggedIn;
            loginButton.interactable = !DBManager.LoggedIn;
            gameButton.interactable = DBManager.LoggedIn;
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
