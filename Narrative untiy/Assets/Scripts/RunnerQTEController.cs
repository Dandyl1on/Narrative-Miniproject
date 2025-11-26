using UnityEngine;

public class RunnerQTEController : MonoBehaviour
{
    [SerializeField] private StoryController storyController;
    [SerializeField] private SimpleQTE qte;
    [SerializeField] private HunterController hunter;

    [Header("Slowmotion")]
    [SerializeField] private float slowMotionScale = 0.2f;

    private RunnerObstacle currentObstacle;
    private bool qteActive = false;

    public void StartObstacleQTE(RunnerObstacle obstacle)
    {
        if (qteActive) return;

        qteActive = true;
        currentObstacle = obstacle;

        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        storyController.TriggerQTE();
    }

    public void QTESuccess()
    {
        ResetTime();
        currentObstacle?.HandleSuccess();
        hunter?.OnPlayerDodged();
        qteActive = false;
        currentObstacle = null;
    }

    public void QTEFail()
    {
        ResetTime();
        currentObstacle?.HandleFail();
        hunter?.OnPlayerStumble();
        qteActive = false;
        currentObstacle = null;
    }

    private void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
