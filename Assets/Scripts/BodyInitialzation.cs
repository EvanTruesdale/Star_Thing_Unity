using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts
{
    public class BodyInitialzation : MonoBehaviour
    {

        public GameObject BodyPrefab;
        private List<GameObject> Bodies = new List<GameObject>();
        private GameObject Sun;
        private PhysicsCalculation Constants;

        //Values read from the TXT file
        static List<string> names = new List<string>();
        static List<float> scaledMasses = new List<float>();
        static List<float> scaledDistances = new List<float>();   //using semi-major axis for distance, reference ReadCSV function for details
        static List<float> scaledVelocities = new List<float>();
        static List<float> rotationPeriods = new List<float>();
        static List<float> orbitalInclinations = new List<float>();
        static List<float> orbitalEccentricities = new List<float>();
        static List<float> orbitObliquities = new List<float>();
        static List<float> scaledRadii = new List<float>();

        //Access methods for TXT file data
        public static float GetScaledMass(string name)
        {
            int index = names.IndexOf(name);
            return scaledMasses[index];
        }

        public static float GetScaledDistance(string name)
        {
            int index = names.IndexOf(name);
            return scaledDistances[index];
        }

        public static float GetScaledVelocity(string name)
        {
            int index = names.IndexOf(name);
            return scaledVelocities[index];
        }

        public static float GetRotationPeriod(string name)
        {
            int index = names.IndexOf(name);
            return rotationPeriods[index];
        }

        public static float GetOrbitalInclination(string name)
        {
            int index = names.IndexOf(name);
            return orbitalInclinations[index];
        }

        public static float GetOrbitalEccentricity(string name)
        {
            int index = names.IndexOf(name);
            return orbitalEccentricities[index];
        }

        public static float GetOrbitObliquity(string name)
        {
            int index = names.IndexOf(name);
            return orbitObliquities[index];
        }

        public static float GetScaledRadius(string name)
        {
            int index = names.IndexOf(name);
            return scaledRadii[index];
        }

        float velocityScalar;
        float distanceScalar;
        float massScalar;

        void Start()
        {

            //Find the Sun for velocity calculations
            Sun = GameObject.FindGameObjectWithTag("Sun");

            //Read data file in Assets
            ReadCSV();

            //Read constants from Physics Script
            Constants = gameObject.GetComponent<PhysicsCalculation>();
            velocityScalar = Constants.velocityScalar;
            distanceScalar = Constants.distanceScalar;
            massScalar = Constants.massScalar;

            //Create objects with given values
            for (int i = 0; i < names.Count; i++)
            {
                //Create object, with Position


                //START OBJECTS AT APHELION
                //START OBJECTS AT APHELION
                //START OBJECTS AT APHELION


                GameObject holderObject = Instantiate(BodyPrefab,
                                                  new Vector3(0,
                                                              scaledDistances[i] * (1 + orbitalEccentricities[i]) * Mathf.Sin(Mathf.Deg2Rad * orbitalInclinations[i]),
                                                              scaledDistances[i] * (1 + orbitalEccentricities[i]) * Mathf.Cos(Mathf.Deg2Rad * orbitalInclinations[i])),
                                                  new Quaternion());
                //Set rotation
                holderObject.transform.Rotate(new Vector3(1, 0, 0), orbitObliquities[i]);
                //Set mass,
                //scaled b/c Rigidbody can't handle the size, 
                //masses are scaled up for calculation then force is scaled back down
                holderObject.GetComponent<Rigidbody>().mass = scaledMasses[i];
                //Set velocity, calculated specifically for APHELION by program
                float velocityAdjustment = (2 / ((scaledDistances[i] * distanceScalar) * (1 + orbitalEccentricities[i]))) - 1 / (scaledDistances[i] * distanceScalar);
                holderObject.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sqrt(Constants.G * Sun.GetComponent<Rigidbody>().mass * massScalar * velocityAdjustment) / velocityScalar,
                                                                          0, 0);

                //Set scale
                holderObject.transform.localScale = new Vector3(scaledRadii[i], scaledRadii[i], scaledRadii[i]);

                //Set name
                holderObject.name = names[i];

                //Add body to PhysicsCalculation Bodies list
                Constants.AddBody(holderObject);
            }

        }

        void ReadCSV()
        {
            //Data from Planetary Fact Sheet https://nssdc.gsfc.nasa.gov/planetary/factsheet/
            //Tesla Data from HORIZONS Web Interface https://ssd.jpl.nasa.gov/horizons.cgi#results

            //Load text file
            TextAsset file = Resources.Load<TextAsset>("Body_Data");

            //Split text into lines
            string[] lines = file.text.Split('\n');
            foreach (string line in lines)
            {

                //Split lines into values separated by commas
                string[] values = line.Split(',');

                //Add values to respective lists for body initialization
                //NAME, MASS, RADIUS, ROTATION PERIOD, SEMI-MAJOR AXIS, ORBITAL INCLINATION, ORBITAL ECCENTRICITY, OBLIQUITY
                names.Add(values[0]);
                scaledMasses.Add(float.Parse(values[1]));
                scaledRadii.Add(float.Parse(values[2]));
                rotationPeriods.Add(float.Parse(values[3]));
                scaledDistances.Add(float.Parse(values[4]));
                orbitalInclinations.Add(float.Parse(values[5]));
                orbitalEccentricities.Add(float.Parse(values[6]));
                orbitObliquities.Add(float.Parse(values[7]));

            }
        }

    }

}

