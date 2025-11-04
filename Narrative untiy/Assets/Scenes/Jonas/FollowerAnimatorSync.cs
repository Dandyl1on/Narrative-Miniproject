using UnityEngine;

namespace Scenes.Jonas
{
    public class FollowerAnimatorSync : MonoBehaviour
    {
        public Transform foxTransform; // Assigned by FollowerManager
        private Animator foxAnimator;
        private Animator myAnimator;
        private bool isFollowing = false;

        void Start()
        {
            myAnimator = GetComponent<Animator>();
        }

        void Update()
        {
            if (!isFollowing || foxAnimator == null) return;

            // Copy Fox’s boolean states
            myAnimator.SetBool("IsMoving", foxAnimator.GetBool("IsMoving"));
            myAnimator.SetBool("IsGround", foxAnimator.GetBool("IsGround"));
            myAnimator.SetBool("Falling", foxAnimator.GetBool("Falling"));

            // Handle trigger copying (excluding Idle2)
            SyncTrigger("Jump");
            SyncTrigger("Land");
            SyncTrigger("TakingDamage");
            SyncTrigger("Faint");
        }

        void SyncTrigger(string triggerName)
        {
            if (foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(triggerName))
            {
                myAnimator.SetTrigger(triggerName);
            }
        }

        public void StartFollowing(Transform fox)
        {
            foxTransform = fox;
            foxAnimator = fox.GetComponent<Animator>();
            isFollowing = true;
        }
    }
}
