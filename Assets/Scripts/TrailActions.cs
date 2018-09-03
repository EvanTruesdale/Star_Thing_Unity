using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailActions : MonoBehaviour {
	
    //Change trail time based on Speed (UPDATE)
	void Update () {
        GetComponent<TrailRenderer>().time = 15f / GetComponentInParent<Rigidbody>().velocity.magnitude;
	}
}
