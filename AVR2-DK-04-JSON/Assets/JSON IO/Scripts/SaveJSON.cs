using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveJSON : MonoBehaviour
{
    private string path = null;
    public InputField inputField;
    private Trainer trainer;
    [SerializeField]
    private Text console;

    private void Awake()
    {
        path = Application.dataPath + "/JSON IO/";
    }

    public void Write(bool append)
    {
        trainer = GetComponent<UpdateJSON>().trainer;

        // Opens a stream to access the location and will append or overwrite the content
        string fileName = FirstLetterUpper(trainer.firstname) + FirstLetterUpper(trainer.lastname) + ".txt";
        StreamWriter writer = new StreamWriter(path + fileName);

        // Add a line inside the current open stream
        writer.WriteLine(JsonUtility.ToJson(trainer, true));

        // close the stream
        writer.Close();

        // Update the object in the ReadJSON component
        GetComponent<ReadJSON>().trainer = trainer;

        // Update the console
        console.text = "File " + fileName + " updated.";
    }

    public string FirstLetterUpper(string _string)
    {
        return _string.ToCharArray()[0].ToString().ToUpper() + _string.Substring(1, _string.Length - 1);
    }
}
