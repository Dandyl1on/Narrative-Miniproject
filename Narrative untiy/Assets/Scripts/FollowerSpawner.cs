using UnityEngine;

public class FollowerSpawner : MonoBehaviour
{
    public GameObject Cat;
    public GameObject Hedgehog;
    public GameObject Squrriel;
    public Transform Catrespawn;
    public Transform Hedgehogrespawn;
    public Transform Squrrielrespawn;

    void Start()
    {
        Debug.Log("Starting followers");
        if (GameObject.FindWithTag("FollowerCat")) return;
        Instantiate(Cat, Catrespawn.position, Quaternion.identity);
        Debug.Log("Follower is being spawned");
        Instantiate(Hedgehog, Hedgehogrespawn.position, Quaternion.identity);
        Instantiate(Squrriel, Squrrielrespawn.position, Quaternion.identity);

    }
}
