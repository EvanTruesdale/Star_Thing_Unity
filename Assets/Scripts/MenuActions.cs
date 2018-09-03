using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class MenuActions : MonoBehaviour
{

    //Close All Menus
    public void CloseMainMenu()
    {
        GameObject[] Menus = GameObject.FindGameObjectsWithTag("MainMenu");
        foreach (GameObject Menu in Menus)
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

    //Update Central Body
    public void ChangeCentralBody()
    {
        foreach (GameObject Body in PhysicsCalculation.GetBodies())
        {
            Body.GetComponentInChildren<TrailRenderer>().enabled = true;
            MenuController.SetScale(1);
        }
        string text = GetComponent<Dropdown>().options[GetComponent<Dropdown>().value].text;
        MenuController.SetCentralBody(text);

        GameObject.Find(text).GetComponentInChildren<TrailRenderer>().enabled = false;
        CloseMainMenu();
    }
}