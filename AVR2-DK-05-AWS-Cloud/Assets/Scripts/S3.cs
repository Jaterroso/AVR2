using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amazon;
using Amazon.Runtime;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.IO;

public class S3 : MonoBehaviour
{

    private RegionEndpoint s3RegionEndpoint = RegionEndpoint.USEast2;
    private RegionEndpoint cognitoRegionEndpoint = RegionEndpoint.USEast2;
    private CognitoAWSCredentials credentials;
    private AmazonS3Client s3Client;

    [Header("Cognito")]
    public string identityPoolId = "";

    [Header("S3")]
    public string s3BucketName = "";
    public string sampleFileName = "";
    public string textureFileName = "";

    [Header("UI elements")]
    public Text consoleText = null;

    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
    }

    #region PUBLMIC METHODS
    public void GetObject()
    {
        credentials = GetComponent<Cognito>().credentials;
        s3Client = new AmazonS3Client(credentials, s3RegionEndpoint);

        consoleText.text = string.Format("Fetching {0} from bucket {1}...", sampleFileName, s3BucketName);

        s3Client.GetObjectAsync(
            s3BucketName,
            sampleFileName,
            (responseObj) =>
            {
                string data = null;
                var response = responseObj.Response;
                if (response != null)
                {
                    using (StreamReader reader = new StreamReader(response.ResponseStream))
                    {
                        data = reader.ReadToEnd();
                    }
                    consoleText.text += System.Environment.NewLine;
                    consoleText.text += data;
                }
                else
                {
                    consoleText.text += System.Environment.NewLine;
                    consoleText.text += responseObj.Exception.Message;
                }
            }
        );
    }
    public void GetObjectList()
    {
        credentials = GetComponent<Cognito>().credentials;
        s3Client = new AmazonS3Client(credentials, s3RegionEndpoint);

        consoleText.text = "Fetching all objects from bucket " + s3BucketName + "...";

        var request = new ListObjectsRequest()
        {
            BucketName = s3BucketName
        };
        s3Client.ListObjectsAsync(
            request,
            (responseObj) =>
            {
                consoleText.text = System.Environment.NewLine;
                if (responseObj.Exception == null)
                {
                    consoleText.text += "Retrieving objects list from the bucket " + request.BucketName + System.Environment.NewLine + System.Environment.NewLine;
                    responseObj.Response.S3Objects.ForEach((o) =>
                    {
                        consoleText.text += o.Key + System.Environment.NewLine;
                    });
                } else
                {
                    consoleText.text += "Got an exception: " + responseObj.Exception.Message + System.Environment.NewLine;
                }
            }
        );


    }
    public void ApplyTexture()
    {
        Texture2D texture2D = new Texture2D(256, 256);

        credentials = GetComponent<Cognito>().credentials;
        s3Client = new AmazonS3Client(credentials, s3RegionEndpoint);

        consoleText.text = string.Format("Fetching {0} from bucket {1}..." + System.Environment.NewLine + System.Environment.NewLine, textureFileName, s3BucketName);

        s3Client.GetObjectAsync(
            s3BucketName,
            textureFileName,
            (responseObj) =>
            {
                byte[] data = null;
                var response = responseObj.Response;
                Stream input = response.ResponseStream;

                if (input != null)
                {
                    byte[] buffer = new byte[256 * 256];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        data = ms.ToArray();
                    }
                    texture2D = CreateTextureFromBytes(data);
                }
                GetComponent<FacebookLogin>().S3_Cube.GetComponent<Renderer>().material.mainTexture = texture2D;
                consoleText.text += "Texture " + textureFileName + " from " + s3BucketName + " has been applied successfully." + System.Environment.NewLine;
            }
        );
    }
    #endregion

    #region PRIVATE METHODS
    private Texture2D CreateTextureFromBytes(byte[] data)
    {
        Texture2D tex = new Texture2D(256, 256);
        tex.LoadImage(data);
        return tex;
    }
    #endregion
}
