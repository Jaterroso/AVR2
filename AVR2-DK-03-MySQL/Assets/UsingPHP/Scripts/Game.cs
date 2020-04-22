using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace UsingPHP
{
    public class Game : MonoBehaviour
    {
        public Text playerDisplay;
        public Text scoreDisplay;

        // Start is called before the first frame update
        private void Awake()
        {
            if (DBManager.username == null)
                SceneManager.LoadScene(0);

            playerDisplay.text = "Player: " + DBManager.username;
            scoreDisplay.text = "Score: " + DBManager.score;
        }

        public void CallSavePlayerData()
        {
            StartCoroutine(SavePlayerData("http://localhost/mysql/savedata.php"));
        }

        IEnumerator SavePlayerData(string _url)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("username", DBManager.username);
            wwwForm.AddField("score", DBManager.score);

            using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(_url, wwwForm))
            {
                yield return unityWebRequest.SendWebRequest();

                if (unityWebRequest.downloadHandler.text == "0")
                {
                    Debug.Log("Game saved.");
                }
                else
                {
                    Debug.Log("Save failed. Error #" + unityWebRequest.downloadHandler.text);
                }
                DBManager.Logout();
                SceneManager.LoadScene(0);
            }
        }

        public void IncreaseScore()
        {
            DBManager.score++;
            scoreDisplay.text = "Score: " + DBManager.score;
        }
    }
}
