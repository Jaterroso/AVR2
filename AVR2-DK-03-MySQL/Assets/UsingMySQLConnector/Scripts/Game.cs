using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace UsingMySQLConnector {
    public class Game : MonoBehaviour
    {
        public Text playerDisplay;
        public Text scoreDisplay;

        // Start is called before the first frame update
        private void Awake()
        {
            if (MySQLManager.instance.username == null)
                SceneManager.LoadScene(1);

            playerDisplay.text = "Player: " + User.username;
            scoreDisplay.text = "Score: " + User.score;
        }

        // Update is called once per frame
        public void CallSavePlayerData()
        {
            MySQLManager.instance.SaveUserData(User.username, User.score);
        }

        public void IncreaseScore()
        {
            User.score++;
            scoreDisplay.text = "Score: " + User.score;
        }
    }
}
