using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpdateJSON : MonoBehaviour
{
    public InputField inputField;
    public Trainer trainer;

    public void JsonUpdate()
    {
        trainer = JsonUtility.FromJson<Trainer>(inputField.text);
    }
}
