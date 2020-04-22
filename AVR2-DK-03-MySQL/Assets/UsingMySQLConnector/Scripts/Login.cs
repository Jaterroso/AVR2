using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UsingMySQLConnector
{
    public class Login : MonoBehaviour
    {
        public InputField nameField;
        public InputField passwordField;

        public Button submitButton;

        public void CallLogin()
        {
            MySQLManager.instance.Login(nameField.text, passwordField.text);
        }

        public void VerifyInputs()
        {
            submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
        }
    }
}
