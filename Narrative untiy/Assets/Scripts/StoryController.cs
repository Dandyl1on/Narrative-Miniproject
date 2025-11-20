using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleQTE qte;
    [SerializeField] private EyeScanner2D eyeScanner;   // valgfri, kun hvis du vil disable øjet imens

    [Header("Scenes")]
    [SerializeField] private string deathSceneName = "DeathScene";

    private bool qteActive = false;

    public void TriggerQTE()
    {
        // Hvis der allerede kører en QTE, så gør ingenting
        if (qteActive)
        {
            Debug.Log("[StoryController] QTE already active – ignoring new trigger.");
            return;
        }

        if (qte == null)
        {
            Debug.LogWarning("[StoryController] QTE reference mangler.");
            return;
        }

        qteActive = true;

        // Valgfrit: stop øjet mens QTE kører
        if (eyeScanner != null)
            eyeScanner.enabled = false;

        Debug.Log("[StoryController] Starting QTE.");
        qte.StartQTE();
    }

    public void OnQTESuccessHandler()
    {
        Debug.Log("[StoryController] QTE SUCCESS");

        qteActive = false;

        // Hvis du vil have øjet i gang igen efter en succes:
        if (eyeScanner != null)
            eyeScanner.enabled = true;

        // TODO: fortsæt historien her
    }

    public void OnQTEFailHandler()
    {
        Debug.Log("[StoryController] QTE FAIL – loading death scene: " + deathSceneName);

        qteActive = false;

        // Death scene – ingen grund til at re-enable scanner, vi skifter scene
        if (!string.IsNullOrEmpty(deathSceneName))
        {
            SceneManager.LoadScene(deathSceneName);
        }
        else
        {
            Debug.LogWarning("[StoryController] deathSceneName er ikke sat.");
        }
    }
}
