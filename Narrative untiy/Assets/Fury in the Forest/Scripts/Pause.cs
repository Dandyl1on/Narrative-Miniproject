using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool stop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stop)
        {
            Time.timeScale = 0;
        }
        if (stop && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 1;
            stop = false;
        }
    }
}
