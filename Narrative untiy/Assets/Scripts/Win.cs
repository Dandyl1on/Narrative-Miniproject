using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public GameObject Winscreen;

    public GameObject heatslider;
    public GameObject heattext;
    
    public Heat Heat;

    public Playermovement Playermovement;

    public AudioClip wins;
    private AudioSource winaudio;

    public AudioClip winneraudio;
    public AudioSource canvasaudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Winscreen.SetActive(false);
        winaudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Win"))
        {
            Winscreen.SetActive(true);
            Destroy(heatslider);
            Destroy(heattext);
            Heat.enabled = false;
            canvasaudio.loop = false;
            canvasaudio.clip = winneraudio;
            canvasaudio.volume = 0.5f;
            canvasaudio.Play();
            
            StartCoroutine(PlayWinAudioAfterDelay());
            
        }
    }
    private IEnumerator PlayWinAudioAfterDelay()
    {
        yield return new WaitForSeconds(2);
        winaudio.clip = wins;
        winaudio.Play();
        Time.timeScale = 0;
    }
}
