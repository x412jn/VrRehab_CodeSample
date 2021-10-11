using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{

    public class SepBoxInteraction : MonoBehaviour
    {
        /*
         * [Level of crucial]:3
         *      0= not important, but don't know if there are any portantial risk if you remove this
         *      1= slightly important, but not the part of its function
         *      2= may slightly affect part of side function if you change anything
         *      3= may affect some of the major function if you change anything
         *      4= may break the entire project if you change anything
         * 
         */


        [HideInInspector] public SepShelf sepShelf;
        public void OnInteract()
        {
            if (this.tag == "Box_Target")
            {
                this.tag = "Box_Normal";
                Debug.Log("THIS OBJECT HAVE BEEN TRIGGERED");
                sepShelf.CallHint();
                sepShelf._taskCount--;
            }
        }
    }
}

