using System;
using Mathias_Script;
using UnityEngine;
using System.Collections;

namespace Scenes.Jonas
{
    public class Follower : MonoBehaviour
    {
        public Playermovement Player;
        public Transform respawn;
        
        [Header("Following")]
        public Transform targetToFollow;
        public float followSpeed = 8f;
        public float followDelay = 0.2f;
        private bool isFollowing = false;
        private int pathIndexOffset;

        [Header("Interaction")]
        public string playerTag = "Player";


        [Header("Animation")]
        private Animator myAnimator;
        private Animator foxAnimator;

        public Rigidbody2D rb;
        public Heat Heat;
<<<<<<< Updated upstream
=======
        
    
>>>>>>> Stashed changes


        void Start()
        {
            myAnimator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (Heat.isDead)
            {
                isFollowing = false;
                transform.position = respawn.position;
            }

            if (isFollowing)
            {
                FollowPath();
                SyncAnimations(); 
            }
        }

        // ===============================
        // FOLLOWING THE FOX
        // ===============================
        [SerializeField] private float stopDistance = 0.5f; // Prevent clumping

        void FollowPath()
        {
            if (!isFollowing || PathRecorder.Instance == null) return;

            var path = PathRecorder.Instance.recordedPath;
            int index = Mathf.Clamp(path.Count - pathIndexOffset - 1, 0, path.Count - 1);
            if (index < 0 || index >= path.Count) return;

            if (path.Count == 0 || index < 0) return;

            Vector2 targetPos = path[index];
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
            float distance = Vector2.Distance(transform.position, targetPos);

            if (distance > stopDistance)
            {
                rb.linearVelocity = direction * followSpeed;
            }
            else
            {
                // Smooth vertical drop if in air and not aligned with player's Y
                float yDiff = transform.position.y - Player.transform.position.y;

                if (Mathf.Abs(yDiff) > 0.05f)
                {
                    // Move vertically down/up to match player Y (if needed)
                    float verticalSpeed = Mathf.Sign(-yDiff) * followSpeed * 0.5f; // slower than regular follow speed
                    rb.linearVelocity = new Vector2(0, verticalSpeed);
                }
                else
                {
                    // Aligned — stop moving
                    rb.linearVelocity = Vector2.zero;
                }
            }

            // Flip
            if (Player.transform.localScale.x == 1)
                transform.localScale = Vector3.one;
            else if (Player.transform.localScale.x == -1)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        public void StartFollowing(Transform target, int orderInLine)
        {
            targetToFollow = target;
            isFollowing = true;

            // Stack delays: each follower adds delay based on order
            pathIndexOffset = Mathf.RoundToInt((followDelay * orderInLine) / PathRecorder.Instance.recordInterval);
        }
<<<<<<< Updated upstream
=======
        
>>>>>>> Stashed changes

        // ===============================
        // INTERACTION
        // ===============================
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && Player.pressE)
            {
                // Add to FollowerManager
                FollowerManager.Instance.AddFollower(gameObject);

                // Start animation syncing
                foxAnimator = other.GetComponent<Animator>();
                //StartFollowing(3);
            }
        }

        // ===============================
        // ANIMATION SYNC
        // ===============================
        void SyncAnimations()
        {
            if (foxAnimator == null) return;

            myAnimator.SetBool("IsMoving", foxAnimator.GetBool("IsMoving"));
            myAnimator.SetBool("IsGround", foxAnimator.GetBool("IsGround"));
            //myAnimator.SetBool("Falling", foxAnimator.GetBool("Falling"));

            //SyncTrigger("Jump");
            //SyncTrigger("Land");
            //SyncTrigger("TakingDamage");
            //SyncTrigger("Faint");
        }

        void SyncTrigger(string triggerName)
        {
            if (foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(triggerName))
            {
                myAnimator.SetTrigger(triggerName);
            }
        }
    }
}


