using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Rotation : MonoBehaviour
{

    void FixedUpdate()
    {
        //Rotate planet around y axis, which is tilted with respect to world space
        gameObject.transform.Rotate(0, PhysicsCalculation.velocityScalar * (-1*Mathf.PI * Time.fixedDeltaTime / (BodyInitialzation.GetRotationPeriod(gameObject.name) * 1)), 0);
    }
}