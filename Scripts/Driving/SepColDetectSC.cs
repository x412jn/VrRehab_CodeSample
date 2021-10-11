using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{

    public class SepColDetectSC : MonoBehaviour
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


        [Header("Core")]
        public SepSceneSpawner sceneSpawner;

        //0=countryside
        //1=tunnel
        int currentCheck = 0;
        int lastCheck = 0;
        bool onFirstTwoCheck = false;
        int checkCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            sceneSpawner = GameObject.Find("Spawner").GetComponent<SepSceneSpawner>();
        }


        private void OnTriggerEnter(Collider other)
        {
            //check 
            if (other.tag == "SC_Road_Open")
            {
                currentCheck = 0;
                sceneSpawner.onTunnel = false;
                other.tag = "SC_Road_Close";
            }
            else if (other.tag == "SC_Tunnel_Open")
            {
                currentCheck = 1;
                sceneSpawner.onTunnel = true;
                other.tag = "SC_Tunnel_Close";
            }

            //to check if we met the second check
            checkCount++;
            if (checkCount >= 1)
            {
                onFirstTwoCheck = true;
            }

            if (onFirstTwoCheck)
            {
                //call emergency change bgs
                if (lastCheck != currentCheck)
                {
                    sceneSpawner.callEmergencyBgsChange = true;
                }
            }

            lastCheck = currentCheck;
        }

    }
}

