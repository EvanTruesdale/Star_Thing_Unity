using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailActions : MonoBehaviour {
	
    //Change trail time based on Speed (UPDATE)
	void Update () {
        GameObject Player = GameObject.Find("Player");
        GetComponent<TrailRenderer>().time = 500000f / Mathf.Pow(GetComponentInParent<Rigidbody>().velocity.magnitude, 3f);
        GetComponent<TrailRenderer>().widthMultiplier = Vector3.Distance(GetComponentInParent<Transform>().position, Player.GetComponent<Transform>().position) / 100; 
    }
}