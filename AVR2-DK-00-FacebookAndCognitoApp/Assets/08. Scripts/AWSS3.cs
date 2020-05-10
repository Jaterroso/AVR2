using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using Amazon.S3.Util;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using System.Drawing;

public class AWSS3 : MonoBehaviour
{
    #region MEMBERS
    [Header("Cognito fields")]
    public string IdentityPoolId = "";
    public RegionEndpoint CognitoIdentityRegion = RegionEndpoint.USEast2;
    private CognitoAWSCredentials credentials;
    private AmazonS3Client client;

    [Header("S3 fields")]
    public RegionEndpoint S3Region = RegionEndpoint.USEast2;
    public string S3BucketName = null;
    public string SampleFileName = null;
    public string TextureFileName = null;
    private AmazonS3Client s3Client;

    [Header("UI elements")]
    public Button GetObjectButton = null;
    public Text ResultText = null;
    public Canvas loginCanvas, loggedInCanvas, scoreCanvas, s3Canvas;
    #endregion

    #region PRIVATE MEMBERS
    private AWSCredentials _credentials;
    private IAmazonS3 _s3Client;
    #endregion

    private void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
#if UNITY_EDITOR
        AWSConfigs.LoggingConfig.LogTo = LoggingOptions.UnityLogger;
#endif
        /*credentials = new CognitoAWSCredentials(IdentityPoolId, CognitoIdentityRegion);*/
    }


    #region PRIVATE METHODS
    public Texture2D bytesToTexture2D(byte[] imageBytes)
    {
        Texture2D tex = new Texture2D(256, 256);
        tex.LoadImage(imageBytes);
        return tex;
    }

    #endregion

    #region PUBLIC METHODS
    public void Back()
    {
        s3Canvas.gameObject.SetActive(false);
        loggedInCanvas.gameObject.SetActive(true);
        scoreCanvas.gameObject.SetActive(true);
        GetComponent<FacebookConnect>().sphere.SetActive(false);
    }
    public void GetObject()
    {
        credentials = FindObjectOfType<AWSCognito>().credentials;
        client = new AmazonS3Client(credentials, S3Region);

        ResultText.text = string.Format("Fetching {0} from bucket {1}...", SampleFileName, S3BucketName);
        client.GetObjectAsync(S3BucketName, SampleFileName, (responseObj) =>
        {
            string data = null;
            var response = responseObj.Response;
            if (response != null)
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    data = reader.ReadToEnd();
                }
                ResultText.text += "\n\n";
                ResultText.text += data;
            }
            else
            {
                ResultText.text += "\n";
                ResultText.text += responseObj.Exception.Message;
            }
        });
    }

    public void GetObjectAsTexture()
    {
        // Create objects
        Texture2D texture = new Texture2D(256, 256);
        credentials = FindObjectOfType<AWSCognito>().credentials;
        client = new AmazonS3Client(credentials, S3Region);
        // Logging
        ResultText.text = string.Format("Fetching {0} from bucket {1}...", TextureFileName, S3BucketName);

        // Managing response
        client.GetObjectAsync(S3BucketName, TextureFileName, (responseObj) =>
        {
            byte[] data = null;
            var response = responseObj.Response;
            Stream input = response.ResponseStream;

            if (response.ResponseStream != null)
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

                //Display Image
                texture = bytesToTexture2D(data);
            }
            // Apply texture to material
            FindObjectOfType<FacebookConnect>().sphere.GetComponent<Renderer>().material.mainTexture = texture;

            ResultText.text += "\n\n";
            ResultText.text += "Texture applied.";

        });
    }

    public void ListObjects()
    {
        credentials = FindObjectOfType<AWSCognito>().credentials;
        client = new AmazonS3Client(credentials, S3Region);
        ResultText.text = "Fetching all objects from " + S3BucketName + "...";
        var request = new ListObjectsRequest()
        {
            BucketName = S3BucketName
        };

        client.ListObjectsAsync(request, (responseObject) =>
        {
            ResultText.text += "\n\n";
            if (responseObject.Exception == null)
            {
                ResultText.text += "List retrieved from " + request.BucketName + "\n\n";
                responseObject.Response.S3Objects.ForEach((o) =>
                {
                    ResultText.text += string.Format("{0}\n", o.Key);
                });
            }
            else
            {
                ResultText.text += "Got Exception: " + responseObject.Exception.Message + "\n\n";
            }
        });
    }
    #endregion
}
