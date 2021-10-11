using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepSymptomFeedback : MonoBehaviour
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

        public GameObject symptomVisualCue;
        public Transform VisualCueAnchor;
        bool onCD = false;

        private void Update()
        {
            if (!onCD)
            {
                onCD = true;
                Instantiate(symptomVisualCue, VisualCueAnchor);
                StartCoroutine("DisplaySymptomCD");
            }
        }
        IEnumerator DisplaySymptomCD()
        {
            yield return new WaitForSeconds(60f);
            onCD = false;
        }
    }
}

