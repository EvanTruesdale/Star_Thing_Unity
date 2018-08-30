using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

namespace Assets.Scripts
{

    public class MenuController : MonoBehaviour
    {

        static Transform PlayerTransform;
        static GameObject MainMeunPrefab;
        static bool isMenuOpen;
        static float distance = 15;
        static float speed = 1;
        static float scale = 1;

        void Start()
        {
            PlayerTransform = GameObject.Find("Main Camera").transform;
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
            Instantiate(MainMeunPrefab, menuPosition, Quaternion.LookRotation(PlayerTransform.forward));

            menuPosition = PlayerTransform.position + PlayerTransform.forward * -distance;
            Instantiate(MainMeunPrefab, menuPosition, Quaternion.LookRotation(-PlayerTransform.forward));

            GameObject[] SpeedSliders = GameObject.FindGameObjectsWithTag("SpeedSlider");
            foreach (GameObject Slider in SpeedSliders)
            {
                Slider.GetComponent<Slider>().value = speed;
            }

            GameObject[] ScaleSliders = GameObject.FindGameObjectsWithTag("ScaleSlider");
            foreach (GameObject Slider in ScaleSliders)
            {
                Slider.GetComponent<Slider>().value = scale;
            }
        }

        public static void SetMenuState(bool input)
        {
            isMenuOpen = input;
        }

        public static void SetSpeed(float input)
        {
            speed = input;
            Time.timeScale = speed;
        }

        public static float GetSpeed()
        {
            return speed;
        }

        public static void SetScale(float input)
        {
            scale = input;
            List<GameObject> Bodies = PhysicsCalculation.GetBodies();
            foreach (GameObject Body in Bodies)
            {
                int index = BodyInitialzation.GetNames().IndexOf(Body.name);
                float radius = BodyInitialzation.GetRadii()[index];
                Body.GetComponent<Transform>().localScale = new Vector3(radius * scale, radius * scale, radius * scale);
            }
        }

        public static float GetScale()
        {
            return scale;
        }
    }
}