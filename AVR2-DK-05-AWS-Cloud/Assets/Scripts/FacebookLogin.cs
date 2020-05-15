using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System;

public class FacebookLogin : MonoBehaviour
{

    [Header("UI Elements")]
    public GameObject FB_Login_UI;
    public GameObject FB_LoggedIn_UI;
    public GameObject S3_UI;
    public Image FB_ProfilePic_IMG;
    public Text FB_Username_TXT;

    [Header("Facebook Friends")]
    public GameObject FB_FriendPrefab;
    public GameObject FB_FriendsListRoot;

    [Header("S3 objects")]
    public GameObject S3_Cube;

    // Start is called before the first frame update
    void Awake()
    {
        if (!FB.IsInitialized)
            FB.Init(FBInitialized);
    }

    // Update is called once per frame
    void Start()
    {
        FB_Login_UI.SetActive(true);
        FB_LoggedIn_UI.SetActive(false);
        S3_UI.SetActive(false);
        S3_Cube.SetActive(false);
    }

    #region PUBLIC METHODS
    public void Login()
    {
        if (!FB.IsLoggedIn)
            FB.LogInWithReadPermissions(new List<string> { "user_friends" }, FBLoggedIn);
    }
    public void Logout()
    {
        FB.LogOut();
        foreach (Transform tr in FB_FriendsListRoot.GetComponentsInChildren<Transform>())
        {
            Destroy(tr.gameObject);
        }
        FB_Login_UI.SetActive(true);
        FB_LoggedIn_UI.SetActive(false);
    }
    public void GoToS3()
    {
        FB_Login_UI.SetActive(false);
        FB_LoggedIn_UI.SetActive(false);
        S3_UI.SetActive(true);
        S3_Cube.SetActive(true);
    }
    public void GoBack()
    {
        FB_Login_UI.SetActive(false);
        FB_LoggedIn_UI.SetActive(true);
        S3_UI.SetActive(false);
        S3_Cube.SetActive(false);
    }
    #endregion

    #region PRIVATE METHODS
    private void RetrieveFBProfilePic()
    {
        FB.API("me/picture?width=256&height=256", HttpMethod.GET, FB_ProfilePic_Retrieved);
    }
    private void RetrieveFBUsername()
    {
        FB.API("me?fields=name", HttpMethod.GET, FB_Username_Retrieved);
    }
    private void RetrieveFBFriends()
    {
        FB.API("me/friends", HttpMethod.GET, FB_Friends_Retrieved);
    }
    private void CreateFriend(string name, string id)
    {
        GameObject myFriend = Instantiate(FB_FriendPrefab, FB_FriendsListRoot.transform);
        myFriend.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
        FB.API(id + "/picture?width=256&height=256", HttpMethod.GET,
            delegate(IGraphResult graphResult)
            {
                myFriend.GetComponent<Image>().sprite = Sprite.Create(graphResult.Texture, new Rect(0.0f, 0.0f, 256.0f, 256.0f), new Vector2(0.5f, 0.5f));
            }
        );
    }
    #endregion

    #region CALLBACKS
    private void FBInitialized()
    {
        Debug.Log("Facebook has been initialized");
    }
    private void FBLoggedIn(ILoginResult result)
    {
        if (result.Error == null)
        {
            GetComponent<Cognito>().FBHasLoggedIn(
                AccessToken.CurrentAccessToken.TokenString,
                AccessToken.CurrentAccessToken.UserId
            );
            Debug.Log("Successfully logged in.");
            FB_Login_UI.SetActive(false);
            FB_LoggedIn_UI.SetActive(true);
            RetrieveFBProfilePic();
            RetrieveFBUsername();
            RetrieveFBFriends();
        } else
        {
            Debug.LogError("Facebook login failed.");
        }
    }
    private void FB_ProfilePic_Retrieved(IGraphResult result)
    {
        Texture2D profilePicTexture = result.Texture;
        FB_ProfilePic_IMG.sprite = Sprite.Create(profilePicTexture, new Rect(0.0f, 0.0f, 256.0f, 256.0f), new Vector2(0.5f, 0.5f));
    }
    private void FB_Username_Retrieved(IGraphResult result)
    {
        IDictionary<string, object> profile = result.ResultDictionary;
        FB_Username_TXT.text = profile["name"].ToString();
    }
    private void FB_Friends_Retrieved(IGraphResult result)
    {
        IDictionary<string, object> friends = result.ResultDictionary;
        List<object> friendsList = (List<object>)friends["data"];
        foreach (object friend in friendsList)
        {
            IDictionary<string, object> dictionary = (Dictionary<string, object>)friend;
            Debug.Log(dictionary["name"].ToString() + " / " + dictionary["id"].ToString());
            CreateFriend(dictionary["name"].ToString(), dictionary["id"].ToString());
        }
    }
    #endregion
}
