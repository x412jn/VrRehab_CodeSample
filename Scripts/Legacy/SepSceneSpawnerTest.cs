using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepSceneSpawnerTest : MonoBehaviour
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

        private bool onSimulationStart = false;

        //PREFABS
        [Space]
        [Header("Prefabs")]
        public GameObject[] prefabsCountryside_straight;
        public GameObject[] prefabsCountryside_curve;
        public GameObject[] prefabTunnel_straight;
        public GameObject[] prefabTunnel_curve;

        [Space]
        [Header("Spawn Mode Perameters")]
        public bool enableTunnel;
        //one road equals 35 meters(approximatly)
        public int lenght_Tunnel = 20;
        public int length_Countryside = 20;


        private int currentSpawnRemain;
        private GameObject[] spawnModeTarget_straight;
        private GameObject[] spawnModeTarget_curve;
        private bool onSpawningTunnel;


        [Space]
        [Header("Other Parameters")]
        /// <summary>
        /// get random number, if the number lower than randomCheckAmount, then it is failed,
        /// curve only spawn when it pass the check
        /// </summary>
        public int randomCheckAmount_Curve = 0;
        public int BG_SpawnTimes = 50;
        public int InitialSceneSpawnTimes = 500;

        [Space]
        [Header("tracking anchor")]
        public Transform BG_Anchor;

        [Space]
        [Header("Terrain Perameters")]
        public GameObject prefab_terrain;
        public Transform terrain_anchor;
        delegate void TerrainCheck();
        TerrainCheck terrainCheck;
        //private UnityAction action_checkTerrain;
        //public Vector3 playerPosition;
        //private float dimension_upRight;
        //private float dimension_upLeft;
        //private float dimension_downRight;
        //private float dimension_downLeft;
        //private float pos_x;
        //private float pos_z;
        private List<GameObject> existedTerrainHolder;
        private List<GameObject> tunnelsForChecking;
        private List<int[]> coordinationCheck;

        public int terrain_Length = 80;
        public int terrain_Width = 40;


        [Space]
        [Header("DON'T TOUCH ANYTHING BELOW!")]
        public List<GameObject> existedObjectHolder;
        private Transform startPoint;
        private Transform endPoint;
        public List<Transform> pathNodes;
        /// <summary>
        /// to tell engine where to spawn new prefabs
        /// </summary>
        public Transform currentPathEnd;


        private GameObject tempObject;
        private GameObject tempObjectBackward;

        private int debugInt = 0;


        // Start is called before the first frame update
        void Start()
        {
            //configHolder = GameObject.Find("ConfigHolder").GetComponent<SepConfigHolder>();

            //_randomCheckAmount_Curve = configHolder.driving_Turns;

            pathNodes = new List<Transform>();
            existedObjectHolder = new List<GameObject>();
            existedTerrainHolder = new List<GameObject>();
            tunnelsForChecking = new List<GameObject>();
            coordinationCheck = new List<int[]>();

            if (enableTunnel)
            {
                currentSpawnRemain = length_Countryside;
                onSpawningTunnel = false;
            }

            SpawnPrefab(this.transform, InitialSceneSpawnTimes, randomCheckAmount_Curve);
            spawnBG(BG_Anchor, BG_SpawnTimes);

            SpawnTerrainStatic(terrain_Length, terrain_Width);

        }


        public void SpawnModeCheck()
        {
            //spawnModeTarget_straight = prefabTunnel_straight;
            //spawnModeTarget_curve = prefabTunnel_curve;
            if (enableTunnel)
            {
                if (onSpawningTunnel)
                {
                    spawnModeTarget_straight = prefabTunnel_straight;
                    spawnModeTarget_curve = prefabTunnel_curve;
                }
                else
                {
                    spawnModeTarget_straight = prefabsCountryside_straight;
                    spawnModeTarget_curve = prefabsCountryside_curve;
                }
                currentSpawnRemain--;
                if (currentSpawnRemain <= 0)
                {
                    onSpawningTunnel = !onSpawningTunnel;
                    if (onSpawningTunnel)
                    {
                        currentSpawnRemain = lenght_Tunnel;
                    }
                    else
                    {
                        currentSpawnRemain = length_Countryside;
                    }
                }
            }
            else
            {
                onSimulationStart = true;
                spawnModeTarget_straight = prefabsCountryside_straight;
                spawnModeTarget_curve = prefabsCountryside_curve;
            }
        }

        public void spawnBG(Transform spawningPos, int spawnTimes)
        {
            int arrayRnd;
            for (int i = 0; i < spawnTimes; i++)
            {
                SpawnModeCheck();
                arrayRnd = Random.Range(0, spawnModeTarget_straight.Length - 1);
                GameObject spawningObject_target = spawnModeTarget_straight[arrayRnd];
                tempObjectBackward = Instantiate(spawningObject_target, spawningPos);
                tempObjectBackward.transform.parent = BG_Anchor;
                existedObjectHolder.Add(tempObjectBackward);
                spawningPos = tempObjectBackward.transform.Find("EndPoint").transform;

            }
        }

        public void SpawnPrefab
            (Transform spawningPos, int spawnTimes, int rndMax)
        {
            int arrayRnd_object01;
            //int arrayRnd_object02;
            int frequencyRnd;
            int direction = 0;
            GameObject spawningObject_target;
            for (int i = 0; i < spawnTimes; i++)
            {
                SpawnModeCheck();
                frequencyRnd = Random.Range(0, 100);
                if (frequencyRnd > randomCheckAmount_Curve)
                {
                    arrayRnd_object01 = Random.Range(0, spawnModeTarget_straight.Length - 1);
                    spawningObject_target = spawnModeTarget_straight[arrayRnd_object01];
                    tempObject = Instantiate(spawningObject_target, spawningPos);

                    tunnelsForChecking.Add(tempObject);
                    terrainCheck += checkTerrain;
                    //insert list

                    tempObject.transform.parent = this.transform;
                    existedObjectHolder.Add(tempObject);
                    spawningPos = tempObject.transform.Find("EndPoint").transform;

                    //startPoint = tempObject.transform.Find("WayPoints").Find("wp1");
                    //pathNodes.Add(startPoint);
                    GetWaypoints(tempObject.transform.Find("WayPoints"));

                    HoldCleaner();
                }
                else
                {
                    //This whole chunk of codes is for spawning curves, 
                    //and direction correction is preventing curve from turning backwards

                    if (direction == 0)
                    {
                        direction = 1;
                    }
                    else if (direction == 1)
                    {
                        direction = 0;
                    }

                    spawningObject_target = spawnModeTarget_curve[direction];
                    tempObject = Instantiate(spawningObject_target, spawningPos);

                    tunnelsForChecking.Add(tempObject);
                    terrainCheck += checkTerrain;
                    //insert list

                    tempObject.transform.parent = this.transform;
                    existedObjectHolder.Add(tempObject);
                    spawningPos = tempObject.transform.Find("EndPoint").transform;

                    //startPoint = tempObject.transform.Find("WayPoints").Find("wp1");
                    //pathNodes.Add(startPoint);
                    GetWaypoints(tempObject.transform.Find("WayPoints"));

                    HoldCleaner();
                }
                currentPathEnd = spawningPos;

            }

            //proceed ienumerator
            //StartCoroutine("checkTerrain");
            

            if (terrainCheck != null)
            {
                //terrainCheck();
            }

        }

        void SpawnTerrainStatic(int length, int width)
        {
            int cor_x;
            int cor_z;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    cor_x = x * 1000;
                    cor_z = z * 1000;
                    terrain_anchor.position = new Vector3(cor_x, -1, cor_z);
                    Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                }
            }
            //for (int x = 1; x > -width; x--)
            //{
            //    for (int z = 0; z < length; z++)
            //    {
            //        cor_x = x * 1000;
            //        cor_z = z * 1000;
            //        terrain_anchor.position = new Vector3(cor_x, -1, cor_z);
            //        Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
            //    }
            //}
        }

        void checkTerrain()
        {
            GameObject inspectingObject;
            if (tunnelsForChecking[0] != null)
            {
                inspectingObject = tunnelsForChecking[0];

                Vector3 origin = inspectingObject.transform.position;
                Vector3 direction = inspectingObject.transform.up * -1.0f;

                //Debug.Log("checking " + inspectingObject.GetInstanceID() + "'s terrain situation");
                Ray ray = new Ray(origin, direction);

                if (!Physics.Raycast(ray, 100f, 8))
                {
                    //Debug.Log("Terrain Not Found under " + inspectingObject.GetInstanceID());
                    //SpawnTerrain((int)inspectingObject.transform.position.x, (int)inspectingObject.transform.position.z);

                    //==================================================================
                    int x = (int)inspectingObject.transform.position.x;
                    int z = (int)inspectingObject.transform.position.z;

                    GameObject tempTerrainObj;
                    //get point zero
                    x /= 1000;
                    z /= 1000;

                    //define coordination
                    int[] cor = new int[] { x * 1000, z * 1000 };

                    int[] corUp = new int[] { x * 1000, (z + 1) * 1000 };
                    int[] corDown = new int[] { x * 1000, (z - 1) * 1000 };
                    int[] corLeft = new int[] { (x - 1) * 1000, z * 1000 };
                    int[] corRight = new int[] { (x + 1) * 1000, z * 1000 };

                    //check if coordination check is null
                    if (coordinationCheck != null)
                    {
                        //check if there is overlaps
                        for (int i = 0; i < coordinationCheck.Count; i++)
                        {
                            if (cor == coordinationCheck[i])
                            {
                                cor = null;
                            }
                            if (corUp == coordinationCheck[i])
                            {
                                corUp = null;
                            }
                            if (corDown == coordinationCheck[i])
                            {
                                corDown = null;
                            }
                            if (corLeft == coordinationCheck[i])
                            {
                                corLeft = null;
                            }
                            if (corRight == coordinationCheck[i])
                            {
                                corRight = null;
                            }
                        }
                        if (cor != null)
                        {
                            coordinationCheck.Add(cor);

                            terrain_anchor.position = new Vector3(cor[0], -1, cor[1]);
                            //spawn terrain to new position
                            tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                            tempTerrainObj.name = debugInt.ToString();
                            debugInt++;
                            existedTerrainHolder.Add(tempTerrainObj);
                        }
                        if (corUp != null)
                        {
                            coordinationCheck.Add(corUp);

                            terrain_anchor.position = new Vector3(corUp[0], -1, corUp[1]);
                            //spawn terrain to new position
                            tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                            tempTerrainObj.name = debugInt.ToString();
                            debugInt++;
                            existedTerrainHolder.Add(tempTerrainObj);
                        }
                        if (corDown != null)
                        {
                            coordinationCheck.Add(corDown);

                            terrain_anchor.position = new Vector3(corDown[0], -1, corDown[1]);
                            //spawn terrain to new position
                            tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                            tempTerrainObj.name = debugInt.ToString();
                            debugInt++;
                            existedTerrainHolder.Add(tempTerrainObj);
                        }
                        if (corLeft != null)
                        {
                            coordinationCheck.Add(corLeft);

                            terrain_anchor.position = new Vector3(corLeft[0], -1, corLeft[1]);
                            //spawn terrain to new position
                            tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                            tempTerrainObj.name = debugInt.ToString();
                            debugInt++;
                            existedTerrainHolder.Add(tempTerrainObj);
                        }
                        if (corRight != null)
                        {
                            coordinationCheck.Add(corRight);

                            terrain_anchor.position = new Vector3(corRight[0], -1, corRight[1]);
                            //spawn terrain to new position
                            tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                            tempTerrainObj.name = debugInt.ToString();
                            debugInt++;
                            existedTerrainHolder.Add(tempTerrainObj);
                        }
                    }
                    else
                    {
                        coordinationCheck.Add(cor);
                        terrain_anchor.position = new Vector3(cor[0], -1, cor[1]);
                        //spawn terrain to new position
                        tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                        tempTerrainObj.name = debugInt.ToString();
                        debugInt++;
                        existedTerrainHolder.Add(tempTerrainObj);

                        coordinationCheck.Add(corUp);
                        terrain_anchor.position = new Vector3(corUp[0], -1, corUp[1]);
                        //spawn terrain to new position
                        tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                        tempTerrainObj.name = debugInt.ToString();
                        debugInt++;
                        existedTerrainHolder.Add(tempTerrainObj);

                        coordinationCheck.Add(corDown);
                        terrain_anchor.position = new Vector3(corDown[0], -1, corDown[1]);
                        //spawn terrain to new position
                        tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                        tempTerrainObj.name = debugInt.ToString();
                        debugInt++;
                        existedTerrainHolder.Add(tempTerrainObj);

                        coordinationCheck.Add(corLeft);
                        terrain_anchor.position = new Vector3(corLeft[0], -1, corLeft[1]);
                        //spawn terrain to new position
                        tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                        tempTerrainObj.name = debugInt.ToString();
                        debugInt++;
                        existedTerrainHolder.Add(tempTerrainObj);

                        coordinationCheck.Add(corRight);
                        terrain_anchor.position = new Vector3(corRight[0], -1, corRight[1]);
                        //spawn terrain to new position
                        tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                        tempTerrainObj.name = debugInt.ToString();
                        debugInt++;
                        existedTerrainHolder.Add(tempTerrainObj);
                    }
                    //======================================================================

                }
                tunnelsForChecking.RemoveAt(0);
            }
        }

        //IEnumerator checkTerrain()
        //{
        //    bool onSpawning = true;
        //    GameObject inspectingObject;
        //    while (onSpawning)
        //    {
        //        if (tunnelsForChecking[0] != null)
        //        {
        //            inspectingObject = tunnelsForChecking[0];

        //            Vector3 origin = inspectingObject.transform.position;
        //            Vector3 direction = inspectingObject.transform.up * -1.0f;

        //            Debug.Log("checking " + inspectingObject.name + "'s terrain situation");
        //            Ray ray = new Ray(origin, direction);

        //            if (!Physics.Raycast(ray, 10f, 8))
        //            {
        //                Debug.Log("Terrain Not Found under " + inspectingObject.name);
        //                SpawnTerrain((int)inspectingObject.transform.position.x, (int)inspectingObject.transform.position.z);
        //            }
        //            tunnelsForChecking.RemoveAt(0);
        //            yield return new WaitForEndOfFrame();
        //        }
        //        else
        //        {
        //            onSpawning = false;
        //        }
        //    }

        //}

        //void SpawnTerrain_Clean(int x, int z)
        //{
        //    GameObject tempTerrainObj;
        //    x /= 1000;
        //    z /= 1000;
        //    x *= 1000;
        //    z *= 1000;

        //    terrain_anchor.position = new Vector3(x, -1, z);

        //    //spawn terrain to new position
        //    tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
        //    existedTerrainHolder.Add(tempTerrainObj);
        //}

        void SpawnTerrain(int x, int z)
        {
            GameObject tempTerrainObj;
            //get point zero
            x /= 1000;
            z /= 1000;

            //define coordination
            int[] cor = new int[] { x * 1000, z * 1000 };
            int[] corUp = new int[] { x * 1000, (z + 1) * 1000 };
            int[] corDown = new int[] { x * 1000, (z - 1) * 1000 };
            int[] corLeft = new int[] { (x - 1) * 1000, z * 1000 };
            int[] corRight = new int[] { (x + 1) * 1000, z * 1000 };

            //check if coordination check is null
            if (coordinationCheck != null)
            {
                //check if there is overlaps
                for (int i = 0; i < coordinationCheck.Count; i++)
                {
                    if (cor == coordinationCheck[i])
                    {
                        cor = null;
                    }
                    if (corUp == coordinationCheck[i])
                    {
                        corUp = null;
                    }
                    if (corDown == coordinationCheck[i])
                    {
                        corDown = null;
                    }
                    if (corLeft == coordinationCheck[i])
                    {
                        corLeft = null;
                    }
                    if (corRight == coordinationCheck[i])
                    {
                        corRight = null;
                    }
                }
                if (cor != null)
                {
                    coordinationCheck.Add(cor);

                    terrain_anchor.position = new Vector3(cor[0], -1, cor[1]);
                    //spawn terrain to new position
                    tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                    existedTerrainHolder.Add(tempTerrainObj);
                }
                if (corUp != null)
                {
                    coordinationCheck.Add(corUp);

                    terrain_anchor.position = new Vector3(corUp[0], -1, corUp[1]);
                    //spawn terrain to new position
                    tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                    existedTerrainHolder.Add(tempTerrainObj);
                }
                if (corDown != null)
                {
                    coordinationCheck.Add(corDown);

                    terrain_anchor.position = new Vector3(corDown[0], -1, corDown[1]);
                    //spawn terrain to new position
                    tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                    existedTerrainHolder.Add(tempTerrainObj);
                }
                if (corLeft != null)
                {
                    coordinationCheck.Add(corLeft);

                    terrain_anchor.position = new Vector3(corLeft[0], -1, corLeft[1]);
                    //spawn terrain to new position
                    tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                    existedTerrainHolder.Add(tempTerrainObj);
                }
                if (corRight != null)
                {
                    coordinationCheck.Add(corRight);

                    terrain_anchor.position = new Vector3(corRight[0], -1, corRight[1]);
                    //spawn terrain to new position
                    tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                    existedTerrainHolder.Add(tempTerrainObj);
                }
            }
            else
            {
                coordinationCheck.Add(cor);
                terrain_anchor.position = new Vector3(cor[0], -1, cor[1]);
                //spawn terrain to new position
                tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                existedTerrainHolder.Add(tempTerrainObj);

                coordinationCheck.Add(corUp);
                terrain_anchor.position = new Vector3(corUp[0], -1, corUp[1]);
                //spawn terrain to new position
                tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                existedTerrainHolder.Add(tempTerrainObj);

                coordinationCheck.Add(corDown);
                terrain_anchor.position = new Vector3(corDown[0], -1, corDown[1]);
                //spawn terrain to new position
                tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                existedTerrainHolder.Add(tempTerrainObj);

                coordinationCheck.Add(corLeft);
                terrain_anchor.position = new Vector3(corLeft[0], -1, corLeft[1]);
                //spawn terrain to new position
                tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                existedTerrainHolder.Add(tempTerrainObj);

                coordinationCheck.Add(corRight);
                terrain_anchor.position = new Vector3(corRight[0], -1, corRight[1]);
                //spawn terrain to new position
                tempTerrainObj = Instantiate(prefab_terrain, terrain_anchor.position, terrain_anchor.rotation);
                existedTerrainHolder.Add(tempTerrainObj);
            }
        }


        void GetWaypoints(Transform waypointParent)
        {
            //List<Transform> tempWaypointHolder = new List<Transform>();
            foreach (Transform waypoints in waypointParent)
            {
                //tempWaypointHolder.Add(waypoints);
                pathNodes.Add(waypoints);
            }
        }



        public void HoldCleaner()
        {
            if (existedObjectHolder.Count > 1000)
            {
                for (int i = 0; i < 500; i++)
                {
                    //Debug.Log("on deleting " + existedObjectHolder[0].name + "transform is " + existedObjectHolder[0].transform.position);
                    Destroy(existedObjectHolder[0]);
                    existedObjectHolder.RemoveAt(0);
                    //Debug.Log("remove time:" + i);
                }
                Debug.Log("current object list 0 is " + existedObjectHolder[0].name + "transform is " + existedObjectHolder[0].transform.position);
            }

            if (existedTerrainHolder.Count > 20)
            {
                for (int i = 0; i < 5; i++)
                {
                    Destroy(existedTerrainHolder[0]);
                    existedTerrainHolder.RemoveAt(0);
                }
            }
        }
    }
}

