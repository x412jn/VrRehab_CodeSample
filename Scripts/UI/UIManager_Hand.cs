using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

namespace BCCH_VR_Therapy
{
    public class UIManager_Hand : MonoBehaviour
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

        [Header("[[Right Hand Properties]]")]
        public float speedKmPerh = 0;
        public GameObject speedWindow;
        public TextMeshProUGUI speedText;

        

        private void Update()
        {
            OnUpdateSpeedText();
        }

        //==========================
        //FOR DRIVING SCENE
        //==========================

        public void OnEnableSpeedWindow()
        {
            speedWindow.SetActive(true);
        }

        public void OnDisableSpeedWindow()
        {
            speedWindow.SetActive(false);
        }

        private void OnUpdateSpeedText()
        {
            string speedTextOutput = speedKmPerh.ToString() + " Km/h";
            speedText.text = speedTextOutput;
        }

    }
}

