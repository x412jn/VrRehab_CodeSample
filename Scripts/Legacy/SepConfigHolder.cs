using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepConfigHolder : MonoBehaviour
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

        public bool driving_EnableTunnel = true;

        public float driving_MaximumSpeed
        {
            get { return _driving_MaximumSpeed; }
            set { _driving_MaximumSpeed = Mathf.Clamp(value, 0, 200f); }
        }
        private float _driving_MaximumSpeed = 0f;

        public int driving_Turns
        {
            get { return _driving_Turns; }
            set { _driving_Turns = Mathf.Clamp(value, 0, 100); }
        }
        private int _driving_Turns = 0;

        /// <summary>
        /// 1=low|2=mideum|3=high
        /// </summary>
        public int driving_Bumps
        {
            get { return _driving_Tunnel_DrastLevel; }
            set { _driving_Tunnel_DrastLevel = Mathf.Clamp(value, 1, 3); }
        }
        private int _driving_Tunnel_DrastLevel = 1;

        
        public float driving_Torsion
        {
            get { return _driving_Torsion; }
            set { _driving_Torsion = Mathf.Clamp(value, 0, 200f); }
        }
        private float _driving_Torsion = 0f;

        /// <summary>
        /// 0=orange|1=yellow|2=white
        /// </summary>
        public int driving_Tunnel_Light_Color = 0;

        public int driving_ContrySide_TreesDensity
        {
            get { return _driving_ContrySide_TreesDensity; }
            set { _driving_ContrySide_TreesDensity = Mathf.Clamp(value, 1, 3); }
        }
        private int _driving_ContrySide_TreesDensity = 1;

        public int driving_Traffic
        {
            get { return _driving_Traffic; }
            set { _driving_Traffic = Mathf.Clamp(value, 1, 3); }
        }
        private int _driving_Traffic = 1;

        public int driving_Tunnel_RoadSign
        {
            get { return _driving_Tunnel_RoadSign; }
            set { _driving_Tunnel_RoadSign = Mathf.Clamp(value, 1, 3); }
        }
        private int _driving_Tunnel_RoadSign = 1;


        public int driving_Length_CountrySide
        {
            get { return _driving_ContrySide_TreesDensity; }
            set { _driving_ContrySide_TreesDensity = Mathf.Clamp(value, 20, 40); }
        }
        private int _driving_Length_CountrySide = 20;

        public int driving_Length_Tunnel
        {
            get { return _driving_Length_Tunnel; }
            set { _driving_Length_Tunnel = Mathf.Clamp(value, 20, 40); }
        }
        private int _driving_Length_Tunnel = 20;
        //======================================================

        public int grocery_density
        {
            get { return _grocery_density; }
            set { _grocery_density = Mathf.Clamp(value, 1, 3); }
        }
        private int _grocery_density = 1;

        public int grocery_contrast
        {
            get { return _grocery_contrast; }
            set { _grocery_contrast = Mathf.Clamp(value, 1, 3); }
        }
        private int _grocery_contrast = 1;

        private void Awake()
        {
            //if (GameObject.FindGameObjectsWithTag("enabledConfigHolder") != null)
            //{
            //    Destroy(this);
            //}
            //else
            //{
            //    this.tag = "enabledConfigHolder";
            //}
            DontDestroyOnLoad(this.gameObject);
        }

        public void ResetEverythingToZero()
        {
            driving_MaximumSpeed = 0;
            driving_Turns = 0;
            driving_Bumps = 1;
            driving_Torsion = 0;
            driving_Tunnel_Light_Color = 0;
        }
    }
}

