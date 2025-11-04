using UnityEngine;

namespace Scenes.Jonas
{
    public class FoxController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpForce = 10f;
        private bool isJumping = false;

        private Rigidbody2D rb;
        private Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            // Handle movement
            float horizontal = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

            // Set the "Speed" parameter for animations
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

            // Handle jumping
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                isJumping = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                animator.SetBool("IsJumping", true);
            }

            // Handle landing
            if (rb.linearVelocity.y == 0)
            {
                isJumping = false;
                animator.SetBool("IsJumping", false);
            }
        }
    }
}
