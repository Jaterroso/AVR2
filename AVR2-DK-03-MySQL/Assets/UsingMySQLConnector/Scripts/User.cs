using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsingMySQLConnector
{
    public static class User
    {
        public static string username;
        public static int score;

        public static bool LoggedIn { get { return username != null; } }

        public static void Logout()
        {
            username = null;
        }
    }
}
