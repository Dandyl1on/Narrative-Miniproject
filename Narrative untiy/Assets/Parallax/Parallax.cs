using UnityEngine;

namespace Parallax
{
    public class Parallax : MonoBehaviour
    {

        private float length;
        public GameObject cam;
        public float parallaxEffect;

        public Vector2 startpos;

        void Start()
        {
            startpos = transform.position;
            length = GetComponent<SpriteRenderer>().bounds.size.x;


        }

        void FixedUpdate()
        {
            float temp = (cam.transform.position.x * (1 - parallaxEffect));
            float dist = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startpos.x + dist, startpos.y, transform.position.z);

            if (temp > startpos.x + length)
            {
                startpos.x += length;
            }
            else if (temp < startpos.x - length)
            {
                startpos.x -= length;
            }
        }
    }
}