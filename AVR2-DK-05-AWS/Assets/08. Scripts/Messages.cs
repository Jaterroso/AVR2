using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Messages
{
    public const string ERROR_FB_INIT = "Error initializing Facebook SDK: ";
    public const string ERROR_FB_LOGIN = "Error logging in to Facebook: ";
    public const string ERROR_FB_LOGOUT = "Error logging out from Facebook: ";
    public const string ERROR_FB_NOTLOGGED = "You must be logged in to sync data.";
    public const string ERROR_GAME_LOADED = "Error while loading the game.";
    public const string ERROR_SHARE = "Sharing on Facebook failed. ";
    public const string ERROR_COGNITO_SYNC = "Sync with Cognito Sync failed.";

    public const string SUCCESS_FB_INIT = "Facebook has been initialized.";
    public const string SUCCESS_FB_LOGIN = "Successfully logged in on Facebook.";
    public const string SUCCESS_FB_LOGOUT = "Successfully logged out from Facebook.";
    public const string SUCCESS_GAME_LOADED = "Game successfully loaded.";
    public const string SUCCESS_SHARE = "Successfully shared on Facebook.";
    public const string SUCCESS_COGNITO_SYNC = "Sync with Cognito Sync successful.";
    public const string SUCCESS_DATA_UPDATED = "Data have been updated.";

    public const string WARNING_NEW_USER = "New user created.";
}
