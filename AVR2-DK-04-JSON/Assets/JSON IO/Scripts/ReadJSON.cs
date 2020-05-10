using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ReadJSON : MonoBehaviour
{
    private string path = null;
    public InputField inputField;
    public Image profilePic;
    public Trainer trainer;
    public Text console;

    private void Awake()
    {
        path = Application.dataPath + "/JSON IO/";
    }

    public void Read(string trainerName)
    {
        console.text = "";
        // opens the stream at targeted location
        StreamReader reader = new StreamReader(path + trainerName + ".txt");

        // Store the value of the reader
        string result = reader.ReadToEnd();

        // Close the stream
        reader.Close();

        // Update the text
        inputField.text = result;

        // Create an object based on the JSON string
        trainer = JsonUtility.FromJson<Trainer>(result);

        // Profile pic
        profilePic.sprite = LoadImage(trainer.image);
    }

    private Sprite LoadImage(string fileName)
    {
        Texture2D tex = new Texture2D(512, 512);
        byte[] fileData;
        fileData = File.ReadAllBytes("Assets/JSON IO/Textures/" + fileName);
        tex.LoadImage(fileData);

        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0,0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f),
            100
        );

        return sprite;
    }
}
