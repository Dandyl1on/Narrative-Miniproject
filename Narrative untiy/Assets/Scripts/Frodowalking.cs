using UnityEngine;

public class Frodowalking : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float horizontalInput;
    
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower;
    [SerializeField] private bool grounded;
    [SerializeField] private bool falls;
    [SerializeField] private float delayJump = 0.2f;

    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
        animator.SetBool("isMoving", horizontalInput != 0);
        falls = rb.linearVelocity.y < -0.1f && !grounded;

        if (!grounded)
        {
            delayJump += Time.deltaTime;
        }
        // normal jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !falls)
        {
            Jump();
        }
        // delayed jump
        if (Input.GetKeyDown(KeyCode.Space) && delayJump < 0.2f && falls)
        {
            Jump();
        }
        
        //flips the sprite
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(0.5900847f, 0.5900847f, 0.5900847f);
        }
        else if (horizontalInput < -0.01)
        {
            transform.localScale = new Vector3(-0.5900847f, 0.5900847f, 0.5900847f);
        }
    }
    
    void Jump()
    {
        Debug.Log("Jump in function");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
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
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}
