using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SepList_ShelfRowHold
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

    public Transform[] row01;
    public Transform[] row02;

    public SepList_ShelfRowHold(Transform[] inputRow01, Transform[] inputRow02)
    {
        row01 = inputRow01;
        row02 = inputRow02;
    }
}

