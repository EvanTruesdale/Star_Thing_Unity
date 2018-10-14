using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class MenuActions : MonoBehaviour
{

    //Close All Menus
    public void CloseMainMenu()
    {
        GameObject[] Menus = GameObject.FindGameObjectsWithTag("MainMenu");
        GameObject[] InfoMenus = GameObject.FindGameObjectsWithTag("InfoMenu");
        foreach (GameObject Menu in Menus)
        {
            Destroy(Menu);
        }
        foreach (GameObject Menu in InfoMenus)
        {
            Destroy(Menu);
        }
        MenuController.SetMenuState(false);
        GameObject.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = false;
    }

    //Update Speed
    public void ChangeSpeed()
    {
        float newSpeed = gameObject.GetComponent<Slider>().value;
        MenuController.SetSpeed(newSpeed);
        GameObject[] Sliders = GameObject.FindGameObjectsWithTag("SpeedSlider");
        foreach (GameObject Slider in Sliders)
        {
            Slider.GetComponent<Slider>().value = newSpeed;
        }

        GameObject[] Texts = GameObject.FindGameObjectsWithTag("SpeedText");
        foreach (GameObject Text in Texts)
        {
            Text.GetComponent<Text>().text = "Speed: " + newSpeed;
        }
    }

    //Update Scale
    public void ChangeScale()
    {
        float newScale = gameObject.GetComponent<Slider>().value;
        MenuController.SetScale(newScale);
        GameObject[] Sliders = GameObject.FindGameObjectsWithTag("ScaleSlider");
        foreach (GameObject Slider in Sliders)
        {
            Slider.GetComponent<Slider>().value = newScale;
        }

        GameObject[] Texts = GameObject.FindGameObjectsWithTag("ScaleText");
        foreach (GameObject Text in Texts)
        {
            Text.GetComponent<Text>().text = "Scale: " + newScale;
        }
    }

    //Map Function
    public static float Map(float value, float low1, float high1, float low2, float high2){
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    //Update Central Body
    public void ChangeCentralBody()
    {
        foreach (GameObject Body in PhysicsCalculation.GetBodies())
        {
            Body.GetComponentInChildren<TrailRenderer>().enabled = true;
        }
        string text = GetComponent<Dropdown>().options[GetComponent<Dropdown>().value].text;
        MenuController.SetCentralBody(text);

        //Turn off Parent Trail
        try
        {
            GameObject Parent = GameObject.Find(BodyInitialzation.GetCentralBody(text));
            if(Parent.GetComponentInChildren<TrailRenderer>() != null)
            {
                Parent.GetComponentInChildren<TrailRenderer>().enabled = false;
            }
        }
        catch(ArgumentOutOfRangeException e)
        {
            
        }

        //Turn off Trail
        if(GameObject.Find(text).GetComponentInChildren<TrailRenderer>() != null)
        {
            GameObject.Find(text).GetComponentInChildren<TrailRenderer>().enabled = false;
        }


        CloseMainMenu();
    }

    public void ToggleTrails(){
        foreach (GameObject Body in PhysicsCalculation.GetBodies())
        {
            if(Body.name != MenuController.GetCentralBody()){
                Body.GetComponentInChildren<TrailRenderer>().enabled = GetComponent<Toggle>().isOn;
            }
            else
            {
                Body.GetComponentInChildren<TrailRenderer>().enabled = false;
            }
        }
    }
}