using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BCCH_VR_Therapy
{
    public class SepJsonListener : MonoBehaviour
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

        public Config config;
        private string jsonPath = Application.streamingAssetsPath + "../../../../Config/config.json";

        // Start is called before the first frame update
        void Start()
        {
            LoadJson();
        }

        private void Update()
        {
            LoadJson();
        }

        [System.Serializable]
        public class Config
        {
            public bool enabled;
            public bool disabled;
            public int speed;
            public int pavement;
            public int turns;
        }

        private void LoadJson()
        {
            string jsonContents = File.ReadAllText(jsonPath);
            config = JsonUtility.FromJson<Config>(jsonContents);
            //if (config != null)
            //{
            //    Debug.Log("Found config");
            //}
        }

        public void WriteJson()
        {
            string json = JsonUtility.ToJson(config);
            File.WriteAllText(jsonPath, json);
        }
    }
}

