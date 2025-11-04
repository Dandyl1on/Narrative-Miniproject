using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SignController : MonoBehaviour
{
    public TextMeshProUGUI toolText;

    public Image A;
    public Image D;
    public Image W;
    public Image S;
    public Sprite E;
    public Image Space;
    public Pause sign;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toolText.enabled = false;
        A.enabled = false;
        D.enabled = false;
        W.enabled = false;
        S.enabled = false;
        Space.enabled = false;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sign"))
        {
            sign = other.GetComponent<Pause>();
            sign.stop = true;
            toolText.enabled = true;
            A.enabled = true;
            D.enabled = true;
            W.enabled = true;
            S.enabled = true;
            Space.enabled = true;
            
        }

        if (other.CompareTag("Rescue sign"))
        {
            sign = other.GetComponent<Pause>();
            sign.stop = true;

            toolText.enabled = true;
            toolText.text = "Press E to rescue the animals Press P to continue";
            S.enabled = true;
            S.sprite = E;
        }

        if (other.CompareTag("Fire sign"))
        {
            sign = other.GetComponent<Pause>();
            sign.stop = true;

            toolText.enabled = true;
            toolText.text = "Get water by pressing E in a pond and lower the heat meter to avoid dying from overheating Press P to continue";
            S.enabled = true;
            S.sprite = E;
        }

        if (other.CompareTag("WinSign"))
        {
            sign = other.GetComponent<Pause>();
            sign.stop = true;

            toolText.enabled = true;
            toolText.text = "If you have found all 3 animals you can find the last Zipline to escape the burning forest Press P to continue";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        toolText.enabled = false;
        A.enabled = false;
        D.enabled = false;
        W.enabled = false;
        S.enabled = false;
        Space.enabled = false;
        Destroy(sign);
    }
}
