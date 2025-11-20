using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    [SerializeField] private string fallbackSceneName = "MainMenu";

    public void OnRestartButton()
    {
        if (SceneHistory.Instance == null)
        {
            Debug.LogWarning("[DeathScreenController] SceneHistory mangler – loader fallback.");
            SceneManager.LoadScene(fallbackSceneName);
            return;
        }

        string previous = SceneHistory.Instance.PreviousSceneName;

        if (!string.IsNullOrEmpty(previous))
        {
            Debug.Log("[DeathScreenController] Restarting previous scene: " + previous);
            SceneManager.LoadScene(previous);
        }
        else
        {
            Debug.LogWarning("[DeathScreenController] PreviousSceneName er tom – loader fallback.");
            SceneManager.LoadScene(fallbackSceneName);
        }
    }

    public void OnQuitToMenuButton()
    {
        Debug.Log("[DeathScreenController] Quit to menu.");
        SceneManager.LoadScene(fallbackSceneName);
    }
}
