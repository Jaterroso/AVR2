using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class Warrior
{
    public string positionStr;
    public string rotationStr;

    public int health;
    public int increaseHealth { set { health += value; } }

    public int strength;
    public int increaseStrngth { set { strength += value; } }

    public int speed;
    public int increasSpeed { set { speed += value; } }

    public string gender;

    public string name;

    public int age;

    public GameObject prefab;

    public Vector3 position;
    public void SetPosition()
    {
        this.position = new Vector3(
            float.Parse(this.positionStr.Split(',')[0], CultureInfo.InvariantCulture),
            float.Parse(this.positionStr.Split(',')[1], CultureInfo.InvariantCulture),
            float.Parse(this.positionStr.Split(',')[2], CultureInfo.InvariantCulture)
        );
    }

    public Quaternion rotation;
    public void SetRotation()
    {
        this.rotation.eulerAngles = new Vector3(
            float.Parse(this.rotationStr.Split(',')[0], CultureInfo.InvariantCulture),
            float.Parse(this.rotationStr.Split(',')[1], CultureInfo.InvariantCulture),
            float.Parse(this.rotationStr.Split(',')[2], CultureInfo.InvariantCulture)
        );
    }
}
