using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class ReadWriteContent : MonoBehaviour
{
    public string Path = "Assets/IO File/HelloUnityWorld.txt";
    public InputField inputField;

    public void Write(bool append)
    {
        // Opens a stream to access the location and will append or overwrite the content
        StreamWriter writer = new StreamWriter(Path, append);

        // Add a line inside the current open stream
        writer.WriteLine(inputField.text);

        // close the stream
        writer.Close();
    }

    public void Read()
    {
        // opens the stream at targeted location
        StreamReader reader = new StreamReader(Path);

        // Store the value of the reader
        string result = reader.ReadToEnd();        

        // Close the stream
        reader.Close();

        // Update the text
        inputField.text = result;
    }
}
