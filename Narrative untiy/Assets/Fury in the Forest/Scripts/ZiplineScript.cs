using UnityEngine;
using System.Collections;

public class ZiplineScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Player;
    public GameObject Destination;
    private bool isOnZipline = false;

    public Animator ani;
    public Cameracontroller cam;

    void Update()
    {
        ani.SetBool("ZipLine", cam.OnZipline);
        if (isOnZipline && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MovePlayerOnZipline());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Ensure the tag is correct (case-sensitive)
        {
            isOnZipline = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isOnZipline = false;
        }
    }

    IEnumerator MovePlayerOnZipline()
    {
        float duration = 1.5f; // Time in seconds to reach the end
        float elapsedTime = 0f;
        Vector2 start = Player.transform.position;
        Vector2 end = Destination.transform.position;
        
        cam.OnZipline = true; // Notify the camera to stop following normally
        
        while (elapsedTime < duration)
        {
            Player.transform.position = Vector2.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Player.transform.position = end; // Ensure the player reaches the destination
        
        cam.OnZipline = false;

    }
}

