using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int total = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        UpdateUI();
    }

    public void RegisterResult(bool correct)
    {
        total++;

        if (correct)
            score++;

        Debug.Log($"Score: {score} / {total}");
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {score} / {total}";
    }
}
