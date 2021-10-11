using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BCCH_VR_Therapy
{
    public class SepSimManagerNoJson : MonoBehaviour
    {
        /*
         * [Level of crucial]:1
         *      0= not important, but don't know if there are any portantial risk if you remove this
         *      1= slightly important, but not the part of its function
         *      2= may slightly affect part of side function if you change anything
         *      3= may affect some of the major function if you change anything
         *      4= may break the entire project if you change anything
         * 
         */


        //[Header("DEPENDENCY REMINDER,DONT DROP ANYTHING TO IT")]
        //public SepConfigHolder configHolder;

        private void Awake()
        {
            //configHolder = GameObject.Find("ConfigHolder").GetComponent<SepConfigHolder>();
        }

        public void OnDisableSimulation()
        {
            //configHolder.ResetEverythingToZero();

            //GET TO START SCENE
            SceneManager.LoadScene("SepStartScene");
        }

        public void OnEnableSimulation_DevOnly()
        {
            //LOAD TO TARGET SCENE
            SceneManager.LoadScene("SepDrivingScene");
        }

        public void OnEnableSimulation_DevOnly_Grocery()
        {
            SceneManager.LoadScene("SepGrocery");
        }
    }
}

