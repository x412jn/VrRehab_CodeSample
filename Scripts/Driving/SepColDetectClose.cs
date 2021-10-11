using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepColDetectClose : MonoBehaviour
    {
        /*
         * [Level of crucial]:2
         *      0= not important, but don't know if there are any portantial risk if you remove this
         *      1= slightly important, but not the part of its function
         *      2= may slightly affect part of side function if you change anything
         *      3= may affect some of the major function if you change anything
         *      4= may break the entire project if you change anything
         * 
         */

        public SepSceneSpawner sceneSpawner;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Road_Close")
            {
                Destroy(other.gameObject);
                //Debug.Log("Road close detected: " + other.gameObject.GetInstanceID());
            }
            if (other.tag == "Tunnel_Close")
            {
                Destroy(other.gameObject);
                //Debug.Log("Tunnel close detected: " + other.gameObject.GetInstanceID());
            }
            
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.tag=="Road_Close")
            {
                Destroy(other.gameObject);
                //Debug.Log("Tunnel close detected: " + other.gameObject.GetInstanceID());
            }
            if (other.tag == "Tunnel_Close")
            {
                Destroy(other.gameObject);
                //Debug.Log("Tunnel close detected: " + other.gameObject.GetInstanceID());
            }
            
        }
    }
}

