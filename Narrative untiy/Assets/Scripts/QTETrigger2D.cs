using UnityEngine;

public class QTETrigger2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StoryController storyController;
    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        Debug.Log("[QTETrigger2D] Player entered trigger – waiting for F");
        // Her kan du evt. vise en "Press F" UI prompt
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        Debug.Log("[QTETrigger2D] Player left trigger");
        // Her kan du evt. skjule "Press F" UI prompt
    }

    private void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("[QTETrigger2D] Player pressed F – starting conversation + QTE");

            if (storyController != null)
            {
                storyController.TriggerQTE();
            }
            else
            {
                Debug.LogWarning("[QTETrigger2D] StoryController reference mangler.");
            }
        }
    }
}
