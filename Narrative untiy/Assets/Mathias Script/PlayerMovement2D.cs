using UnityEngine;

namespace Mathias_Script
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement2D : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpForce = 10f;
        public Transform groundCheck;
        public LayerMask groundLayer;

        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private bool isGrounded;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            // Move the player
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            // Flip sprite
            if (moveInput != 0)
                if (moveInput != 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Sign(moveInput) * Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }


            // Check for jump
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

            if (Input.GetButtonDown("Jump") && isGrounded)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
