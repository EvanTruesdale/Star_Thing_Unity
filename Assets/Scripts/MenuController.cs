﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

namespace Assets.Scripts
{

    public class MenuController : MonoBehaviour
    {

        public static Transform PlayerTransform;
        static GameObject MainMeunPrefab;
        static GameObject InfoMeunPrefab;

        static string centralBody = "Sun";
        string infoText = "";
        static bool isMenuOpen;
        static float distance = 15;

        static float speed = 1;
        static float scale = 1;

        void Start()
        {
            //Get Player and Prefabs
            PlayerTransform = GameObject.Find("Player").transform;
            MainMeunPrefab = Resources.Load<GameObject>("MainMenuPrefab");
            InfoMeunPrefab = Resources.Load<GameObject>("InfoMenuPrefab");
            //Menu Parameters are disabled to start
            isMenuOpen = false;
            GameObject.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = false;
        }

        void Update()
        {
            //Open Menu
            if (Input.GetButton("Fire1") && !isMenuOpen)
            {
                OpenMainMenu();
            }

            //Move Bodies to put central body at origin
            foreach (GameObject Body in PhysicsCalculation.GetBodies(true))
            {
                if(Body.name != centralBody)
                {
                    Body.GetComponent<Transform>().position -= GameObject.Find(centralBody).GetComponent<Transform>().position;
                }
            }
            GameObject.Find(centralBody).GetComponent<Transform>().position = new Vector3(0, 0, 0);

            foreach (GameObject Body in PhysicsCalculation.GetBodies())
            {
                Body.GetComponentInChildren<TrailRenderer>().Clear();
            }

            GameObject.Find("MenuManager").GetComponent<Transform>().position = PlayerTransform.position;
        }

        public void OpenMainMenu()
        {
            //Set Menu State
            isMenuOpen = true;

            //Enable Reticle
            GameObject.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = true;

            //Get Dropdown Data (Bodies)
            MainMeunPrefab.GetComponentInChildren<Dropdown>().ClearOptions();

            //Add selected Body first
            foreach (GameObject Body in PhysicsCalculation.GetBodies(true))
            {
                if (Body.name == centralBody)
                {
                    Dropdown.OptionData data = new Dropdown.OptionData(Body.name);
                    MainMeunPrefab.GetComponentInChildren<Dropdown>().options.Add(data);
                }
            }
            //Add Parent Body
            foreach (GameObject Body in PhysicsCalculation.GetBodies(true))
            {
                try
                {
                    if(Body.name == BodyInitialzation.GetCentralBody(centralBody))
                    {
                        Dropdown.OptionData data = new Dropdown.OptionData(Body.name);
                        MainMeunPrefab.GetComponentInChildren<Dropdown>().options.Add(data);
                    }
                }
                catch(ArgumentOutOfRangeException e)
                {
                    
                }
            }
            //Add Children Bodies
            foreach (GameObject Body in PhysicsCalculation.GetBodies(true))
            {
                try
                {
                    if(BodyInitialzation.GetCentralBody(Body.name) == centralBody)
                    {
                        Dropdown.OptionData data = new Dropdown.OptionData(Body.name);
                        MainMeunPrefab.GetComponentInChildren<Dropdown>().options.Add(data);
                    }
                }
                catch(ArgumentOutOfRangeException e)
                {

                }
            }

            //Forward Menu
            Vector3 menuPosition = PlayerTransform.position + PlayerTransform.Find("Main Camera").forward * distance;
            GameObject Menu1 = Instantiate(MainMeunPrefab, menuPosition, Quaternion.LookRotation(PlayerTransform.Find("Main Camera").forward));

            //Backward Menu
            menuPosition = PlayerTransform.position + PlayerTransform.Find("Main Camera").forward * -distance;
            GameObject Menu2 = Instantiate(MainMeunPrefab, menuPosition, Quaternion.LookRotation(-PlayerTransform.Find("Main Camera").forward));


            //Set Menus as children of MenuManager to keep them together and easy to move
            Menu1.transform.SetParent(GameObject.Find("MenuManager").transform);
            Menu2.transform.SetParent(GameObject.Find("MenuManager").transform);

            Menu1.GetComponentInChildren<Dropdown>().itemText.text = centralBody;
            Menu2.GetComponentInChildren<Dropdown>().itemText.text = centralBody;

            if (centralBody != "Sun")
            {
                //Info Menu
                //menuPosition = PlayerTransform.position + PlayerTransform.Find("Main Camera").forward * distance;
                //GameObject InfoMenu = Instantiate(InfoMeunPrefab, menuPosition, Quaternion.LookRotation(PlayerTransform.Find("Main Camera").forward));
                //InfoMenu.transform.SetParent(GameObject.Find("MenuManager").transform);
                //InfoMenu.transform.Rotate(new Vector3(0, 30f, 0));
                //InfoMenu.transform.position = menuPosition.normalized * 8 + Vector3.Cross(PlayerTransform.Find("Main Camera").up, PlayerTransform.Find("Main Camera").forward).normalized * 8;
                //SetInfoText(centralBody);
            }

            //Set Slider values
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

        //Read and Set Methods for various constants
        public static void SetMenuState(bool input)
        {
            isMenuOpen = input;
        }

        public static void SetSpeed(float input)
        {
            speed = input;
            Time.timeScale = .1f * speed;
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
                float radius = BodyInitialzation.GetScaledRadius(Body.name);
                float bodyScale = MenuActions.Map(scale, 1, 10, radius, 1f);
                Body.GetComponent<Transform>().localScale = new Vector3(bodyScale, bodyScale, bodyScale);
            }
        }

