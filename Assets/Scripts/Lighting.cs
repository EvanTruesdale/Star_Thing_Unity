using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour {

    private Transform SunTransform;
    private Transform BodyTransform;

	// Use this for initialization
	void Start () {
        SunTransform = GameObject.FindGameObjectWithTag("Sun").transform;
        BodyTransform = gameObject.transform.parent;
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
