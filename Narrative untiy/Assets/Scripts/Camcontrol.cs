using UnityEngine;

public class Camcontrol : MonoBehaviour
{
    public Transform Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Camera controls
        float targetX = Player.position.x;
        float targetY = Player.position.y+2;
        
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
