using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelActions : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<TextMesh>().text = GetComponent<Transform>().parent.name;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 PS = GetComponent<Transform>().parent.localScale;
        GetComponent<Transform>().localScale = new Vector3(.3f/PS.x, .3f/PS.y, .3f/PS.z);

        GameObject Camera = GameObject.Find("Main Camera");
        Vector3 lookAt = (2 * GetComponent<Transform>().position - Camera.GetComponent<Transform>().position);
        GetComponent<Transform>().LookAt(lookAt);
	}
}
