using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PhysicsCalculation : MonoBehaviour
    {

        //This is the Main collection of Bodies
        static List<GameObject> Bodies = new List<GameObject>();
        //Constants
        public static float G = 6.67e-11f;
        public static float distanceScalar = 1e7f;
        public static float massScalar = 1e24f;
        public static float velocityScalar = Mathf.Sqrt(distanceScalar);
        public static float radiusScalar = 1e10f;

        public static float trueDistanceScalar = 1e11f;
        public static float trueMassScalar = 1e24f;
        public static float trueVelocityScalar = Mathf.Sqrt(trueDistanceScalar);

        private void Start()
        {
            //Add Sun to Bodies List (doesn't come from data sheet)
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
                        //Get real positions of planets
                        Vector3 position1 = Body1.transform.position * distanceScalar;
                        Vector3 position2 = Body2.transform.position * distanceScalar;
                        //Get real masses of planets
                        float mass1 = Body1.GetComponent<Rigidbody>().mass * massScalar;
                        float mass2 = Body2.GetComponent<Rigidbody>().mass * massScalar;

                        //Calculate difference Vector
                        Vector3 difference = position1 - position2;
                        //Calculate Magnitude
                        float magnitude = G * mass1 * mass2 / Mathf.Pow(difference.magnitude, 2);

                        if (magnitude > 1e10f)
                        {
                            //Scale Force back down for Rigidbody
                            difference = difference.normalized;
                            difference *= magnitude / massScalar;

                            //Add Force to Rigidbody
                            Body2.GetComponent<Rigidbody>().AddForce(difference);
                        }
                    }
                }
            }
        }

        //Function to Read Bodies, with and without Sun
        public static List<GameObject> GetBodies(bool includeSun = false)
        {
            List<GameObject> ReturnBodies = new List<GameObject>(Bodies);
            if (!includeSun)
            {
                ReturnBodies.Remove(GameObject.FindGameObjectWithTag("Sun"));
            }
            return ReturnBodies;
        }

        //Add Body function
        public void AddBody(GameObject Body)
        {
            Bodies.Add(Body);
        }
    }
}

