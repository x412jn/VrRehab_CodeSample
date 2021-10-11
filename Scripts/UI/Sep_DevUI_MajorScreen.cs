using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BCCH_VR_Therapy
{
    public class Sep_DevUI_MajorScreen : MonoBehaviour
    {
        /*
         * [Level of crucial]:1
         *      0= not important, but don't know if there are any portantial risk if you remove this
         *      1= slightly important, but not the part of its function
         *      2= may slightly affect part of side function if you change anything
         *      3= may affect some of the major function if you change anything
         *      4= may break the entire project if you change anything
         * 
         * 
         * [Description]
         * This is a script for developer mode to control configuration while standalone.
         * It would be called when you holds Y button while you are in SepStartScene.
         * 
         * To apply this script, you should manually binding properties that it required on the inspetor
         * 
         * Then, in your XR Rig, you can attach a input listener to call methods below.
         * 
         *      There are two major parts of this function:
         *          1.In hand ui
         *          2.On display major screen
         *      By calling in-hand ui, you can control which major screen panel you want to display.
         *      All the sub-function to control parameters are connected with major screen, 
         *      yet due to the complexity, we cannot combine all the ui into one single in-hand panel.
         *      
         * [Explain of in-hand ui methods]
         *      In order to pops up the first in-hand ui window, you need to call "OnEnableFirstWindow".
         *      In order to close all in-hand ui windows, you need to call "OnDisable_Everything"
         *      To return to last window, you can call "OnClicked_Return"
         *      To exit the program, you can call "OnClicked_ExitButton"
         * 
         * [Explain of major screen methods]
         *      We need to call "Upd_" methods when user change the slider value
         *      it need to update its new value into PlayerPref
         *      then update text that it suppose to display on ui
         * 
         * [Explain of starting the simulation]
         *      Due to the legacy solution, we implemented another script called "SepSimManagerNoJson"
         *      all the function related to loading scene are depending on that script,
         *      and you have to manually attach to a gameobject, then attach that gameobject to object with this script
         *      so far, as we concern about any potential risk that might come up with, we decide not to remove SimManager for now.
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

        [Header("Put All MAJOR SCREEN Windows below")]
        public GameObject[] subInterfaces_MajorScreens;

        [Header("put message window to below")]
        public GameObject messageObject;
        public TextMeshProUGUI messageText;

        [Header("[Config Properties]")]
        public GameObject UI_MajorScreen_Driving;
        public GameObject UI_MajorScreen_Grocery;

        [Space]
        [Header("Driving")]
        public TextMeshProUGUI t_Driv_Speed;
        public TextMeshProUGUI t_Driv_Turns;
        public TextMeshProUGUI t_Driv_Bumps;
        public TextMeshProUGUI t_Driv_Torsion;
        public TextMeshProUGUI t_Driv_CtrySide_TreesDensity;
        public TextMeshProUGUI t_Driv_Traffic;
        public TextMeshProUGUI t_Driv_Tun_RoadSign;

        public TextMeshProUGUI t_Driv_Length_CtrySide;
        public TextMeshProUGUI t_Driv_Length_Tunnel;

        public TextMeshProUGUI t_Driv_TimeOfDay;
        public TextMeshProUGUI t_Driv_Weather;

        [Space]
        public Toggle inp_Driv_EnableTunnel;
        public Toggle inp_Driv_EnableCountryside;

        public Slider inp_Driv_Speed;
        public Slider inp_Driv_Turns;
        public Slider inp_Driv_Bumps;
        public Slider inp_Driv_Torsion;
        public Dropdown inp_Driv_Tun_LightColor;

        public Slider inp_Driv_CtrySide_TreesDensity;
        public Slider inp_Driv_Traffic;
        public Slider inp_Driv_Tun_RoadSign;

        public Slider inp_Driv_Length_CtrySide;
        public Slider inp_Driv_Length_Tunnel;

        public Slider inp_Driv_TimeOfDay;
        public Slider inp_Driv_Weather;

        [Space]
        [Header("Goucery")]
        public TextMeshProUGUI t_Groc_Density;
        public TextMeshProUGUI t_Groc_Contrast;
        public TextMeshProUGUI t_Groc_Target;
        public TextMeshProUGUI t_Groc_Speed;

        public Slider inp_Groc_Density;
        public Slider inp_Groc_Contrast;
        public Slider inp_Groc_Target;
        public Slider inp_Groc_Speed;

        public Toggle inp_Groc_EnableTask;

        
        [Space]
        [Header("[[Don Put Anything to below]]")]

        public GameObject currentWindow;
        public List<GameObject> windowList;



        // Start is called before the first frame update
        void Awake()
        {
            //manager_JsonListener = GameObject.Find("JsonListener").GetComponent<SepJsonListener>();
            //configHolder = GameObject.Find("ConfigHolder").GetComponent<SepConfigHolder>();


            //Instantiate menu objects and create a list for storing previous panels(for calling "return" function, we should always have a trace that allows us to move back to last window that we closed)
            UI_VRMenuGameObject.SetActive(false);
            windowList = new List<GameObject>();

            
            


            //Initiate the ui value according to PlayerPref parameters
            inp_Driv_Bumps.value = (float)PlayerPrefs.GetInt("Driving_Bumps", (int)inp_Driv_Bumps.value);
            inp_Driv_CtrySide_TreesDensity.value = (float)PlayerPrefs.GetInt("Driving_Countryside_TreesDensity", (int)inp_Driv_CtrySide_TreesDensity.value);
            inp_Driv_Length_CtrySide.value = (float)PlayerPrefs.GetInt("Driving_Length_Countryside", (int)inp_Driv_Length_CtrySide.value);
            inp_Driv_Length_Tunnel.value = (float)PlayerPrefs.GetInt("Driving_Length_Tunnel", (int)inp_Driv_Length_Tunnel.value);
            inp_Driv_Speed.value = PlayerPrefs.GetFloat("Driving_MaximumSpeed", inp_Driv_Speed.value);
            inp_Driv_TimeOfDay.value = (float)PlayerPrefs.GetInt("Driving_TimeOfDay", (int)inp_Driv_TimeOfDay.value);
            inp_Driv_Torsion.value = PlayerPrefs.GetFloat("Driving_Torsion", inp_Driv_Torsion.value);
            inp_Driv_Traffic.value = (float)PlayerPrefs.GetInt("Driving_Traffic", (int)inp_Driv_Traffic.value);
            inp_Driv_Tun_LightColor.value = PlayerPrefs.GetInt("Driving_Tunnel_LightColor", inp_Driv_Tun_LightColor.value);
            inp_Driv_Turns.value = (float)PlayerPrefs.GetInt("Driving_Turns", (int)inp_Driv_Turns.value);
            inp_Driv_Weather.value = (float)PlayerPrefs.GetInt("Driving_Weather", (int)inp_Driv_Weather.value);
            inp_Groc_Contrast.value = (float)PlayerPrefs.GetInt("Grocery_Contrast", (int)inp_Groc_Contrast.value);
            inp_Groc_Density.value = (float)PlayerPrefs.GetInt("Grocery_Density", (int)inp_Groc_Density.value);
            inp_Groc_Target.value = (float)PlayerPrefs.GetInt("Grocery_TargetAmount", (int)inp_Groc_Target.value);
            inp_Groc_Speed.value = (float)PlayerPrefs.GetInt("Grocery_Speed", (int)inp_Groc_Speed.value);


            inp_Driv_EnableCountryside.isOn = (PlayerPrefs.GetInt("EnableCountryside", 1) == 1);
            inp_Driv_EnableTunnel.isOn = (PlayerPrefs.GetInt("EnableTunnel", 1) == 1);
            inp_Groc_EnableTask.isOn = (PlayerPrefs.GetInt("Grocery_TaskEnabled", 1) == 1);
            
        }



        public void OnEnableFirstWindow()
        {
            UI_VRMenuGameObject.SetActive(true);
            UI_RootOverRoot.SetActive(true);
            currentWindow = UI_RootOverRoot;
        }



        public void OnClicked_HomeButton()
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

        public void OnClicked_ConfigButton()
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
            

            //Close current window and open new window
            UI_Config_Root.SetActive(true);
            UI_RootOverRoot.SetActive(false);

            //Add next window to current window
            currentWindow = UI_Config_Root;

            //Add closed window to list
            windowList.Add(UI_RootOverRoot);
        }

        public void OnClicked_ExitButton()
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

        public void OnClicked_Return()
        {
            currentWindow.SetActive(false);
            windowList[windowList.Count - 1].SetActive(true);
            currentWindow = windowList[windowList.Count - 1];
            windowList.RemoveAt(windowList.Count - 1);
        }

        public void OnDisable_Everything()
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

        public void Config_OnClicked_Driving()
        {
            //CURRENT WINDOW IS [UI_Config_Root]
            //NEXT WINDOW IS [UI_Config_Driving]

            //Close current window and open new window
            for(int i = 0; i < subInterfaces_MajorScreens.Length; i++)
            {
                subInterfaces_MajorScreens[i].SetActive(false);
            }
            UI_MajorScreen_Driving.SetActive(true);

        }

        public void Config_OnClicked_Grocery()
        {
            //CURRENT WINDOW IS [UI_Config_Root]
            //NEXT WINDOW IS [UI_Config_Grocery]

            //Close current window and open new window
            for (int i = 0; i < subInterfaces_MajorScreens.Length; i++)
            {
                subInterfaces_MajorScreens[i].SetActive(false);
            }
            UI_MajorScreen_Grocery.SetActive(true);
        }

        //=======================================================================
        //UPDATE VALUE TO CONFIG HOLDER
        //=======================================================================
        public void Upd_Driv_EnableTunnel()
        {
            //configHolder.driving_EnableTunnel = inp_Driv_EnableTunnel.isOn;
            if (inp_Driv_EnableTunnel.isOn)
            {
                PlayerPrefs.SetInt("EnableTunnel", 1);
            }
            else
            {
                PlayerPrefs.SetInt("EnableTunnel", 0);
            }
            
        }

        public void Upd_Driv_EnableCountrySide()
        {
            if (inp_Driv_EnableCountryside.isOn)
            {
                PlayerPrefs.SetInt("EnableCountryside", 1);
            }
            else
            {
                PlayerPrefs.SetInt("EnableCountryside", 0);
            }
        }

        public void Upd_Driv_Speed()
        {
            int intVar = (int)inp_Driv_Speed.value;
            t_Driv_Speed.text = intVar.ToString();

            //configHolder.driving_MaximumSpeed = inp_Driv_Speed.value;
            PlayerPrefs.SetFloat("Driving_MaximumSpeed", inp_Driv_Speed.value);
            
        }

        public void Upd_Driv_Turns()
        {
            int intVar = (int)inp_Driv_Turns.value;
            t_Driv_Turns.text = intVar.ToString();

            //configHolder.driving_Turns = (int)inp_Driv_Turns.value;
            PlayerPrefs.SetInt("Driving_Turns", (int)inp_Driv_Turns.value);
        }

        public void Upd_Driv_Bumps()
        {
            switch ((int)inp_Driv_Bumps.value)
            {
                case 0:
                    t_Driv_Bumps.text = "None";
                    break;

                case 1:
                    t_Driv_Bumps.text = "Low";
                    break;

                case 2:
                    t_Driv_Bumps.text = "Medium";
                    break;

                case 3:
                    t_Driv_Bumps.text = "High";
                    break;

                default:
                    t_Driv_Bumps.text = "None";
                    break;
            }

            //configHolder.driving_Bumps = (int)inp_Driv_Bumps.value;
            PlayerPrefs.SetInt("Driving_Bumps", (int)inp_Driv_Bumps.value);
        }

        public void Upd_Driv_Torsion()
        {
            int intVar = (int)inp_Driv_Torsion.value;
            t_Driv_Torsion.text = intVar.ToString();

            //configHolder.driving_Torsion = (int)inp_Driv_Torsion.value;
            PlayerPrefs.SetFloat("Driving_Torsion", inp_Driv_Torsion.value);
        }

        public void Upd_Driv_Tun_Light_Color()
        {
            //configHolder.driving_Tunnel_Light_Color = inp_Driv_Tun_LightColor.value;
            PlayerPrefs.SetInt("Driving_Tunnel_LightColor", inp_Driv_Tun_LightColor.value);
        }

        public void Upd_Driv_CtrySide_TreeDensity()
        {
            switch ((int)inp_Driv_CtrySide_TreesDensity.value)
            {
                case 0:
                    t_Driv_CtrySide_TreesDensity.text = "None";
                    break;

                case 1:
                    t_Driv_CtrySide_TreesDensity.text = "Low";
                    break;

                case 2:
                    t_Driv_CtrySide_TreesDensity.text = "Med";
                    break;

                case 3:
                    t_Driv_CtrySide_TreesDensity.text = "High";
                    break;
                default:
                    t_Driv_CtrySide_TreesDensity.text = "None";
                    break;
            }

            //configHolder.driving_ContrySide_TreesDensity = (int)inp_Driv_CtrySide_TreesDensity.value;
            PlayerPrefs.SetInt("Driving_Countryside_TreesDensity", (int)inp_Driv_CtrySide_TreesDensity.value);
        }

        public void Upd_Driv_Traffic()
        {
            switch ((int)inp_Driv_Traffic.value)
            {
                case 0:
                    t_Driv_Traffic.text = "None";
                    break;

                case 1:
                    t_Driv_Traffic.text = "Low";
                    break;
                case 2:
                    t_Driv_Traffic.text = "Med";
                    break;
                case 3:
                    t_Driv_Traffic.text = "High";
                    break;
                default:
                    t_Driv_Traffic.text = "None";
                    break;
            }
            //configHolder.driving_Traffic = (int)inp_Driv_Traffic.value;
            PlayerPrefs.SetInt("Driving_Traffic", (int)inp_Driv_Traffic.value);
        }

        public void Upd_Driv_Tun_RoadSign()
        {
            switch ((int)inp_Driv_Tun_RoadSign.value)
            {
                case 1:
                    t_Driv_Tun_RoadSign.text = "None";
                    break;
                case 2:
                    t_Driv_Tun_RoadSign.text = "Low";
                    break;
                case 3:
                    t_Driv_Tun_RoadSign.text = "High";
                    break;
                default:
                    t_Driv_Tun_RoadSign.text = "None";
                    break;
            }
            //configHolder.driving_Tunnel_RoadSign = (int)inp_Driv_Tun_RoadSign.value;
            PlayerPrefs.SetInt("Driving_Tunnel_Roadsign", (int)inp_Driv_Tun_RoadSign.value);
        }


        public void Upd_Driv_Length_CountrySide()
        {
            int intVar = (int)inp_Driv_Length_CtrySide.value;
            t_Driv_Length_CtrySide.text = intVar.ToString();

            //configHolder.driving_Length_CountrySide = (int)inp_Driv_Length_CtrySide.value;
            PlayerPrefs.SetInt("Driving_Length_Countryside", (int)inp_Driv_Length_CtrySide.value);
        }

        public void Upd_Driv_Length_Tunnel()
        {
            int intVar = (int)inp_Driv_Length_Tunnel.value;
            t_Driv_Length_Tunnel.text = intVar.ToString();

            //configHolder.driving_Length_Tunnel = (int)inp_Driv_Length_Tunnel.value;
            PlayerPrefs.SetInt("Driving_Length_Tunnel", (int)inp_Driv_Length_Tunnel.value);
        }

        public void Upd_Driv_TimeOfDay()
        {
            switch ((int)inp_Driv_TimeOfDay.value)
            {
                case 0:
                    t_Driv_TimeOfDay.text = "Noon";
                    break;
                case 1:
                    t_Driv_TimeOfDay.text = "Afternoon";
                    break;
                case 2:
                    t_Driv_TimeOfDay.text = "Dawn";
                    break;
                case 3:
                    t_Driv_TimeOfDay.text = "Night";
                    break;
                default:
                    t_Driv_TimeOfDay.text = "Noon";
                    break;
            }
            PlayerPrefs.SetInt("Driving_TimeOfDay", (int)inp_Driv_TimeOfDay.value);
        }

        public void Upd_Driv_Weather()
        {
            switch ((int)inp_Driv_Weather.value)
            {
                case 0:
                    t_Driv_Weather.text = "Clear Sky";
                    break;
                case 1:
                    t_Driv_Weather.text = "Cloudy";
                    break;
                case 2:
                    t_Driv_Weather.text = "Rain";
                    break;
                default:
                    t_Driv_Weather.text = "Clear Sky";
                    break;
            }
            PlayerPrefs.SetInt("Driving_Weather", (int)inp_Driv_Weather.value);
        }


        //=================================================

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
            PlayerPrefs.SetInt("Grocery_Density", (int)inp_Groc_Density.value);
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
            PlayerPrefs.SetInt("Grocery_Contrast", (int)inp_Groc_Contrast.value);
        }

        public void Upd_Groc_Target()
        {
            t_Groc_Target.text = inp_Groc_Target.value.ToString();
            PlayerPrefs.SetInt("Grocery_TargetAmount", (int)inp_Groc_Target.value);
        }

        public void Upd_Groc_EnableTask()
        {
            if (inp_Groc_EnableTask.isOn)
            {
                PlayerPrefs.SetInt("Grocery_TaskEnabled", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Grocery_TaskEnabled", 0);
            }
        }

        public void Upd_Groc_Speed()
        {
            switch ((int)inp_Groc_Speed.value)
            {
                case 1:
                    t_Driv_Speed.text = "low";
                    break;
                case 2:
                    t_Driv_Speed.text = "medium";
                    break;
                case 3:
                    t_Driv_Speed.text = "high";
                    break;
                default:
                    t_Driv_Speed.text = "low";
                    break;
            }
            PlayerPrefs.SetInt("Grocery_Speed", (int)inp_Groc_Speed.value);
        }

        //=======================================================================
        //START SIMULATION
        //=======================================================================

        public void Config_Driving_Tun_Start()
        {
            //synchonize data
            PlayerPrefs.SetInt("Driving_Bumps", (int)inp_Driv_Bumps.value);
            PlayerPrefs.SetInt("Driving_Countryside_TreesDensity", (int)inp_Driv_CtrySide_TreesDensity.value);
            PlayerPrefs.SetInt("Driving_Length_Countryside", (int)inp_Driv_Length_CtrySide.value);
            PlayerPrefs.SetInt("Driving_Length_Tunnel", (int)inp_Driv_Length_Tunnel.value);
            PlayerPrefs.SetFloat("Driving_MaximumSpeed", inp_Driv_Speed.value);
            PlayerPrefs.SetInt("Driving_TimeOfDay", (int)inp_Driv_TimeOfDay.value);
            PlayerPrefs.SetFloat("Driving_Torsion", inp_Driv_Torsion.value);
            PlayerPrefs.SetInt("Driving_Traffic", (int)inp_Driv_Traffic.value);
            PlayerPrefs.SetInt("Driving_Tunnel_LightColor", inp_Driv_Tun_LightColor.value);
            PlayerPrefs.SetInt("Driving_Turns", (int)inp_Driv_Turns.value);
            PlayerPrefs.SetInt("Driving_Weather", (int)inp_Driv_Weather.value);

            manager_Simulation.OnEnableSimulation_DevOnly();
            //SceneManager.LoadScene("SepDrivingScene");
        }

        public void Config_Grocery_Start()
        {
            //synchronize data
            PlayerPrefs.SetInt("Grocery_Contrast", (int)inp_Groc_Contrast.value);
            PlayerPrefs.SetInt("Grocery_Density", (int)inp_Groc_Density.value);
            PlayerPrefs.SetInt("Grocery_TargetAmount", (int)inp_Groc_Target.value);
            PlayerPrefs.SetInt("Grocery_Speed", (int)inp_Groc_Speed.value);

            manager_Simulation.OnEnableSimulation_DevOnly_Grocery();
        }
    }
}

