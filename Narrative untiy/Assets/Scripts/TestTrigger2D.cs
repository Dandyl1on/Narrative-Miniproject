using UnityEngine;

public class TestTrigger2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D fired with: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered 2D trigger!");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is inside 2D trigger...");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left 2D trigger.");
        }
    }
}
