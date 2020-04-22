using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UsingMySQLConnector
{
    public class Registration : MonoBehaviour
    {
        public InputField nameField;
        public InputField passwordField;

        public Button submitButton;

        public void CallRegister()
        {
            MySQLManager.instance.Register(nameField.text, passwordField.text);
        }

        public void VerifyInputs()
        {
            submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
        }
    }
}
