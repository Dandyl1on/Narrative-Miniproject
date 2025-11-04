using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool isOnFire;
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOnFire = true;
    }

    public void Extinguish()
    {
        if (isOnFire)
        {
            Destroy(gameObject);
        }
    }
}