using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInitialzation : MonoBehaviour {

    //use data from Planetary Fact Sheet
    public GameObject Bodies;
    private Gravity Constants;
    public float scaledMass;
    //using average distance and velocity data
    public float scaledDistance;
    public float scaledVelocity;
    public float rotationPeriod;
    public float orbitalInclination;
    public float orbitObliquity;
    public float scaledRadius;

    float distanceScalar;
    float massScalar;
    float velocityScalar;
    float radiusScalar;

	// Use this for initialization
	void Start () {
        Constants = Bodies.GetComponent<Gravity>();
        distanceScalar = Constants.distanceScalar;
        massScalar = Constants.massScalar;
        velocityScalar = Constants.velocityScalar;
        radiusScalar = Constants.radiusScalar;

        gameObject.GetComponent<Rigidbody>().mass = scaledMass;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(scaledVelocity/velocityScalar, 0, 0);
        gameObject.transform.position = new Vector3(0,
                                                    scaledDistance * Mathf.Sin(Mathf.Deg2Rad * orbitalInclination),
                                                    scaledDistance * Mathf.Cos(Mathf.Deg2Rad * orbitalInclination));
        gameObject.transform.localScale = new Vector3(1, 1, 1);
	}
}
