using UnityEngine;

public class RunnerObstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float qteTriggerX = 2f;  // ahead of player
    [SerializeField] private RunnerQTEController qteController;

    private bool qteStarted = false;
    private bool resolved = false;

    private void Update()
    {
        // Move toward player
        transform.position += Vector3.left * moveSpeed * Time.unscaledDeltaTime * Time.timeScale;

        // Trigger QTE early
        if (!qteStarted && transform.position.x <= qteTriggerX)
        {
            qteStarted = true;
            qteController.StartObstacleQTE(this);
        }

        // Remove obstacle after resolved
        if (resolved && transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }

    public void HandleSuccess()
    {
        Debug.Log("[RunnerObstacle] SUCCESS - playing dodge sequence.");
        resolved = true;

        // Optionally disable renderer so it disappears instantly
        GetComponent<SpriteRenderer>().enabled = false;

        // Or move it instantly past the player
        transform.position = new Vector3(-15, transform.position.y, transform.position.z);
    }

    public void HandleFail()
    {
        Debug.Log("[RunnerObstacle] FAIL - the player trips and slows.");
        resolved = true;
        // Keep or destroy obstacle – doesn't matter
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
