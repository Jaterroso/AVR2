using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONExampleScript : MonoBehaviour
{
    /// <summary>
    /// Place a unity game monobehaviour or game object to serialize
    /// </summary>
    public Object objectToConvert;

    /// <summary>
    /// The result of the JSON Conversion
    /// </summary>
    public string JSONResult;

    /// <summary>
    /// Saves the JSON value
    /// </summary>
    public void Save()
    {
        if (objectToConvert)
        {
            JSONResult = JsonUtility.ToJson(objectToConvert);
        }
    }

    public void Load()
    {
        if (JSONResult.Length > 0 && objectToConvert)
        { 
            JsonUtility.FromJsonOverwrite(JSONResult, objectToConvert);
        }
    }
}
