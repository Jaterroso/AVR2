using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerStatistics : MonoBehaviour
{
    public Text textComponent;

    public void UpdatePlayerStatistics(PlayerStatistics playerStatistics)
    {
        textComponent.text = " Health :"+ playerStatistics.healthPoints.ToString();
        textComponent.text += "\n Strength : " + playerStatistics.strength.ToString();
        textComponent.text += "\n Speed : " + playerStatistics.speed.ToString();        
    }

    public void DisplayPlayerStatisticAsJSON(PlayerStatistics playerStatistics)
    {
        textComponent.text = JsonUtility.ToJson(playerStatistics);
    }
}
