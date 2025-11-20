using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistory : MonoBehaviour
{
    public static SceneHistory Instance { get; private set; }

    public string PreviousSceneName { get; private set; }
    public string CurrentSceneName { get; private set; }

    private void Awake()
    {
        // Singleton-setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Lyt på sceneskift
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Init for den første scene
        CurrentSceneName = SceneManager.GetActiveScene().name;
        PreviousSceneName = null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PreviousSceneName = CurrentSceneName;
        CurrentSceneName = scene.name;
        // Debug.Log($"[SceneHistory] Previous={PreviousSceneName}, Current={CurrentSceneName}");
    }
}
