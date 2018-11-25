using System;
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
               //Body.GetComponentInChildren<TrailRenderer>().Clear();
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
                catch(ArgumentOutOfRangeException)
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
                catch(ArgumentOutOfRangeException)
                {

                }
            }

            Transform CameraTransform = PlayerTransform.Find("Main Camera");

            //Forward Menu
            Vector3 menuPosition = PlayerTransform.position + CameraTransform.forward * distance;
            GameObject Menu1 = Instantiate(MainMeunPrefab, menuPosition, Quaternion.LookRotation(CameraTransform.forward));

            //Backward Menu
            menuPosition = PlayerTransform.position + CameraTransform.forward * -distance;
            GameObject Menu2 = Instantiate(MainMeunPrefab, menuPosition, Quaternion.LookRotation(-CameraTransform.forward));


            //Set Menus as children of MenuManager to keep them together and easy to move
            Menu1.transform.SetParent(GameObject.Find("MenuManager").transform);
            Menu2.transform.SetParent(GameObject.Find("MenuManager").transform);

            Menu1.GetComponentInChildren<Dropdown>().itemText.text = centralBody;
            Menu2.GetComponentInChildren<Dropdown>().itemText.text = centralBody;

            if (centralBody != "Sun")
            {
                //Info Menus
                menuPosition = PlayerTransform.position + Quaternion.AngleAxis(16, Menu1.transform.right) * CameraTransform.forward * distance;
                GameObject InfoMenu1 = Instantiate(InfoMeunPrefab, menuPosition, Quaternion.LookRotation(    Quaternion.AngleAxis(16, Menu1.transform.right) * CameraTransform.forward * distance   ));
                InfoMenu1.transform.SetParent(GameObject.Find("MenuManager").transform);

                menuPosition = PlayerTransform.position + Quaternion.AngleAxis(16, Menu2.transform.right) * CameraTransform.forward * -distance;
                GameObject InfoMenu2 = Instantiate(InfoMeunPrefab, menuPosition, Quaternion.LookRotation(    Quaternion.AngleAxis(16, Menu2.transform.right) * CameraTransform.forward * -distance   ));
                InfoMenu2.transform.SetParent(GameObject.Find("MenuManager").transform);

                SetInfoText(centralBody);
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
                PlayerTransform.position = new Vector3(-5, 2 ,0);
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
            string text = string.Format("Mass: {0} kilograms\nRadius: {1} kilometers\nAphelion: {2} kilometers\nPerhelion: {3} kilometers\nRotation Period: {4} hours\nOrbital Inclination: {5} degrees\nOrbital Obliquity: {6} degrees",
                                        (BodyInitialzation.GetScaledMass(bodyName) * PhysicsCalculation.trueMassScalar).ToString("e2"),
                                        (BodyInitialzation.GetScaledRadius(bodyName) * PhysicsCalculation.trueDistanceScalar).ToString("e2"),
                                        (BodyInitialzation.GetScaledDistance(bodyName) * (1 + BodyInitialzation.GetOrbitalEccentricity(bodyName)) * PhysicsCalculation.trueDistanceScalar).ToString("e2"),
                                        (BodyInitialzation.GetScaledDistance(bodyName) * (1 - BodyInitialzation.GetOrbitalEccentricity(bodyName)) * PhysicsCalculation.trueDistanceScalar).ToString("e2"),
                                        BodyInitialzation.GetRotationPeriod(bodyName).ToString("f2"),
                                        BodyInitialzation.GetOrbitalInclination(bodyName).ToString("f1"),
                                        BodyInitialzation.GetOrbitObliquity(bodyName).ToString("f1")
                                       );

            GameObject[] InfoMenus = GameObject.FindGameObjectsWithTag("InfoMenu");
            foreach(GameObject InfoMenu in InfoMenus)
            {
                Text[] TextObjects = InfoMenu.GetComponentsInChildren<Text>();

                foreach (Text TextObject in TextObjects)
                {
                    if (TextObject.name == "Body Text")
                    {
                        TextObject.text = bodyName.ToUpper();
                    }
                    else if (TextObject.name == "Description Text")
                    {
                        TextObject.text = text;
                    }
                }
            }
        }
    }
}