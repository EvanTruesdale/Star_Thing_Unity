using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour {

    private Transform SunTransform;
    private Transform BodyTransform;

	void Start () {
        //Get transforms of sun and body
        SunTransform = GameObject.FindGameObjectWithTag("Sun").transform;
        BodyTransform = gameObject.transform.parent;
	}
	
	void Update () {
        //get the Vector between sun and body
        Vector3 difference = BodyTransform.position - SunTransform.position;

        //set distance from body
        difference = difference.normalized;
        difference *= 3;

        //place light at new position and rotate to body
        gameObject.transform.position = BodyTransform.position - difference;
        gameObject.transform.LookAt(BodyTransform.position);
	}
}
