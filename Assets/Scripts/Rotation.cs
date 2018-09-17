using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Rotation : MonoBehaviour
{

    private GameObject BodyManager;

    void Start()
    {
        //Find BodyManager
        BodyManager = GameObject.Find("BodyManager");
    }

    void FixedUpdate()
    {
        //Rotate planet around y axis, which is tilted with respect to world space
        gameObject.transform.Rotate(0, -Mathf.PI * Time.fixedDeltaTime / BodyInitialzation.GetRotationPeriod(gameObject.name) * 3600, 0);
    }
}
