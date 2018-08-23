using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PhysicsCalculation : MonoBehaviour
    {

        static List<GameObject> Bodies = new List<GameObject>();
        public float G;
        public float distanceScalar;
        public float massScalar;
        public float velocityScalar;
        public float radiusScalar;

        private void Start()
        {
            Bodies.Add(GameObject.FindGameObjectWithTag("Sun"));
        }

        void FixedUpdate()
        {
            foreach (GameObject Body1 in Bodies)
            {
                foreach (GameObject Body2 in Bodies)
                {
                    if (Body1 != Body2)
                    {

                        Vector3 position1 = Body1.transform.position * distanceScalar;
                        Vector3 position2 = Body2.transform.position * distanceScalar;
                        float mass1 = Body1.GetComponent<Rigidbody>().mass * massScalar;
                        float mass2 = Body2.GetComponent<Rigidbody>().mass * massScalar;

                        Vector3 difference = position1 - position2;

                        float magnitude = G * mass1 * mass2 / Mathf.Pow(difference.magnitude, 2);

                        difference = difference.normalized;
                        difference *= magnitude / massScalar;

                        Body2.GetComponent<Rigidbody>().AddForce(difference);
                    }
                }
            }
        }

        public static List<GameObject> GetBodies(bool includeSun = false)
        {
            List<GameObject> ReturnBodies = new List<GameObject>(Bodies);
            if (!includeSun)
            {
                ReturnBodies.Remove(GameObject.FindGameObjectWithTag("Sun"));
            }
            return ReturnBodies;
        }

        public void AddBody(GameObject Body)
        {
            Bodies.Add(Body);
        }
    }
}

