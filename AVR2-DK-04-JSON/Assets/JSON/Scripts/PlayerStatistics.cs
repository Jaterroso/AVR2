using System;
using UnityEngine;

[Serializable]
public class PlayerStatistics : MonoBehaviour
{
    public int healthPoints;
    public int AddHealthPoints { set { healthPoints += value; } }

    public int strength;
    public int AddStrengthPoints { set { strength += value; } }

    public int speed;
    public int AddSpeedPoints { set { speed += value; } }
}
