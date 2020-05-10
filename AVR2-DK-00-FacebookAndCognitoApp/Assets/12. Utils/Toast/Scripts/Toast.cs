using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Toast : MonoBehaviour
{
    #region ENUMS
    public enum Time
    {
        threeSeconds,
        twoSeconds,
        oneSecond
    };
    public enum Position
    {
        top,
        bottom
    };
    public enum Type
    {
        error,
        warning,
        success
    };
    #endregion

    #region FIELDS
    private static List<Sprite> icons;
    private static GameObject messagePrefab;
    #endregion

    #region PUBLIC METHODS
    public static void ShowMessage(Toast.Type _type, string _msg, Toast.Position _position = Position.bottom, Toast.Time _time = Time.threeSeconds)
    {
        // Load message icons from resources folder
        if (icons == null)
        {
            icons = new List<Sprite>() {
                Resources.Load<Sprite>("Sprites/error") as Sprite,
                Resources.Load<Sprite>("Sprites/warning") as Sprite,
                Resources.Load<Sprite>("Sprites/success") as Sprite
            };
        }


        // Load message prefab from resources folder
        if (messagePrefab == null)
            messagePrefab = Resources.Load("Prefabs/Message") as GameObject;

        //Get container object of message
        GameObject containerObject = messagePrefab.gameObject.transform.GetChild(0).gameObject;
        Image toastImage = containerObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Text toastText = containerObject.transform.GetChild(0).GetChild(1).GetComponent<Text>();

        //Set message to text ui
        toastText.text = _msg;

        SetType(toastImage, _type);

        //Set position of container object of message
        SetPosition(containerObject.GetComponent<RectTransform>(), _position);

        //Spawn message object with all changes
        GameObject toastInstance = Instantiate(messagePrefab);

        // Destroy clone of message object according to the time
        DestroyToast(toastInstance, _time);
    }
    #endregion

    #region PRIVATE METHODS
    private static void SetPosition(RectTransform rectTransform, Position position = Position.bottom)
    {
        if (position == Position.top)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector3(0.5f, -100f, 0);
        }
        else
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.anchoredPosition = new Vector3(0.5f, 100f, 0);
        }
    }

    private static void SetType(Image _image, Type _type)
    {
        switch (_type) {
            case Type.error:
                _image.sprite = icons[(int)Type.error];
                break;
            case Type.warning:
                _image.sprite = icons[(int)Type.warning];
                break;
            case Type.success:
                _image .sprite = icons[(int)Type.success];
                break;
        }
    }

    private static void DestroyToast(GameObject clone, Time time)
    {
        if (time == Time.oneSecond)
        {
            Destroy(clone.gameObject, 1f);
        }
        else if (time == Time.twoSeconds)
        {
            Destroy(clone.gameObject, 2f);
        }
        else
        {
            Destroy(clone.gameObject, 3f);
        }
    }
    #endregion
}