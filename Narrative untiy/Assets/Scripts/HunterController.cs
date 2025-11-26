using UnityEngine;

public class HunterController : MonoBehaviour
{
    [SerializeField] private float chaseProgress = 0f;
    [SerializeField] private float maxChase = 100f;

    [SerializeField] private float failPenalty = 25f;
    [SerializeField] private float successReward = 10f;

    public System.Action OnCaught;

    public void OnPlayerStumble()
    {
        chaseProgress += failPenalty;
        Debug.Log($"Hunter gained distance: {chaseProgress}/{maxChase}");

        if (chaseProgress >= maxChase)
        {
            Debug.Log("Hunter caught player!");
            OnCaught?.Invoke();
        }
    }

    public void OnPlayerDodged()
    {
        chaseProgress = Mathf.Max(0, chaseProgress - successReward);
        Debug.Log($"Hunter fell behind: {chaseProgress}/{maxChase}");
    }
}
