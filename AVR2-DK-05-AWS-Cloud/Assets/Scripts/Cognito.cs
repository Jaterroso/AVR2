using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amazon;
using Amazon.CognitoSync;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync.SyncManager;
using System;

public class Cognito : MonoBehaviour
{
    [Header("Cognito properties")]
    public CognitoAWSCredentials credentials;

    [Header("UI elements")]
    public InputField health_IF;
    public InputField experience_IF;
    public InputField force_IF;

    private CognitoSyncManager manager;
    private Dataset playerInfo;
    private int health, experience, force;
    private bool sync = false;

    #region PRIVATE METHODS
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        credentials = new CognitoAWSCredentials("us-east-2:7c943b75-f8dd-43f1-893a-13bcfe9c6166", RegionEndpoint.USEast2);
        manager = new CognitoSyncManager(credentials, RegionEndpoint.USEast2);
        playerInfo = manager.OpenOrCreateDataset("playerInfo");
        playerInfo.OnSyncSuccess += playerInfo_SyncSuccess;
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (!string.IsNullOrEmpty(playerInfo.Get("health")))
        {
            health = int.Parse(playerInfo.Get("health"));
            health_IF.text = health.ToString();
        }
        else
            health_IF.text = "";
        if (!string.IsNullOrEmpty(playerInfo.Get("experience")))
        {
            experience = int.Parse(playerInfo.Get("experience"));
            experience_IF.text = experience.ToString();
        }
        else
            experience_IF.text = "";
        if (!string.IsNullOrEmpty(playerInfo.Get("force")))
        {
            force = int.Parse(playerInfo.Get("force"));
            force_IF.text = force.ToString();
        }
        else
            force_IF.text = "";
    }
    #endregion

    #region PUBLIC METHODS
    public void Sync()
    {
        sync = true;
        playerInfo.SynchronizeOnConnectivity();
    }
    public void ChangeHealth(int newHealth)
    {
        newHealth = int.Parse(health_IF.text);
        health = newHealth;
        playerInfo.Put("health", newHealth.ToString());
    }
    public void ChangeExperience(int newExperience)
    {
        newExperience = int.Parse(experience_IF.text);
        experience = newExperience;
        playerInfo.Put("experience", newExperience.ToString());
    }
    public void ChangeForce(int newForce)
    {
        newForce = int.Parse(force_IF.text);
        force = newForce;
        playerInfo.Put("force", newForce.ToString());
    }
    public void FBHasLoggedIn(string token, string id)
    {
        string oldFacebookID = playerInfo.Get("FacebookID");
        if (string.IsNullOrEmpty(oldFacebookID) || id.Equals(oldFacebookID))
        {
            playerInfo.Put("FacebookID", id);
            credentials.AddLogin("graph.facebook.com", token);
        }
        else
        {
            credentials.Clear();
            playerInfo.Delete();
            credentials.AddLogin("graph.facebook.com", token);
            Sync();
            StartCoroutine(WaitForEndOfSync(id));
        }
    }

    private IEnumerator WaitForEndOfSync(string id)
    {
        while(sync)
            yield return null;

        playerInfo.Put("FacebookID", id);
    }
    #endregion

    #region CALLBACKS
    private void playerInfo_SyncSuccess(object sender, SyncSuccessEventArgs e)
    {
        List<Record> records = e.UpdatedRecords;
        foreach (Record r in records)
            Debug.Log(r.Key + " was updated: " + r.Value);

        UpdateUI();
        sync = false;
    }
    #endregion
}
