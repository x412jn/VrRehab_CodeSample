using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAFix : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Fix");
    }

    IEnumerator Fix()
    {
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 0.5f;     //any value
        yield return new WaitForEndOfFrame();                           //needeed to apply the changes
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 1f;       //target value
        Destroy(this);

    }
}
