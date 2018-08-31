using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailActions : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        GetComponent<TrailRenderer>().time = 15f / GetComponentInParent<Rigidbody>().velocity.magnitude;
	}
}
