using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SepShelfCol : MonoBehaviour
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


    public UnityEvent stopColision;
    public GameObject redDot;

    private void Start()
    {
        if (stopColision == null)
        {
            stopColision = new UnityEvent();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COL_Stay");
        if (collision.gameObject.tag == "Box_Target")
        {
            Debug.Log("COL");
            redDot.SetActive(true);
            stopColision.Invoke();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("COL_Stay");
        if (collision.gameObject.tag == "Box_Target")
        {
            Debug.Log("COL");
            redDot.SetActive(true);
            stopColision.Invoke();
        }
    }
}
