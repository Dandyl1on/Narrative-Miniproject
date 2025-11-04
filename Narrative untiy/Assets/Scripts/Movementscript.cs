using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Playermovement : MonoBehaviour
{   
    //the horizontal float is for movement either 1 for right or -1 for left and 0 for no movement
    public float horizontalInput;
    public float speed = 8f;
    public float jumpPower;
    public bool grounded;
    public bool falls;
    
    public float delayJump = 0.2f;
    public bool doubleJump;
    
    public float idle2Chance = 0.2f;
    public float checkInterval = 1f;

    public Animator Animation;

    public GameObject ziplineText;

    public bool lookDown;
    public bool lookUp;

    public AudioClip[] jumpSound;
    private AudioSource Audio;

    //below all are used for basic player stuff
    [SerializeField] private Rigidbody2D rb;

    public bool pressE;



    private void Start()
    {
        Animation = GetComponent<Animator>();
        InvokeRepeating(nameof(CheckForIdle2), checkInterval, checkInterval);
        ziplineText.SetActive(false);
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
        
        Animation.SetBool("IsMoving", horizontalInput != 0);
        Animation.SetBool("IsGround", grounded);
        falls = rb.linearVelocity.y < -0.1f && !grounded;
        Animation.SetBool("Falling", falls);
        
        
        if (grounded)
        {
            doubleJump = false;
        }
        else
        {
            delayJump += Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !falls)
        {
            Jump();
            doubleJump = false;
        }

        // delayed jump off cliff
        if (Input.GetKeyDown(KeyCode.Space) && delayJump < 0.2f && falls)
        {
            Jump();
        }

        // double jump
        if (Input.GetKeyDown(KeyCode.Space) && !doubleJump && delayJump > 0.2f)
        {
            Jump();
            doubleJump = true;

        }
        //Rotates the sprite correctly
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //looks down on key press
        if (Input.GetKeyDown(KeyCode.S))
        {
            lookDown = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            lookDown = false;
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            lookUp = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            lookUp = false;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressE = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            pressE = false;
        }
        
    }

    void Jump()
    {
        Animation.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        int index = UnityEngine.Random.Range(0, jumpSound.Length);
        AudioClip jump = jumpSound[index];
        Audio.clip = jump;
        Audio.Play();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            falls = false;
            delayJump = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Zipline"))
        {
            ziplineText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zipline"))
        {
            ziplineText.SetActive(false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
    

    void CheckForIdle2()
    {
        if (Random.value < idle2Chance)
        {
            Animation.SetTrigger("Idle2");
        }
    }
}
