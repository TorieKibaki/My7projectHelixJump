using UnityEngine;
using TMPro; // ? Required for TextMeshPro UI components

public class UIManager : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TMP_Text textScore;  // TextMeshProUGUI for the current score
    [SerializeField] private TMP_Text textBest;   // TextMeshProUGUI for the best score

    void Update()
    {
        // Make sure GameManager.singleton exists before using it
        if (GameManager.singleton != null)
        {
            textScore.text = "Score: " + GameManager.singleton.score;
            textBest.text = "Best: " + GameManager.singleton.best;
        }
    }
}
