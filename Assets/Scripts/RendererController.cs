using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class RendererController : MonoBehaviour {

	void Update () {
        foreach(GameObject Body in PhysicsCalculation.GetBodies(true))
        {
            try
            {
                Transform bodyTransform = Body.GetComponent<Transform>();
                if(bodyTransform.localScale.x / bodyTransform.position.magnitude < 0.003f)
                {
                    Body.GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    Body.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            catch(MissingComponentException)
            {
                
            }
            catch(NullReferenceException)
            {
                
            }
        }
	}
}
