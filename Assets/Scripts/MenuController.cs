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

        static string centralBody = "default";
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

            if (centralBody == "default")
            {

            }
            else
            {

                //Move Camera to new Position in front of Central Body

                Vector3 playerPosition = new Vector3();
                playerPosition = GameObject.Find(centralBody).GetComponent<Transform>().position;
                playerPosition -= new Vector3(1, 0, 0);

                PlayerTransform.position = playerPosition;

                GameObject.Find("MenuManager").GetComponent<Transform>().position = playerPosition;
            }
        }

        public void OpenMainMenu()
        {
            //Set Menu State
            isMenuOpen = true;

            //Enable Reticle
            GameObject.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = true;

            //Get Dropdown Data (Bodies)
            MainMeunPrefab.GetComponentInChildren<Dropdown>().ClearOptions();
            foreach (GameObject Body in PhysicsCalculation.GetBodies())
            {
                Dropdown.OptionData data = new Dropdown.OptionData(Body.name);
                MainMeunPrefab.GetComponentInChildren<Dropdown>().options.Add(data);
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

            if(centralBody != "default"){
                //Info Menu
                menuPosition = PlayerTransform.position + PlayerTransform.Find("Main Camera").forward * distance;
                GameObject LoadMenu = Instantiate(InfoMeunPrefab, menuPosition, Quaternion.LookRotation(PlayerTransform.Find("Main Camera").forward));
                LoadMenu.transform.SetParent(GameObject.Find("MenuManager").transform);
                LoadMenu.transform.Rotate(new Vector3(20f, 0, 0));
                LoadMenu.transform.position += new Vector3(0, -6.5f, 0);
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
                float radius = BodyInitialzation.GetScaledRadius(Body.name);
                float bodyScale = MenuActions.Map(scale, 1, 10, radius, 1);
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
        }

        public void SetInfoText(string bodyName)
        {
            string text = string.Format("Mass: {0} kilograms\nRadius: {1} kilometers\nAphelion: {2} kilometers\nPerhelion: {3} kilometers\nRotation Period: {4} days\nOrbital Inclination: {5} degrees\nOrbital Obliquity: {6} degrees",
                                        BodyInitialzation.GetScaledMass(bodyName) * PhysicsCalculation.massScalar,
                                        BodyInitialzation.GetScaledRadius(bodyName) * PhysicsCalculation.radiusScalar,
                                        BodyInitialzation.GetScaledDistance(bodyName) * (1 + BodyInitialzation.GetOrbitalEccentricity(bodyName)) * PhysicsCalculation.distanceScalar,
                                        BodyInitialzation.GetScaledDistance(bodyName) * (1 - BodyInitialzation.GetOrbitalEccentricity(bodyName)) * PhysicsCalculation.distanceScalar,
                                        BodyInitialzation.GetRotationPeriod(bodyName),
                                        BodyInitialzation.GetOrbitalInclination(bodyName),
                                        BodyInitialzation.GetOrbitObliquity(bodyName)
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