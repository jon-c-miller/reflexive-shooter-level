using UnityEngine;

public class HUDControllerV : MonoBehaviour
{
    [SerializeField] Canvas healthCanvas;
    [SerializeField] UnityEngine.UI.Text healthText;
    [Space]
    [SerializeField] Canvas unitsRemainingCanvas;
    [SerializeField] UnityEngine.UI.Text unitsRemainingText;
    [Space]
    [SerializeField] Canvas scoreCanvas;
    [SerializeField] UnityEngine.UI.Text scoreText;

    public void SetUIActiveStatus(bool isActive)
    {
        healthCanvas.enabled = isActive;
        unitsRemainingCanvas.enabled = isActive;
        scoreCanvas.enabled = isActive;
    }

    public void SetTextSizeAndColor(HUDControllerM model)
    {
        healthText.color = model.HealthTextColor;
        healthText.fontSize = model.HealthTextSize;
        unitsRemainingText.color = model.UnitsRemainingTextColor;
        unitsRemainingText.fontSize = model.UnitsRemainingTextSize;
        scoreText.color = model.ScoreTextColor;
        scoreText.fontSize = model.ScoreTextSize;
    }

    public void UpdateHealthDisplay(int health)
    {
        healthText.text = health.ToString();
    }

    public void UpdateUnitsRemainingDisplay(int unitsRemaining)
    {
        unitsRemainingText.text = unitsRemaining.ToString();
    }

    public void UpdateScoreDisplay(int score)
    {
        scoreText.text = score.ToString();
    }
}