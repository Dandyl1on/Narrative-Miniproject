using UnityEngine;

public class Cameracontroller : MonoBehaviour
{

    [SerializeField] private Transform Player;
    [SerializeField] private float distance;
    [SerializeField] private float cameraSpeed;
    public ZiplineScript zipline;
    public bool OnZipline;
    public Playermovement player;

    private float lookAHead;
    private float lookLow;
    private float lookUp;
    
    private float lookOffsetY;
    
    // Update is called once per frame
    void Update()
    {
        float targetX = Player.position.x;
        float targetY = Player.position.y+2f;

        if (OnZipline)
        {
            targetX = Mathf.Lerp(transform.position.x, Player.position.x + 2f, Time.deltaTime * cameraSpeed);
            targetY = Mathf.Lerp(transform.position.y, Player.position.y - 1.5f, Time.deltaTime * cameraSpeed);
        }
        else
        {
            float targetOffSet = 0f;
            if (player.lookDown)
            {
                targetOffSet -= distance;
            }
            else if(player.lookUp)
            {
                targetOffSet = distance;
            }

            lookOffsetY = Mathf.Lerp(lookOffsetY, targetOffSet, Time.deltaTime * cameraSpeed);
            targetY += lookOffsetY;
        }
        transform.position = new Vector3(targetX, targetY, transform.position.z);
        
        /*if (!OnZipline) // Only follow normally if not on a zipline
        {
            
        }
        else
        {
            // Smoothly move camera to match zipline motion
            float targetX = Mathf.Lerp(transform.position.x, Player.position.x + 2f, Time.deltaTime * cameraSpeed);
            float targetY = Mathf.Lerp(transform.position.y, Player.position.y - 1.5f, Time.deltaTime * cameraSpeed);
            transform.position = new Vector3(targetX, targetY, transform.position.z);
           
        }

        if (player.lookDown)
        {
            lookLow = Mathf.Lerp(lookLow, distance, Time.deltaTime * cameraSpeed);
            transform.position = new Vector3(Player.position.x+lookAHead, Player.position.y - lookLow,
                transform.position.z);
        }
        else
        {
            // Reset smoothly when not looking down
            lookLow = Mathf.Lerp(lookLow, 0f+lookAHead, Time.deltaTime * cameraSpeed);
        }
        
        if (player.lookUp)
        {
            lookUp = Mathf.Lerp(lookUp, distance, Time.deltaTime * cameraSpeed);
            transform.position = new Vector3(Player.position.x+lookAHead, Player.position.y + lookUp,
                transform.position.z);
        }
        else
        {
            // Reset smoothly when not looking down
            lookUp = Mathf.Lerp(lookUp, 0f+lookAHead, Time.deltaTime * cameraSpeed);
        }*/
    }
}
