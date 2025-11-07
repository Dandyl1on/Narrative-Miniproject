using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Heat : MonoBehaviour
{
    public Slider HeatSlider;
    public float IncreaseHeat;
    public float DecreaseHeat;
    
    public bool beginHeat;
    public Firepit Firepit;

    public Transform Respawn;
    public bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HeatSlider.GetComponent<Slider>();
    }

    

    private void Update()
    {
        if (beginHeat)
        {
            HeatSlider.value += 4.5f * Time.deltaTime;
        }
        
        if (HeatSlider.value == HeatSlider.maxValue)
        {
            transform.position = Respawn.position;
            beginHeat = false;
            HeatSlider.value = 0;
            isDead = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.CompareTag("Flames"))
        {
            HeatSlider.value += IncreaseHeat * Time.deltaTime;
            if (HeatSlider.value == HeatSlider.maxValue)
            {
                Firepit.playsound();
                transform.position = Respawn.position;
                beginHeat = false;
                HeatSlider.value = 0;
                isDead = true;
            }
        }

        if (other.CompareTag("Water"))
        {
            HeatSlider.value-= Time.deltaTime * DecreaseHeat;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Heat begin"))
        {
            beginHeat = true;
            isDead = false;
        }
    }
}
