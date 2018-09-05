using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTexture : MonoBehaviour
{

    void Start()
    {
        //Standard Textures
        string texturePath = gameObject.name + "_Texture";
        GetComponent<Renderer>().material.SetTexture("_MainTex", (Texture)Resources.Load(texturePath));

        //Planetary Add-Ons
        if (gameObject.name == "Venus")
        {
            texturePath = gameObject.name + "_Atmosphere_Texture";
            GetComponent<Renderer>().material.SetTexture("_DETAIL_MULX2", (Texture)Resources.Load(texturePath));
        }
        if (gameObject.name == "Tesla Roadster")
        {
            GameObject child = Instantiate(Resources.Load<GameObject>("Tesla_Roadster_Model"));
            child.transform.parent = gameObject.transform;
            child.transform.parent.gameObject.GetComponent<Renderer>().enabled = false;
            child.transform.localPosition = new Vector3(0, 0, -.5f);
            child.transform.localRotation = new Quaternion();
            child.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
