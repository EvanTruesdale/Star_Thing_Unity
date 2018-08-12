using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    private GameObject[] Bodies;
    public float G;
    public float distanceScalar;
    public float massScalar;

    private void Start()
    {
        Bodies = GameObject.FindGameObjectsWithTag("Body");
    }

    void FixedUpdate(){
        foreach (GameObject Body1 in Bodies){
            foreach (GameObject Body2 in Bodies){
                if (Body1 != Body2){
                    Vector3 pos1 = Body1.transform.position;
                    Vector3 pos2 = Body2.transform.position;
                    float mass1 = Body1.GetComponent<Rigidbody>().mass * massScalar;
                    float mass2 = Body2.GetComponent<Rigidbody>().mass * massScalar;

                    Vector3 difference = pos1 - pos2;
                    float mag = G*mass1*mass2 / Mathf.Pow(difference.magnitude * distanceScalar, 2);

                    difference = difference.normalized;
                    difference.Scale(new Vector3(mag / massScalar, mag / massScalar, mag / massScalar));

                    Body1.GetComponent<Rigidbody>().AddForce(difference);
                }
            }
        }
    }
}
