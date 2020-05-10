using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectStructure : ScriptableObject
{
    [MenuItem("EON Reality/Create project structure")]
    static void MenuCreateProjectStructure()
    {
        CreateProjectStructure();
    }

    static void CreateProjectStructure()
    {
        string f = Application.dataPath + "/";
        if (!Directory.Exists(f + "01. Scenes"))
            Directory.CreateDirectory(f + "01. Scenes");
        if (!Directory.Exists(f + "02. Models"))
            Directory.CreateDirectory(f + "02. Models");
        if (!Directory.Exists(f + "03. Prefabs"))
            Directory.CreateDirectory(f + "03. Prefabs");
        if (!Directory.Exists(f + "04. Materials"))
            Directory.CreateDirectory(f + "04. Materials");
        if (!Directory.Exists(f + "05. Textures"))
            Directory.CreateDirectory(f + "05. Textures");
        if (!Directory.Exists(f + "06. Audio"))
            Directory.CreateDirectory(f + "06. Audio");
        if (!Directory.Exists(f + "07. Videos"))
            Directory.CreateDirectory(f + "07. Videos");
        if (!Directory.Exists(f + "08. Scripts"))
            Directory.CreateDirectory(f + "08. Scripts");
        if (!Directory.Exists(f + "09. Shaders"))
            Directory.CreateDirectory(f + "09. Shaders");
        if (!Directory.Exists(f + "10. Animations"))
            Directory.CreateDirectory(f + "10. Animations");
        if (!Directory.Exists(f + "11. 3rd-Party"))
            Directory.CreateDirectory(f + "11. 3rd-Party");
        if (!Directory.Exists(f + "12. Utils"))
            Directory.CreateDirectory(f + "12. Utils");
        Debug.Log("Project structure has been applied.");
    }
}
