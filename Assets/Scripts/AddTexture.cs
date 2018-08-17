using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTexture : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string texturePath = gameObject.name + "_Texture";
        GetComponent<Renderer>().material.SetTexture("_MainTex", (Texture)Resources.Load(texturePath));

        if(gameObject.name == "Venus"){
            texturePath = gameObject.name + "_Atmosphere_Texture";
            GetComponent<Renderer>().material.SetTexture("_DETAIL_MULX2", (Texture)Resources.Load(texturePath));
        }
	}
}
