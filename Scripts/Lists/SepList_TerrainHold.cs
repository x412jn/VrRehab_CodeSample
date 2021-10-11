using System.Collections;
using System.Collections.Generic;

public class SepList_TerrainHold
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

    public int Terrain_x;
    public int Terrain_z;

    public SepList_TerrainHold(int x,int z)
    {
        Terrain_x = x;
        Terrain_z = z;
    }
}
