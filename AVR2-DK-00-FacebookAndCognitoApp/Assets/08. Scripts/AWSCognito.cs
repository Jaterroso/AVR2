using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using UnityEngine.UI;
using System;

public class AWSCognito : MonoBehaviour
{
    Dataset scoreDS;
    string theName;
    int theScore;

    bool sync = false;

    [SerializeField]
    private InputField scoreIF, nameIF;
    CognitoSyncManager syncManager;

    [HideInInspector]
    public CognitoAWSCredentials credentials;

    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

#if UNITY_EDITOR
    AWSConfigs.LoggingConfig.LogTo = LoggingOptions.UnityLogger;
#endif

        credentials = new CognitoAWSCredentials("us-east-2:ebf6556b-3a85-4c18-ab5e-e5b30ab2105c", RegionEndpoint.USEast2);
        syncManager = new CognitoSyncManager(credentials, RegionEndpoint.USEast2);
        scoreDS = syncManager.OpenOrCreateDataset("scoreDS");
        scoreDS.OnSyncSuccess += ScoreDS_OnSyncSuccess;
        scoreDS.OnSyncFailure += ScoreDS_OnSyncFailure;
        StartCoroutine("UpdateScore");
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Synchronize()
    {
        if (!string.IsNullOrEmpty(scoreDS.Get("FacebookId")) && !FindObjectOfType<FacebookConnect>().isLoggedIn)
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_FB_NOTLOGGED);
        else
        {
            sync = true;
            scoreDS.SynchronizeOnConnectivity();
        }
    }

    private void ScoreDS_OnSyncFailure(object sender, SyncFailureEventArgs e)
    {
        Toast.ShowMessage(Toast.Type.error, Messages.ERROR_COGNITO_SYNC);
    }

    private void ScoreDS_OnSyncSuccess(object sender, SyncSuccessEventArgs e)
    {
        List<Record> newRecords = e.UpdatedRecords;
        for (int k = 0; k < newRecords.Count; k++)
        {
            Debug.Log(newRecords[k].Key + " was updated: " + newRecords[k].Value);
        }
        Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_COGNITO_SYNC);
        StartCoroutine("UpdateScore");
        sync = false;
    }


    public void ChangeName(string _newName)
    {
        try
        {
            theName = _newName;
            scoreDS.Put("name", theName);
        }
        catch
        {

        }
    }

    public void ChangeScore(string _newScore)
    {
        try
        {
            theScore = int.Parse(_newScore);
            scoreDS.Put("score", _newScore);
        }
        catch
        {

        }
    }


    public IEnumerator UpdateScore()
    {
        if (!string.IsNullOrEmpty(scoreDS.Get("name")))
        {
            theName = scoreDS.Get("name");
            nameIF.text = theName;
        }
        else
            nameIF.text = "";

        if (!string.IsNullOrEmpty(scoreDS.Get("score")))
        {
            theScore = int.Parse(scoreDS.Get("score"));
            scoreIF.text = theScore.ToString();
        }
        else
            scoreIF.text = "";

        yield return new WaitForSeconds(3.0f);

        Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_DATA_UPDATED);

    }

    public void FBHasLoggedIn(string _token, string _id)
    {
        string oldFacebookId = scoreDS.Get("FacebookId");
        if (string.IsNullOrEmpty(oldFacebookId) || _id.Equals(oldFacebookId))
        {
            scoreDS.Put("FacebookId", _id);
            credentials.AddLogin("graph.facebook.com", _token);
        }
        else
        {
            Toast.ShowMessage(Toast.Type.warning, Messages.WARNING_NEW_USER);
            credentials.Clear();
            scoreDS.Delete();
            credentials.AddLogin("graph.facebook.com", _token);
            Synchronize();
            StartCoroutine(WaitForEndOfSync(_id));
        }
    }

    private IEnumerator WaitForEndOfSync(string _id)
    {
        while (sync)
            yield return null;

        scoreDS.Put("FacebookId", _id);
    }
}
