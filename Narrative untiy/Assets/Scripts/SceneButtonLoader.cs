using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonLoader : MonoBehaviour
{
    /// <summary>
    /// Kaldes fra en UI-knap. Indtast scenenavn som parameter i OnClick.
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SceneButtonLoader] Scene-navn er tomt.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
