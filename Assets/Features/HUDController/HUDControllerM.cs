using UnityEngine;

[System.Serializable]
public class HUDControllerM
{
    [Header("Configuration")]
    public int HealthTextSize;
    public Color HealthTextColor;
    [Space]
    public int UnitsRemainingTextSize;
    public Color UnitsRemainingTextColor;
    [Space]
    public int ScoreTextSize;
    public Color ScoreTextColor;
    [Space]
    public int LevelTextSize;
    public Color LevelTextColor;
}