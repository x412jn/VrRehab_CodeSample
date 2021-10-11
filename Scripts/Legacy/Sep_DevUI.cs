using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BCCH_VR_Therapy
{
    public class Sep_DevUI : MonoBehaviour
    {
        /*
         * [Level of crucial]:0
         *      0= not important, but don't know if there are any portantial risk if you remove this
         *      1= slightly important, but not the part of its function
         *      2= may slightly affect part of side function if you change anything
         *      3= may affect some of the major function if you change anything
         *      4= may break the entire project if you change anything
         * 
         */

        [Header("[[CORE]]")]
        //private SepJsonListener manager_JsonListener;
        //public SepSimManager manager_Simulation;
        public SepSimManagerNoJson manager_Simulation;
        //private SepConfigHolder configHolder;

        [Header("[[Left Hand Properties]]")]
        public GameObject UI_VRMenuGameObject;
        public GameObject UI_RootOverRoot;
        public GameObject UI_Config_Root;

        [Header("Put All Major Windows to below")]
        public GameObject[] subInterfaces;

        [Header("put message window to below")]
        public GameObject messageObject;
        public TextMeshProUGUI messageText;

        [Header("[Config Properties]")]
        public GameObject UI_Config_Driving;
        public GameObject UI_Config_Grocery;

        [Space]
        [Header("Driving")]
        public TextMeshProUGUI t_Driv_Tun_Speed;
        public TextMeshProUGUI t_Driv_Tun_CurvRate;
        public TextMeshProUGUI t_Driv_Tun_CurvDrast;
        public TextMeshProUGUI t_Driv_Tun_Torsion;

        

        public Slider inp_Driv_Tun_Speed;
        public Slider inp_Driv_Tun_CurvRate;
        public Slider inp_Driv_Tun_CurvDrast;
        public Slider inp_Driv_Tun_Torsion;
        public Dropdown inp_Drip_Tun_LightColor;

        [Space]
        [Header("Goucery")]
        public TextMeshProUGUI t_Groc_Density;
        public TextMeshProUGUI t_Groc_Contrast;

        public Slider inp_Groc_Density;
        public Slider inp_Groc_Contrast;

        [Header("put all sub-windows of CONFIG to below")]
        public GameObject[] subInterfaces_config;

        [Header("[[Don Put Anything to below]]")]

        public GameObject currentWindow;
        public List<GameObject> windowList;



        // Start is called before the first frame update
        void Awake()
        {
            //manager_JsonListener = GameObject.Find("JsonListener").GetComponent<SepJsonListener>();
            //configHolder = GameObject.Find("ConfigHolder").GetComponent<SepConfigHolder>();

            UI_VRMenuGameObject.SetActive(false);
            windowList = new List<GameObject>();
        }



        public void OnEnableFirstWindow()
        {
            UI_VRMenuGameObject.SetActive(true);
            UI_RootOverRoot.SetActive(true);
            currentWindow = UI_RootOverRoot;
        }



        public void OnHomeButtonClicked()
        {
            if (SceneManager.GetActiveScene().name == "SepStartScene")
            {
                //Close current window and open new window
                MessageCall("You are already in start scene!");
                UI_RootOverRoot.SetActive(false);

                //Add next window to current window
                currentWindow = messageObject;

                //Add closed window to list
                windowList.Add(UI_RootOverRoot);
            }
            else
            {
                //RUN SIMULATION DISABLE TO END SIMULATION
                manager_Simulation.OnDisableSimulation();
                //SceneManager.LoadScene("SepStartScene");
            }
        }

        public void OnConfigButtonClicked()
        {
            //CURRENT WINDOW IS [UI_RootOverRoot]
            //NEXT WINDOW IS [UI_Config_Root]

            //CONFIG CANNOT BE POPED UP IN SIMULATION
            if (SceneManager.GetActiveScene().name != "SepStartScene")
            {
                //Close current window and open new window
                MessageCall("You are not in start scene!");
                UI_RootOverRoot.SetActive(false);

                //Add next window to current window
                currentWindow = messageObject;

                //Add closed window to list
                windowList.Add(UI_RootOverRoot);
            }
            //DISABLE ALL CONFIG SUB-WINDOWS
            for (int i = 0; i < subInterfaces_config.Length; i++)
            {
                subInterfaces_config[i].SetActive(false);
            }

            //Close current window and open new window
            UI_Config_Root.SetActive(true);
            UI_RootOverRoot.SetActive(false);

            //Add next window to current window
            currentWindow = UI_Config_Root;

            //Add closed window to list
            windowList.Add(UI_RootOverRoot);
        }

        public void OnExitButtonClicked()
        {
            //EXPORT LOG

            //RESET JSON
            manager_Simulation.OnDisableSimulation();

            //QUIT
            Application.Quit();
            //EditorApplication.isPlaying = false;
        }


        private void MessageCall(string onCallMessage)
        {
            messageObject.SetActive(true);
            messageText.text = onCallMessage;
        }

        public void OnReturnClicked()
        {
            currentWindow.SetActive(false);
            windowList[windowList.Count - 1].SetActive(true);
            currentWindow = windowList[windowList.Count - 1];
            windowList.RemoveAt(windowList.Count - 1);
        }

        public void DisableEverything()
        {
            windowList.Clear();
            currentWindow = null;

            messageObject.SetActive(false);
            for (int i = 0; i < subInterfaces.Length; i++)
            {
                subInterfaces[i].SetActive(false);
            }

            UI_VRMenuGameObject.SetActive(false);
        }

        //=================================
        //FOR CONFIG
        //=================================

        public void Config_OnDrivingConfig()
        {
            //CURRENT WINDOW IS [UI_Config_Root]
            //NEXT WINDOW IS [UI_Config_Driving]

            //Close current window and open new window
            UI_Config_Root.SetActive(false);
            UI_Config_Driving.SetActive(true);

            //Add next window to current window
            currentWindow = UI_Config_Driving;

            //Add closed window to list
            windowList.Add(UI_Config_Root);
        }

        public void Config_OnGroceryConfig()
        {
            //CURRENT WINDOW IS [UI_Config_Root]
            //NEXT WINDOW IS [UI_Config_Grocery]

            //Close current window and open new window
            UI_Config_Root.SetActive(false);
            UI_Config_Grocery.SetActive(true);

            //Add next window to current window
            currentWindow = UI_Config_Grocery;

            //Add closed window to list
            windowList.Add(UI_Config_Root);

        }


        //UPDATE SPEED VALUE TO CONFIG HOLDER
        public void Upd_Driv_Tun_Speed()
        {
            int intVar = (int)inp_Driv_Tun_Speed.value;
            t_Driv_Tun_Speed.text = intVar.ToString();

            //configHolder.driving_MaximumSpeed = inp_Driv_Tun_Speed.value;
        }

        public void Upd_Driv_Tun_CurvRate()
        {
            int intVar = (int)inp_Driv_Tun_CurvRate.value;
            t_Driv_Tun_CurvRate.text = intVar.ToString();

            //configHolder.driving_Turns = (int)inp_Driv_Tun_CurvRate.value;
        }

        public void Upd_Driv_Tun_CurvDrast()
        {
            switch ((int)inp_Driv_Tun_CurvDrast.value)
            {
                case 1:
                    t_Driv_Tun_CurvDrast.text = "Low";
                    break;

                case 2:
                    t_Driv_Tun_CurvDrast.text = "Medium";
                    break;

                case 3:
                    t_Driv_Tun_CurvDrast.text = "High";
                    break;
            }

            //configHolder.driving_Bumps = (int)inp_Driv_Tun_CurvDrast.value;
        }

        public void Upd_Driv_Tun_Torsion()
        {
            int intVar = (int)inp_Driv_Tun_Torsion.value;
            t_Driv_Tun_Torsion.text = intVar.ToString();

            //configHolder.driving_Torsion = (int)inp_Driv_Tun_Torsion.value;
        }

        public void Upd_Driv_Tun_Light_Color()
        {
            //configHolder.driving_Tunnel_Light_Color = inp_Drip_Tun_LightColor.value;
        }

        public void Upd_Groc_Density()
        {
            switch ((int)inp_Groc_Density.value)
            {
                case 1:
                    t_Groc_Density.text = "low";
                    break;
                case 2:
                    t_Groc_Density.text = "medium";
                    break;
                case 3:
                    t_Groc_Density.text = "high";
                    break;
                default:
                    t_Groc_Density.text = "low";
                    break;
            }
            //configHolder.grocery_density = (int)inp_Groc_Density.value;
        }

        public void Upd_Groc_Contrast()
        {
            switch ((int)inp_Groc_Contrast.value)
            {
                case 1:
                    t_Groc_Contrast.text = "low";
                    break;
                case 2:
                    t_Groc_Contrast.text = "medium";
                    break;
                case 3:
                    t_Groc_Contrast.text = "high";
                    break;
                default:
                    t_Groc_Contrast.text = "low";
                    break;
            }
            //configHolder.grocery_contrast = (int)inp_Groc_Contrast.value;
        }



        public void Config_Driving_Tun_Start()
        {
            manager_Simulation.OnEnableSimulation_DevOnly();
            //SceneManager.LoadScene("SepDrivingScene");
        }

        public void Config_Grocery_Start()
        {
            manager_Simulation.OnEnableSimulation_DevOnly_Grocery();
        }
    }
}

