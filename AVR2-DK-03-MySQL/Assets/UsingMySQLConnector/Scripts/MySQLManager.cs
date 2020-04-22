using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;

namespace UsingMySQLConnector
{
    public class MySQLManager : MonoBehaviour
    {

        public static MySQLManager instance;
        public string host, database, username, password, table;
        private MySqlConnection mysql;

        // Start is called before the first frame update
        void Awake()
        {
            if (instance != null) Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                ConnectToMySQLDB();
            }
        }

        void ConnectToMySQLDB()
        {
            string connectionCommand = "SERVER=" + host + ";database=" + database + ";User ID=" + username + ";Password=" + password + ";Pooling=true;Charset=utf8";
            try
            {
                mysql = new MySqlConnection(connectionCommand);
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    SceneManager.LoadScene(0);
            }
            catch (Exception _e)
            {
                Debug.Log(_e.Message.ToString());
            }
        }

        public void Register(string _username, string _password)
        {
            mysql.Open();
            string registerCommand = "INSERT INTO " + table + " (username, password) VALUES ('" + _username + "','" + _password + "')";
            MySqlCommand mySQLCommand = new MySqlCommand(registerCommand, mysql);
            try
            {
                mySQLCommand.ExecuteReader();
                Debug.Log("User created successfully.");
                SceneManager.LoadScene(0);
                mysql.Close();
            } catch (Exception _e)
            {
                Debug.Log(_e.Message.ToString());
            }
        }

        public void Login(string _username, string _password)
        {
            mysql.Open();
            string loginCommand = "SELECT username, password, score FROM " + table + " WHERE username='" + _username + "'";
            MySqlCommand mySQLCommand = new MySqlCommand(loginCommand, mysql);
            try
            {
                MySqlDataReader mySqlDataReader = mySQLCommand.ExecuteReader();
                string readPassword = null;
                int readScore = 0;
                while (mySqlDataReader.Read())
                {
                    readPassword = mySqlDataReader["password"].ToString();
                    readScore = int.Parse(mySqlDataReader["score"].ToString());
                }
                if (readPassword == _password)
                {
                    User.username = _username;
                    User.score = readScore;
                    Debug.Log("User " + _username + " successfully logged in.");
                    SceneManager.LoadScene(0);
                }
                else
                    Debug.Log("Invalid password for user " + _username);

                mySqlDataReader.Close();
                mysql.Close();
            }
            catch (Exception _e)
            {
                Debug.Log(_e.Message.ToString());
            }
        }

        public void SaveUserData(string _username, int _score)
        {
            mysql.Open();
            // CHECK USER
            string selectCommand = "SELECT username FROM " + table + " WHERE username='" + _username + "'";
            MySqlCommand mySQLSelectCommand = new MySqlCommand(selectCommand, mysql);
            try
            {
                MySqlDataReader mySqlDataReader = mySQLSelectCommand.ExecuteReader();

                if (!mySqlDataReader.HasRows)
                {
                    mySqlDataReader.Close();
                    mysql.Close();
                    return;
                }
                mySqlDataReader.Close();
            }
            catch (Exception _e)
            {
                Debug.Log(_e.Message.ToString());
            }
            // UPDATE
            string updateCommand = "UPDATE " + table + " SET score = " + _score + " WHERE username = '" + _username + "'";
            MySqlCommand mySQLUpdateCommand = new MySqlCommand(updateCommand, mysql);
            try
            {
                MySqlDataReader mySqlDataReader = mySQLUpdateCommand.ExecuteReader();
                mySqlDataReader.Close();
                User.Logout();
                SceneManager.LoadScene(0);
            }
            catch (Exception _e)
            {
                Debug.Log(_e.Message.ToString());
            }
            mysql.Close();
        }
    }
}
