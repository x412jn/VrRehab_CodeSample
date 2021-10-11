using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepCarEngine_Dejavu : MonoBehaviour
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


        public SepSceneSpawner sceneSpawner;
        public Transform dejavuPath_Container;
        private List<Transform> nodes;
        public float maxSteerAngle = 45f;
        public float speed = 50f;

        public WheelCollider wheelFL;
        public WheelCollider wheelFR;

        private int currentNode = 0;

        // Start is called before the first frame update
        void Start()
        {
            nodes = new List<Transform>();
            DejavuPath();

        }

        void DejavuPath()
        {
            Transform[] pathTransforms = dejavuPath_Container.GetComponentsInChildren<Transform>();
            for (int i = 0; i < pathTransforms.Length; i++)
            {
                if (pathTransforms[i] != dejavuPath_Container.transform)
                {
                    nodes.Add(pathTransforms[i]);
                    //Debug.Log("transform is " + pathTransforms[i].position);
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //ApplySteer();
            ApplySteer_Dejavu();
            Drive();
            //CheckWayPointDistance();
            CheckWayPointDistance_Dejavu();
        }
        private void ApplySteer()
        {
            Vector3 relativeVector = transform.InverseTransformPoint(sceneSpawner.pathNodes[currentNode].position);
            //relativeVector /= relativeVector.magnitude;
            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
            wheelFL.steerAngle = newSteer;
            wheelFR.steerAngle = newSteer;
        }

        private void ApplySteer_Dejavu()
        {
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
            wheelFL.steerAngle = newSteer;
            wheelFR.steerAngle = newSteer;
        }

        private void Drive()
        {
            wheelFL.motorTorque = speed;
            wheelFR.motorTorque = speed;
        }

        private void CheckWayPointDistance()
        {
            if (Vector3.Distance(transform.position, sceneSpawner.pathNodes[currentNode].position) < 10f)
            {
                if (currentNode == sceneSpawner.pathNodes.Count - 1)
                {
                    speed = 0;
                }
                else
                {
                    currentNode++;
                }

            }
        }

        private void CheckWayPointDistance_Dejavu()
        {
            if (Vector3.Distance(transform.position, nodes[currentNode].position) < 10f)
            {
                if (currentNode == nodes.Count - 1)
                {
                    speed = 0;
                }
                else
                {
                    currentNode++;
                }

            }
        }
    }
}

