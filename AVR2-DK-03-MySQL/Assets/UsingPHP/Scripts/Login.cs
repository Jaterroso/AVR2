using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UsingPHP
{
    public class Login : MonoBehaviour
    {
        public InputField nameField;
        public InputField passwordField;
        public InputField debugWindow;

        public Button submitButton;

        public void CallLogin()
        {
            StartCoroutine(LoginPlayer("http://localhost/mysql/login.php"));
        }

        IEnumerator LoginPlayer(string _url)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("username", nameField.text);
            wwwForm.AddField("password", passwordField.text);

            using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(_url, wwwForm))
            {
                yield return unityWebRequest.SendWebRequest();

                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                {
                    Debug.Log("Error: " + unityWebRequest.error);
                }
                else
                {
                    if (unityWebRequest.downloadHandler.text[0] == '0')
                    {
                        DBManager.username = nameField.text;
                        DBManager.score = int.Parse(unityWebRequest.downloadHandler.text.Split('\t')[1]);
                        SceneManager.LoadScene(0);
                    }
                    else
                    {
                        debugWindow.text = "User login failed. Error #" + unityWebRequest.downloadHandler.text;
                        Debug.Log("User login failed. Error #" + unityWebRequest.downloadHandler.text);
                        
                    }
                }
            }
        }

        public void VerifyInputs()
        {
            submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
        }
    }
}
