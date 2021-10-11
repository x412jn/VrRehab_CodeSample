using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class MajorCarPath : MonoBehaviour
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

        public Color lineColor;

        private List<Transform> nodes = new List<Transform>();

        private void OnDrawGizmos()
        {
            Gizmos.color = lineColor;

            Transform[] pathTransforms = GetComponentsInChildren<Transform>();
            nodes = new List<Transform>();

            for (int i = 0; i < pathTransforms.Length; i++)
            {
                if (pathTransforms[i] != transform)
                {
                    nodes.Add(pathTransforms[i]);
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 currentNode = nodes[i].position;
                Vector3 previousNode = Vector3.zero;

                if (i > 0)
                {
                    previousNode = nodes[i - 1].position;
                }
                else if (i == 0 && nodes.Count > 1)
                {
                    previousNode = nodes[nodes.Count - 1].position;
                }

                Gizmos.DrawLine(previousNode, currentNode);
                Gizmos.DrawSphere(currentNode, 1.0f);
            }
        }
    }
}

