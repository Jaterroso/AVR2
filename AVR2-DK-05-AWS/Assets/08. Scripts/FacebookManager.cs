using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour
{
    #region SERIALIZED FIELDS
    [Header("UI elements")]
    [SerializeField]
    private Transform fbLogin_PNL;
    [SerializeField]
    private Transform fbConnected_PNL;
    [SerializeField]
    private Button fbLogout_BTN;
    [SerializeField]
    private Text fbUsername_LBL;

    [SerializeField]
    private Image fbUserProfilePic_IMG;
    [SerializeField]
    private GameObject friendPrefab;
    [SerializeField]
    private GameObject fbFriendsListRoot;

    [SerializeField]
    private Transform gamePanel;
    [SerializeField]
    private GameObject sphere;

    #endregion

    #region PRIVATE FIELDS
    private User currentUser;
    private bool isLoggedIn;
    #endregion

    #region MONOBEHAVIOUR METHODS
    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        if (FB.IsInitialized)
            FB.ActivateApp();
        else
            FB.Init(Facebook_Initialized);

    }

    // Start is called before the first frame update
    void Start()
    {
        currentUser = new User();
        fbConnected_PNL.gameObject.SetActive(false);
        fbLogout_BTN.gameObject.SetActive(false);
        fbLogin_PNL.gameObject.SetActive(true);
        fbUsername_LBL.gameObject.SetActive(false);
        fbUserProfilePic_IMG.gameObject.SetActive(false);
        fbUsername_LBL.text = null;
        if (fbFriendsListRoot.transform.childCount != 0)
        {
            for (int friendIndex = 0; friendIndex < fbFriendsListRoot.transform.childCount; friendIndex++)
                Destroy(fbFriendsListRoot.transform.GetChild(friendIndex).gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region PUBLIC METHODS
    public void Login()
    {
        var permissions = new List<string>() { "user_friends" };
        if (!FB.IsLoggedIn)
            FB.LogInWithReadPermissions(permissions, Facebook_Connected);
    }

    public void Logout()
    {
        try
        {
            FB.LogOut();
            isLoggedIn = false;
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_FB_LOGOUT);
            fbUsername_LBL.text = null;
            fbUsername_LBL.gameObject.SetActive(false);
            fbLogin_PNL.gameObject.SetActive(true);
            fbConnected_PNL.gameObject.SetActive(false);
            //sphere.SetActive(false);
            //gamePanel.gameObject.SetActive(false);
            fbLogout_BTN.gameObject.SetActive(false);
            fbUserProfilePic_IMG.gameObject.SetActive(false);
            fbUserProfilePic_IMG.sprite = Sprite.Create(new Texture2D(256, 256), new Rect(0.0f, 0.0f, 256.0f, 256.0f), new Vector2(0.5f, 0.5f));
        }
        catch (Exception _e)
        {
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_FB_LOGOUT + ": " + _e.Message);
        }
        if (fbFriendsListRoot.transform.childCount != 0)
        {
            for (int friendIndex = 0; friendIndex < fbFriendsListRoot.transform.childCount; friendIndex++)
                Destroy(fbFriendsListRoot.transform.GetChild(friendIndex).gameObject);
        }
    }
    public void Invite()
    {
        FB.AppRequest(message: "You should really try this game!", title: "Check this awesome game I have made!");
    }
    public void Share()
    {
        FB.ShareLink(
            new Uri("http://www.eonreality.com"),
            "This website is awesome!",
            "A description of the website.",
            new Uri("https://pbs.twimg.com/profile_images/1026570168980262913/KSBlclju_400x400.jpg"),
            Facebook_Share
        );
    }
    #endregion

    #region CALLBACKS
    private void SceneManager_sceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        if (_scene.isLoaded)
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_GAME_LOADED);
        else
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_GAME_LOADED);
    }

    private void Facebook_Initialized()
    {
        Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_FB_INIT);
    }

    private void Facebook_Connected(ILoginResult _result)
    {
        if (_result.Error == null)
        {
            currentUser.id = AccessToken.CurrentAccessToken.UserId;
            currentUser.token = AccessToken.CurrentAccessToken.TokenString;
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_FB_LOGIN);
            //Transform.FindObjectOfType<AWSCognito>().FBHasLoggedIn(currentUser.token, currentUser.id);
            fbUsername_LBL.gameObject.SetActive(true);
            fbUserProfilePic_IMG.gameObject.SetActive(true);
            fbLogin_PNL.gameObject.SetActive(false);
            fbConnected_PNL.gameObject.SetActive(true);
            fbLogout_BTN.gameObject.SetActive(true);
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
    
    private void Facebook_RetrieveProfilePic(IGraphResult _result)
     
        currentUser.profilePic = _result.Texture;
        GameObject.Find("FBProfilePic_IMG").GetComponent<Image>().sprite = Sprite.Create(currentUser.profilePic, new Rect(0.0f, 0.0f, 256.0f, 256.0f), new Vector2(0.5f, 0.5f));
    }

    private void Facebook_RetrieveUsername(IGraphResult _result)
    {
        currentUser.userName = _result.ResultDictionary["name"].ToString();
        fbUsername_LBL.text = currentUser.userName;
    }

    private void Facebook_RetrieveFriends(IGraphResult _result)
    {
        List<object> friendsList = (List<object>)_result.ResultDictionary["data"];
        foreach (object friend in friendsList)
        {
            Dictionary<string, object> friendDictionary = (Dictionary<string, object>)friend;
            //CreateFriend(friendDictionary["name"].ToString(), friendDictionary["id"].ToString());
        }
    }
    private void Facebook_Share(IShareResult _shareResult)
    {
        if (_shareResult.Error != null)
            Toast.ShowMessage(Toast.Type.success, Messages.SUCCESS_SHARE);
        else
            Toast.ShowMessage(Toast.Type.error, Messages.ERROR_SHARE);
    }
    #endregion

    #region PRIVATE METHODS
    private void RetrieveFBProfilePic()
    {
        FB.API("me/picture?width=256&height=256", HttpMethod.GET, Facebook_RetrieveProfilePic);
    }

    private void RetrieveFBUsername()
    {
        FB.API("me?fields=name", HttpMethod.GET, Facebook_RetrieveUsername);
    }
    private void RetrieveFBFriendsList()
    {
        FB.API("me/friends", HttpMethod.GET, Facebook_RetrieveFriends);
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
