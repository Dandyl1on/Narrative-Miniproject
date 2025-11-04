using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Firepit : MonoBehaviour
{
    
    public AudioClip[] DeathSound;
    private AudioSource Audio;
    public Heat Heat;
    public Transform Respawn;
    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("FirePit"))
        {
            playsound();
        }
    }
    public void playsound()
    {
        int index = UnityEngine.Random.Range(0, DeathSound.Length);
        AudioClip death = DeathSound[index];
        Audio.clip = death;
        Audio.Play();
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.3f);
        transform.position = Respawn.position;
        Heat.beginHeat = false;
        Heat.HeatSlider.value = 0;
        Heat.isDead = true;
    }
    
}