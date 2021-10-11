using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepCarEngine : MonoBehaviour
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

        [Header("[CORE]")]
        //private SepConfigHolder configHolder;
        public SepSceneSpawner sceneSpawner;
        public Transform dejavuPath_Container;
        private List<Transform> nodes;

        [Space]
        [Header("Parameters")]
        public float maxSteerAngle = 45f;
        public float maxmaxSpeedKmPerH_preset = 60f;
        private float _maxSpeedKmPerH = 60f;
        public float torsion_preset = 100f;
        private float _torsion = 50f;
        [SerializeField] Rigidbody thisRigidbody;
        private float speedKmPerH;

        [Space]
        [Header("Wheels")]
        public WheelCollider wheelFL;
        public WheelCollider wheelFR;

        [Space]
        [Header("Collider checker")]
        public Collider collider_Open;
        public Collider collider_Close;

        [Space]
        [Header("Speed indicator")]
        private int currentNode = 0;
        private int currentNodeTempCount = 0;

        public UIManager_Hand uiManager_Hand;

        // Start is called before the first frame update
        void Awake()
        {
            //configHolder = GameObject.Find("ConfigHolder").GetComponent<SepConfigHolder>();
            _maxSpeedKmPerH = PlayerPrefs.GetFloat("Driving_MaximumSpeed", maxmaxSpeedKmPerH_preset);
            _torsion = PlayerPrefs.GetFloat("Driving_Torsion", torsion_preset);

            nodes = new List<Transform>();
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            ApplySteer();
            Drive();
            CheckWayPointDistance();
            GetSpeedKmPerHour();
        }
        private void ApplySteer()
        {
            Vector3 relativeVector = transform.InverseTransformPoint(sceneSpawner.pathNodes[currentNode].position);
            //relativeVector /= relativeVector.magnitude;
            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
            wheelFL.steerAngle = newSteer;
            wheelFR.steerAngle = newSteer;
        }


        private void Drive()
        {
            if (speedKmPerH <= _maxSpeedKmPerH)
            {
                wheelFL.motorTorque = _torsion;
                wheelFR.motorTorque = _torsion;
            }
            else
            {
                wheelFL.motorTorque = 0;
                wheelFR.motorTorque = 0;
            }
        }

        private void CheckWayPointDistance()
        {
            if (Vector3.Distance(transform.position, sceneSpawner.pathNodes[currentNode].position) < 10f)
            {
                if (currentNode == sceneSpawner.pathNodes.Count - 1)
                {
                    _torsion = 0;
                }
                else
                {
                    currentNode++;
                    currentNodeTempCount++;
                    if (currentNodeTempCount >= 100)
                    {
                        sceneSpawner.SpawnPrefab
                            (sceneSpawner.currentPathEnd,
                            100,
                            sceneSpawner._randomCheckAmount_Curve);
                        currentNodeTempCount = 0;
                    }
                }

            }
        }



        private void GetSpeedKmPerHour()
        {
            speedKmPerH = thisRigidbody.velocity.magnitude * 3.6f;
            uiManager_Hand.speedKmPerh = speedKmPerH;
        }

    }
}

