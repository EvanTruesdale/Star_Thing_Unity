using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class BodyInitialzation : MonoBehaviour {
    
    //Use data from Planetary Fact Sheet
    public GameObject BodyPrefab;
    private PhysicsCalculation Constants;
    private List<string> names = new List<string>();
    private List<float> scaledMasses = new List<float>();

    //using average distance and velocity data
    private List<float> scaledDistances = new List<float>();
    private List<float> scaledVelocities = new List<float>();
    private List<float> rotationPeriods = new List<float>();
    private List<float> orbitalInclinations = new List<float>();
    private List<float> orbitObliquities = new List<float>();
    private List<float> scaledRadii = new List<float>();

    float velocityScalar;

	// Use this for initialization
	void Start () {

        //Read data file in Assets
        ReadCSV2();

        //Read constants from Physics Script
        Constants = gameObject.GetComponent<PhysicsCalculation>();
        velocityScalar = Constants.velocityScalar;

        //Create objects with given values
        for (int i = 0; i < names.Count; i++){
            //Create object, with Position
            GameObject holderObject = Instantiate(BodyPrefab,
                                                  new Vector3(0,
                                                              scaledDistances[i] * Mathf.Sin(Mathf.Deg2Rad * orbitalInclinations[i]),
                                                              scaledDistances[i] * Mathf.Cos(Mathf.Deg2Rad * orbitalInclinations[i])  ),
                                                  new Quaternion());
            //Set rotation
            holderObject.transform.Rotate(new Vector3(1, 0, 0), orbitObliquities[i]);
            //Set mass
            holderObject.GetComponent<Rigidbody>().mass = scaledMasses[i];
            //Set velocity
            holderObject.GetComponent<Rigidbody>().velocity = new Vector3(scaledVelocities[i]/velocityScalar, 0, 0);

            //Set scale
            //holderObject.transform.localScale = new Vector3(scaledRadii[i], scaledRadii[i], scaledRadii[i]);
            holderObject.transform.localScale = new Vector3(1, 1, 1);

            //Set name
            holderObject.name = names[i];

            Constants.Bodies.Add(holderObject);
        }

	}

    void ReadCSV2(){
        //Load text file
        TextAsset file = Resources.Load<TextAsset>("Body_Data");

        //Split text into lines
        string[] lines = file.text.Split('\n');
        foreach (string line in lines){

            //Split lines into values separated by commas
            string[] values = line.Split(',');

            //Add values to respective lists for body initialization
            names.Add(values[0]);
            scaledMasses.Add(float.Parse(values[1]));
            scaledDistances.Add(float.Parse(values[2]));
            scaledVelocities.Add(float.Parse(values[3]));
            rotationPeriods.Add(float.Parse(values[4]));
            orbitalInclinations.Add(float.Parse(values[5]));
            orbitObliquities.Add(float.Parse(values[6]));
            scaledRadii.Add(float.Parse(values[7]));
        }
    }

    public float GetRotationPeriod(string name){
        //Search name list to identify body
        int index = names.IndexOf(name);

        //return rotation period for correct body
        return rotationPeriods[index];
    }
}
