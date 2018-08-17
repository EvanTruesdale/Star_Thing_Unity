using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public GameObject BodiesManager;
    private BodyInitialzation Constants;

	void Start () {
        Constants = BodiesManager.GetComponent<BodyInitialzation>();
	}
	
	void FixedUpdate () {
        gameObject.transform.Rotate(0, Time.fixedDeltaTime / Constants.GetRotationPeriod(gameObject.name)*3600, 0);
	}
}
