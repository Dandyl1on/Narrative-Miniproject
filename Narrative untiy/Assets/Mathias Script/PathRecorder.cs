using System.Collections.Generic;
using UnityEngine;

namespace Mathias_Script
{
    public class PathRecorder : MonoBehaviour
    {
        public static PathRecorder Instance;

        //public float recordInterval = 0.02f;
        public int maxPoints = 50;

        //[HideInInspector] public List<Vector2> recordedPath = new List<Vector2>();

        //private float timer = 0f;
    
        public List<Vector2> recordedPath = new List<Vector2>();
        public float recordInterval = 0.1f;

        private float timer;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

    

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= recordInterval)
            {
                timer = 0f;
                recordedPath.Add(transform.position); // Must be player position
            }
            // Cap list size
            if (recordedPath.Count > maxPoints)
            {
                recordedPath.RemoveAt(0); // Remove oldest point
            }
        }
    }
}
