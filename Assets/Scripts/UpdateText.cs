using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class UpdateText : MonoBehaviour {
    
	void Update () {
        ChangeText();
	}

    public void ChangeText(){
        string text = "";
        if(gameObject.name == "Speed Text"){
            text = "Speed: " + MenuController.GetSpeed();
        } else if (gameObject.name == "Scale Text"){
            //string text = "Scale: " + MenuController.GetSpeed();
        }

        gameObject.GetComponent<Text>().text = text;
    }
}
