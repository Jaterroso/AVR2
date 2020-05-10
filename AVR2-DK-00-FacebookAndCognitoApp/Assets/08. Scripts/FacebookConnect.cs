using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using System;

public class FacebookConnect : MonoBehaviour
{
    #region FIELDS
    public Transform loginPanel;
    public Transform loggedInPanel;
    public Transform gamePanel;
    public GameObject sphere;
    public Button logoutButton;
    public Text usernameLabel;
    public Image userProfilePic;
    public GameObject friendPrefab;
    public GameObject friendsListRoot;
    public User currentUser;
    public bool isLoggedIn;
    #endregion



    #region PRIVATE METHODS
    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        if (FB.IsInitialized)
            FB.ActivateApp();
        else
            FB.Init(FBInit_CB);

    }

    // Start is called before the first frame update
    private void Start()
    {
        currentUser = new User();
        loggedInPanel.gameObject.SetActive(false);
        logoutButton.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
        gamePanel.gameObject.SetActive(false);
        sphere.SetActive(false);
        usernameLabel.gameObject.SetActive(false);
        userProfilePic.gameObject.SetActive(false);
        usernameLabel.text = null;
        if (friendsListRoot.transform.childCount != 0)
        {
            for (int friendIndex = 0; friendIndex < friendsListRoot.transform.childCount; friendIndex++)
                Destroy(friendsListRoot.transform.GetChild(friendIndex).gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void RetrieveFBProfilePic()
    {
        FB.API("me/picture?width=256&height=256", HttpMethod.GET, FBRetrieveProfilePic_CB);
    }

    private void RetrieveFBUsername()
    {
        FB.API("me?fields=name", HttpMethod.GET, RetrieveFBUsername_CB);
    }

    private void RetrieveFBFriendsList()
    {
        FB.API("me/friends", HttpMethod.GET, RetrieveFBFriendsList_CB);
    }
    #endregion

    #region PUBLIC METHODS
    public void Login()
    {
        var permissions = new List<string>() { "user_friends" };
        if (!FB.IsLoggedIn)
            FB.LogInWithReadPermissions(permissions, FBLogin_CB);
    }

    public void Logout()
    {
        try
        {
            FB.LogOut(); 
            isLoggedIn = false;
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_FB_LOGOUT);
            usernameLabel.text = null;
            usernameLabel.gameObject.SetActive(false);
            loginPanel.gameObject.SetActive(true);
            loggedInPanel.gameObject.SetActive(false);
            sphere.SetActive(false);
            gamePanel.gameObject.SetActive(false);
            logoutButton.gameObject.SetActive(false);
            userProfilePic.gameObject.SetActive(false);
            userProfilePic.sprite = Sprite.Create(new Texture2D(256, 256), new Rect(0.0f, 0.0f, 256.0f, 256.0f), new Vector2(0.5f, 0.5f));
        }
        catch (Exception _e)
        {
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_FB_LOGOUT + ": " + _e.Message);
        }
        if (friendsListRoot.transform.childCount != 0) {
            for (int friendIndex = 0; friendIndex < friendsListRoot.transform.childCount; friendIndex++)
                Destroy(friendsListRoot.transform.GetChild(friendIndex).gameObject);
        }
    }

    public void CreateFriend(string _name, string _id)
    {
        GameObject myFriend = Instantiate(friendPrefab, friendsListRoot.transform);
        myFriend.GetComponentInChildren<Text>().text = _name;
        FB.API(_id + "/picture?wifth=256&height=256", HttpMethod.GET,
            delegate (IGraphResult _result)
            {
                try
                {
                    myFriend.GetComponentInChildren<Image>().sprite = Sprite.Create(_result.Texture, new Rect(0.0f, 0.0f, 256.0f, 256.0f), new Vector2(0.5f, 0.5f));
                } catch (ArgumentException _ae)
                {
                    Debug.Log(_ae.Message);
                }
            }
        );
    }

    public void Invite()
    {
        FB.AppRequest(message: "You should really try this game!", title: "Check this awesome game I have made!");
    }

    public void Share()
    {
        FB.ShareLink(
            new Uri("http://www.eonreality.com"),
            "This game is awesome!",
            "A description of the game.",
            new Uri("https://pbs.twimg.com/profile_images/1026570168980262913/KSBlclju_400x400.jpg"),
            Share_CB
        );
    }

    public void PlayGame()
    {
        loggedInPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
        sphere.SetActive(true);
    }


    #endregion

    #region CALLBACKS
    private void FBInit_CB()
    {
        Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_FB_INIT);
    }

    private void FBLogin_CB(ILoginResult _result)
    {
        if (_result.Error == null)
        {
            currentUser.id = AccessToken.CurrentAccessToken.UserId;
            currentUser.token = AccessToken.CurrentAccessToken.TokenString;
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_FB_LOGIN);
            Transform.FindObjectOfType<AWSCognito>().FBHasLoggedIn(currentUser.token, currentUser.id);
            usernameLabel.gameObject.SetActive(true);
            userProfilePic.gameObject.SetActive(true);
            loginPanel.gameObject.SetActive(false);
            loggedInPanel.gameObject.SetActive(true);
            logoutButton.gameObject.SetActive(true);
            RetrieveFBProfilePic();
            RetrieveFBUsername();
            RetrieveFBFriendsList();
            isLoggedIn = true;
        }
        else
        {
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_FB_LOGIN + Environment.NewLine + _result.Error);
            isLoggedIn = false;
        }
    }

    private void FBRetrieveProfilePic_CB(IGraphResult _result)
    {
        currentUser.profilePic = _result.Texture;
        GameObject.Find("UserProfilePic").GetComponent<Image>().sprite = Sprite.Create(currentUser.profilePic, new Rect(0.0f, 0.0f, 256.0f, 256.0f), new Vector2(0.5f, 0.5f));
    }

    private void RetrieveFBUsername_CB(IGraphResult _result)
    {
        currentUser.userName = _result.ResultDictionary["name"].ToString();
        GameObject.Find("UserFullname").GetComponent<Text>().text = currentUser.userName;
    }

    private void RetrieveFBFriendsList_CB(IGraphResult _result)
    {
        List<object> friendsList = (List<object>)_result.ResultDictionary["data"];
        foreach (object friend in friendsList)
        {
            Dictionary<string, object> friendDictionary = (Dictionary<string, object>)friend;
            CreateFriend(friendDictionary["name"].ToString(), friendDictionary["id"].ToString());
        }
    }

    private void Share_CB(IShareResult _shareResult)
    {
        if (_shareResult.Error != null)
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_SHARE);
        else
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_SHARE);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.isLoaded)
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_GAME_LOADED);
        else
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_GAME_LOADED);
    }
    #endregion
}

[Serializable]
public class User
{
    public Texture2D profilePic { get; set; }
    public string userName { get; set; }
    public string id { get; set; }
    public string token { get; set; }

    public User()
    {
        profilePic = null;
        userName = null;
        id = null;
        token = null;
    }

    public User(Texture2D _profilePic, string _userName)
    {
        profilePic = _profilePic;
        userName = _userName;
        id = null;
        token = null;
    }
}
