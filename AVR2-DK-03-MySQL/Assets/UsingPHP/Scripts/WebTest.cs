using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UsingPHP
{
    public class WebTest : MonoBehaviour
    {
        // Start is called before the first frame update
        IEnumerator Start()
        {
            string url = "http://localhost/mysql/webtest.php";
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
            {
                unityWebRequest.SendWebRequest();
                yield return unityWebRequest;
                string[] webResults = unityWebRequest.downloadHandler.text.Split('\t');
                Debug.Log(webResults[0]);
                int webNumber = int.Parse(webResults[1]);
                webNumber *= 2;
                Debug.Log(webNumber);
            }
        }
    }
}
