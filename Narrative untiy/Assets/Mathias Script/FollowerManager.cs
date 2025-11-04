using System;
using System.Collections.Generic;
using Scenes.Jonas;
using UnityEngine;

namespace Mathias_Script
{
    public class FollowerManager : MonoBehaviour
    {
        public static FollowerManager Instance;
        public GameObject player;

        private List<GameObject> followers = new List<GameObject>();

        public Heat Heat;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
<<<<<<< Updated upstream
        }

        public void AddFollower(GameObject followerObj)
        {
            if (followers.Contains(followerObj)) return; // Avoid duplicates

            Follower follower = followerObj.GetComponent<Follower>();

            if (follower != null)
            {
                Transform followTarget = followers.Count == 0
                    ? player.transform
                    : followers[followers.Count - 1].transform;

                follower.StartFollowing(followTarget, followers.Count + 1);
                followers.Add(followerObj);
            }
        }

        private void Update()
        {
            if (Heat.isDead)
            {
                removeFollowers();
            }
        }

        private void removeFollowers()
        {
            foreach (var follower in followers)
            {
                Destroy(follower);
            }
            followers.Clear();
=======
>>>>>>> Stashed changes
        }

        public void AddFollower(GameObject followerObj)
        {
            if (followers.Contains(followerObj)) return; // Avoid duplicates

            Follower follower = followerObj.GetComponent<Follower>();

            if (follower != null)
            {
                Transform followTarget = followers.Count == 0
                    ? player.transform
                    : followers[followers.Count - 1].transform;

                follower.StartFollowing(followTarget, followers.Count + 1);
                followers.Add(followerObj);
            }
        }

        private void Update()
        {
            if (Heat.isDead)
            {
                //removeFollowers();
            }
        }

        /*private void removeFollowers()
        {
            Heat.isDead = false;

            // Reset follower path
            if (PathRecorder.Instance != null)
                PathRecorder.Instance.recordedPath.Clear();

            // Optional: Notify followers manually if needed
            foreach (var follower in followers)
            {
                followers.Clear();
            }
            
        }*/
        
        
    }
}