        public static float GetScale()
        {
            return scale;
        }

        public static void SetCentralBody(string input)
        {
            centralBody = input;

            if (centralBody == "Sun")
            {
                PlayerTransform.position = new Vector3(-40, 15, 0);
            }
            else
            {
                PlayerTransform.position = new Vector3(-5, 0 ,0);
            }

            foreach (GameObject Body in PhysicsCalculation.GetBodies(true))
            {
                if(Body.name != centralBody)
                {
                    Body.GetComponent<Transform>().position -= GameObject.Find(centralBody).GetComponent<Transform>().position;
                }
            }
            GameObject.Find(centralBody).GetComponent<Transform>().position = new Vector3(0, 0, 0);
        }

        public static string GetCentralBody()
        {
            return centralBody;
        }

        public void SetInfoText(string bodyName)
        {
            string text = string.Format("Mass: {0} kilograms\nRadius: {1} kilometers\nAphelion: {2} kilometers\nPerhelion: {3} kilometers\nRotation Period: {4} days\nOrbital Inclination: {5} degrees\nOrbital Obliquity: {6} degrees",
                                        Mathf.Round(BodyInitialzation.GetScaledMass(bodyName) * PhysicsCalculation.massScalar * 100) / 100,
                                        Mathf.Round(BodyInitialzation.GetScaledRadius(bodyName) * PhysicsCalculation.radiusScalar * 100) / 100,
                                        Mathf.Round(BodyInitialzation.GetScaledDistance(bodyName) * (1 + BodyInitialzation.GetOrbitalEccentricity(bodyName)) * PhysicsCalculation.distanceScalar * 100) / 100,
                                        Mathf.Round(BodyInitialzation.GetScaledDistance(bodyName) * (1 - BodyInitialzation.GetOrbitalEccentricity(bodyName)) * PhysicsCalculation.distanceScalar * 100) / 100,
                                        Mathf.Round(BodyInitialzation.GetRotationPeriod(bodyName) * 100) / 100,
                                        Mathf.Round(BodyInitialzation.GetOrbitalInclination(bodyName) * 100) / 100,
                                        Mathf.Round(BodyInitialzation.GetOrbitObliquity(bodyName) * 100) / 100
                                       );

            GameObject InfoMenu = GameObject.FindWithTag("InfoMenu");
            Text[] TextObjects = InfoMenu.GetComponentsInChildren<Text>();

            foreach (Text TextObject in TextObjects)
            {
                if (TextObject.name == "Body Text")
                {
                    TextObject.text = bodyName;
                }
                else if (TextObject.name == "Description Text")
                {
                    TextObject.text = text;
                }
            }
        }
    }
}