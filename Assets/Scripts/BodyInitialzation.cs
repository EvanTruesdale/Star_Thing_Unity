using System;
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
        static List<string> centralBodies = new List<string>();
        static List<float> longitudeOfAscendingNodes = new List<float>();

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
            try
            {
                return orbitObliquities[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                return 0;
            }
        }

        public static float GetScaledRadius(string name)
        {
            int index = names.IndexOf(name);
            return scaledRadii[index];
        }

        public static string GetCentralBody(string name)
        {
            int index = names.IndexOf(name);
            return centralBodies[index];
        }

        public static float GetLongitudeOfAscendingNodes(string name)
        {
            int index = names.IndexOf(name);
            return longitudeOfAscendingNodes[index];
        }

        float velocityScalar;
        float distanceScalar;
        float massScalar;

        int frame = 0;

        void Start()
        {

            //Read data file in Assets
            ReadCSV();

            //Read constants from Physics Script
            Constants = gameObject.GetComponent<PhysicsCalculation>();
            velocityScalar = PhysicsCalculation.velocityScalar;
            distanceScalar = PhysicsCalculation.distanceScalar;
            massScalar = PhysicsCalculation.massScalar;

            //Create objects with given values
            for (int i = 0; i < names.Count; i++)
            {
                //If the central body is the Sun, load them first, because these will be central bodies for the moons
                if(centralBodies[i] == "Sun"){
                    LoadBody(i, "Sun");
                }
            }

            frame = 1;
        }

        void Update()
        {
            if(frame == 1){
                frame = 2;
                for (int i = 0; i < names.Count; i++)
                {
                    //Load secondary bodies now
                    if(centralBodies[i] != "Sun"){
                        LoadBody(i, centralBodies[i]);
                    }
                }

            }
        }

        void LoadBody(int i, string center){
            //Create object, with Position

            //START OBJECTS AT APHELION
            //START OBJECTS AT APHELION
            //START OBJECTS AT APHELION

            GameObject CenterBody = GameObject.Find(center);


            GameObject holderObject = Instantiate(BodyPrefab,
                                                  new Vector3(scaledDistances[i] * (1 + orbitalEccentricities[i]) * Mathf.Cos(Mathf.Deg2Rad * longitudeOfAscendingNodes[i]),
                                                              scaledDistances[i] * (1 + orbitalEccentricities[i]) * Mathf.Sin(Mathf.Deg2Rad * (orbitalInclinations[i] - GetOrbitObliquity(center))),
                                                              scaledDistances[i] * (1 + orbitalEccentricities[i]) * Mathf.Sin(Mathf.Deg2Rad * longitudeOfAscendingNodes[i]) * Mathf.Cos(Mathf.Deg2Rad * (orbitalInclinations[i] - GetOrbitObliquity(center)))),
                                                  new Quaternion());

            //Set rotation
            holderObject.transform.Rotate(new Vector3(1, 0, 0), orbitObliquities[i]);

            //Set mass,
            //scaled b/c Rigidbody can't handle the size, 
            //masses are scaled up for calculation then force is scaled back down
            holderObject.GetComponent<Rigidbody>().mass = scaledMasses[i];

            //Set velocity, calculated specifically for APHELION by program
            float velocityAdjustment = (2 / ((scaledDistances[i] * distanceScalar) * (1 + orbitalEccentricities[i]))) - 1 / (scaledDistances[i] * distanceScalar);
            holderObject.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sin(Mathf.Deg2Rad * longitudeOfAscendingNodes[i]) * Mathf.Sqrt(PhysicsCalculation.G * CenterBody.GetComponent<Rigidbody>().mass * massScalar * velocityAdjustment) / velocityScalar,
                                                                          0,
                                                                          Mathf.Cos(Mathf.Deg2Rad * longitudeOfAscendingNodes[i]) * Mathf.Sqrt(PhysicsCalculation.G * CenterBody.GetComponent<Rigidbody>().mass * massScalar * velocityAdjustment) / velocityScalar);
            holderObject.GetComponent<Rigidbody>().velocity += CenterBody.GetComponent<Rigidbody>().velocity;

            //Set scale
            holderObject.transform.localScale = new Vector3(scaledRadii[i], scaledRadii[i], scaledRadii[i]);

            //Set name
            holderObject.name = names[i];

            //Add body to PhysicsCalculation Bodies list
            Constants.AddBody(holderObject);

            //Move Body to orbit it's center
            holderObject.transform.position += CenterBody.GetComponent<Transform>().position;
        }

        void ReadCSV()
        {
            //Data from Planetary Fact Sheet https://nssdc.gsfc.nasa.gov/planetary/planetfact.html
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

                //NAME, MASS,    RADIUS, ROTATION PERIOD, SEMI-MAJOR AXIS, ORBITAL INCLINATION, ORBITAL ECCENTRICITY, OBLIQUITY, CENTRAL BODY, LONGITUDE OF ASCENDING NODE
                //      10^24kg  10^8m   1hours           10^8m            1degrees             (dimensionless)       1degrees                 1degrees, relative, absolute doesn't matter
                names.Add(values[0].Trim());
                scaledMasses.Add(float.Parse(values[1]));
                scaledRadii.Add(float.Parse(values[2]));
                rotationPeriods.Add(float.Parse(values[3]));
                scaledDistances.Add(float.Parse(values[4]));
                orbitalInclinations.Add(float.Parse(values[5]));
                orbitalEccentricities.Add(float.Parse(values[6]));
                orbitObliquities.Add(float.Parse(values[7]));
                centralBodies.Add(values[8].Trim());
                try
                {
                    longitudeOfAscendingNodes.Add(float.Parse(values[9]));
                }
                catch (IndexOutOfRangeException)
                {
                    longitudeOfAscendingNodes.Add(0);
                }
            }
        }
    }
}

