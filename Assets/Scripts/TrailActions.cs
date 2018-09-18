using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailActions : MonoBehaviour {
	
    //Change trail time based on Speed (UPDATE)
	void Update () {
        GetComponent<TrailRenderer>().time = 5f / Mathf.Pow(GetComponentInParent<Rigidbody>().velocity.magnitude, 3f);
	}
}
