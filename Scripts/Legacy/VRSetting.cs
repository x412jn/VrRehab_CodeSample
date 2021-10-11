﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRSetting : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        XRSettings.eyeTextureResolutionScale = 1.4f;
    }


}
