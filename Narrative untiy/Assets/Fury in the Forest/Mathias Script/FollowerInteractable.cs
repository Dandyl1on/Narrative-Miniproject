using Scenes.Jonas;
using UnityEngine;

namespace Mathias_Script
{
    public class FollowerInteractable : MonoBehaviour
    {
        public float interactRange = 2f;
        private bool playerInRange = false;
        private bool isFollowing = false;
        private Follower follower;
        public GameObject player;

        void Start()
        {
            follower = GetComponent<Follower>(); // Get Follower script
        }

        void Update()
        {
            if (isFollowing) return;

            if (player == null) return;

            float dist = Vector2.Distance(player.transform.position, transform.position);
            playerInRange = dist < interactRange;

        
            if (playerInRange && Input.GetKeyDown(KeyCode.F))
            {
                isFollowing = true;

                if (follower != null)
                {
                    //follower.StartFollowing(); // ✅ Triggers hop first, then follows
                    FollowerManager.Instance.AddFollower(this.gameObject);
                }
            }
        }
    }
}
