using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour {

    public GameObject Sun;
    private Transform SunTransform;
    public GameObject Body;
    private Transform BodyTransform;

	// Use this for initialization
	void Start () {
        SunTransform = Sun.transform;
        BodyTransform = Body.transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 difference = BodyTransform.position - SunTransform.position;
        difference = difference.normalized;
        difference *= 3;
        gameObject.transform.position = BodyTransform.position - difference;
        gameObject.transform.LookAt(BodyTransform.position);
	}
}
