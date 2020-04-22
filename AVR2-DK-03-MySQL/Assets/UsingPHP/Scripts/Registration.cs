using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UsingPHP
{
    public class Registration : MonoBehaviour
    {
        public InputField nameField;
        public InputField passwordField;

        public Button submitButton;

        public void CallRegister()
        {
            StartCoroutine(Register("http://localhost/mysql/register.php"));
        }

        IEnumerator Register(string _url)
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
                    Debug.Log("Received: " + unityWebRequest.downloadHandler.text);
                    if (unityWebRequest.downloadHandler.text == "0")
                    {
                        Debug.Log("User created successfully.");
                        SceneManager.LoadScene(0);
                    }
                    else
                    {
                        Debug.Log("User creation failed. Error: #" + unityWebRequest.downloadHandler.text);
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
