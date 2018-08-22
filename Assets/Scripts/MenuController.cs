using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{

    public class MenuController : MonoBehaviour
    {

        static Transform PlayerTransform;
        static GameObject MainMeunPrefab;
        static bool isMenuOpen;
        static float distance = 15;
        static float speed = 1;

        void Start()
        {
            PlayerTransform = GameObject.Find("Player").transform;
            MainMeunPrefab = Resources.Load<GameObject>("MainMenuPrefab");
            isMenuOpen = false;
        }

        void Update()
        {
            if (Input.GetButton("Fire1") && !isMenuOpen)
            {
                OpenMainMenu();
            }
        }

        public void OpenMainMenu()
        {
            isMenuOpen = true;

            Vector3 menuPosition = PlayerTransform.position + PlayerTransform.forward * distance;
            Instantiate(MainMeunPrefab, menuPosition, Quaternion.Euler(0, 90, 0));

            menuPosition = PlayerTransform.position + PlayerTransform.forward * -distance;
            Instantiate(MainMeunPrefab, menuPosition, Quaternion.Euler(0, -90, 0));

            GameObject[] SpeedSliders = GameObject.FindGameObjectsWithTag("SpeedSlider");
            foreach (GameObject Slider in SpeedSliders)
            {
                Slider.GetComponent<Slider>().value = speed;
            }
        }

        public static void SetMenuState(bool input)
        {
            isMenuOpen = input;
        }

        public static void SetSpeed(float input){
            speed = input;
        }

        public static float GetSpeed(){
            return speed;
        }
    }
}