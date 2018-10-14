using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class ReticleController : MonoBehaviour {

	//Activate Reticle
    public void ActivateReticle(){
        Debug.Log("Activate");
        GameObject.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = true;
    }

    //Deactivate Reticle
    public void DeactivateReticle(){
        Debug.Log("Deactivate");
        GameObject.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        GameObject.Find("GvrReticlePointer").GetComponent<GvrReticlePointer>().reticleGrowthSpeed = 20 / MenuController.GetSpeed();
    }
}
