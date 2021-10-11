using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepGroceryHintDisplayer : MonoBehaviour
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


        public GameObject hint;

        private void Start()
        {
            hint = GameObject.Find("XR Rig").GetComponent<Transform>().Find("Camera Offset").Find("Main Camera").Find("Canvas").Find("Image (3)").gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            hint.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            hint.SetActive(false);
        }
    }
}

