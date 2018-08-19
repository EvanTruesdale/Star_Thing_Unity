using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    private GameObject BodyManager;
    private BodyInitialzation Constants;

	void Start () {
        BodyManager = GameObject.Find("BodyManager");
        Constants = BodyManager.GetComponent<BodyInitialzation>();
	}
	
	void FixedUpdate () {
        //rotate planet around y axis, which is tilted with respect to world space
        gameObject.transform.Rotate(0, -1 * Time.fixedDeltaTime / Constants.GetRotationPeriod(gameObject.name)*3600, 0);
	}
}
