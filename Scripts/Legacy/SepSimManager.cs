using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BCCH_VR_Therapy
{
    public class SepSimManager : MonoBehaviour
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

        [SerializeField] SepJsonListener jsonListener;

        // Start is called before the first frame update
        void Start()
        {
            //when get into the program, check if disabled is true, if not, turn it into true
            if (!jsonListener.config.disabled && SceneManager.GetActiveScene().name == "SepStartScene")
            {
                jsonListener.config.disabled = true;
                jsonListener.WriteJson();
            }
        }

        // Update is called once per frame
        void Update()
        {
            OnJsonMonitor();
        }

        void OnJsonMonitor()
        {
            //listening to json, if enable is true, turn it into false, and disable "disabled", and load scene
            if (jsonListener.config.enabled)
            {
                jsonListener.config.enabled = false;
                jsonListener.config.disabled = false;
                jsonListener.WriteJson();

                //LOAD TO TARGET SCENE
                SceneManager.LoadScene("SepDrivingScene");
            }
            if(jsonListener.config.disabled && SceneManager.GetActiveScene().name!= "SepStartScene")
            {
                //EXPORT CURRENT LOG

                //SET ENABLE=FALSE
                jsonListener.config.enabled = false;
                jsonListener.WriteJson();

                //GET TO START SCENE
                SceneManager.LoadScene("SepStartScene");
            }
        }

        public void OnDisableSimulation()
        {
            jsonListener.config.disabled = true;
            jsonListener.WriteJson();
        }

        public void OnEnableSimulation_DevOnly()
        {
            jsonListener.config.enabled = true;
            jsonListener.WriteJson();
        }
    }
}

