using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class MenuActions : MonoBehaviour
{

    public void CloseMainMenu()
    {
        GameObject[] Menus = GameObject.FindGameObjectsWithTag("MainMenu");
        foreach (GameObject Menu in Menus)
        {
            Destroy(Menu);
        }
        MenuController.SetMenuState(false);
    }

    public void ChangeSpeed()
    {
        float newSpeed = gameObject.GetComponent<Slider>().value;
        MenuController.SetSpeed(newSpeed);
        GameObject[] Sliders = GameObject.FindGameObjectsWithTag("SpeedSlider");
        foreach (GameObject Slider in Sliders)
        {
            Slider.GetComponent<Slider>().value = newSpeed;
        }
    }

}