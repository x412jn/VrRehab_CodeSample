using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepColDetectOpen : MonoBehaviour
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

        [Header("Core")]
        public SepSceneSpawner sceneSpawner;
        public Transform carPosition;

        [Space]
        [Header("Prefabs")]
        public GameObject bump_straight_lv1;
        public GameObject bump_straight_lv2;
        public GameObject bump_curve_right_lv1;
        public GameObject bump_curve_right_lv2;
        public GameObject bump_curve_left_lv1;
        public GameObject bump_curve_left_lv2;

        public GameObject tunnel_Pillar;
        public GameObject tunnel_Light_White;
        public GameObject tunnel_Light_Orange;
        public GameObject tunnel_Light_Yellow;
        public GameObject tunnel_Sign01;
        public GameObject tunnel_Sign02;
        public GameObject tunnel_Sign03;

        public GameObject countryside_Trees;
        public GameObject countryside_Building_near;
        public GameObject countryside_Building_middle;
        public GameObject countryside_Building_far;
        public GameObject countryside_Billboard;
        public GameObject countryside_streetPole;

        private Transform targetTransform;
        private Transform[] targetTransforms;
        private GameObject tempObject;

        //terrain
        [Space]
        [Header("Terrain")]
        public GameObject prefab_terrain;
        public Transform terrainAnchor;
        List<SepList_TerrainHold> terrainHold;
        private int terrain_x = 0;
        private int terrain_z = 0;
        List<GameObject> terrainCleaner;

        //PARAMETERS
        [Space]
        [Header("Config")]
        //spawn once per every "frequency var"
        public int frequency_House_Near = 5;
        public int frequency_House_Far = 10;
        public int frequency_Billboard = 10;
        public int frequency_Bumps = 1;
        public int frequency_Tun_Sign01 = 5;
        public int frequency_Tun_Sign02 = 5;

        private int frequency_House_Near_Check = 0;
        private int frequency_House_Far_Check = 0;
        private int frequency_Billboar_Check = 0;
        private int frequency_Bumps_Check = 0;
        private int frequency_Tun_Sign01_Check = 0;
        private int frequency_Tun_Sign02_Check = 0;

        private void Start()
        {
            terrainHold = new List<SepList_TerrainHold>();
            terrainHold.Add(new SepList_TerrainHold(terrain_x, terrain_z));
            terrainCleaner = new List<GameObject>();
            terrainCleaner.Add(GameObject.Find("PreTerrain01"));
            terrainCleaner.Add(GameObject.Find("PreTerrain02"));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Road_Open")
            {
                other.tag = "Road_Close";
                //Debug.Log("road open detected: " + other.gameObject.GetInstanceID());

                //Enable Road
                other.transform.Find("[ROAD]").gameObject.SetActive(true);

                //spawn bumpyness

                //9=Road
                //12=CurveLeft
                //13=CurveRight
                //14=Straight
                switch (sceneSpawner._level_Bumps)
                {
                    case 0:
                        Debug.Log("No Bumps");
                        other.gameObject.layer = 9;
                        break;
                    case 1:
                        frequency_Bumps = 3;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                    case 2:
                        frequency_Bumps = 2;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                    case 3:
                        frequency_Bumps = 1;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                    default:
                        frequency_Bumps = 3;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                }

                //spawn trees according to playerpref
                switch (sceneSpawner._level_Countryside_ObjectComplexity)
                {
                    case 0:
                        //Spawn pole
                        SpawnStreetPole(other);
                        break;

                    case 1:
                        //Spawn pole
                        SpawnStreetPole(other);

                        //Spawn Trees
                        SpawnTreesLowDensity(other);
                        
                        break;
                    case 2:
                        //Spawn pole
                        SpawnStreetPole(other);

                        //Spawn Trees
                        SpawnTreesLowDensity(other);

                        //Spawn Houses
                        SpawnHouse(other);
                        
                        break;

                    case 3:
                        //Spawn pole
                        SpawnStreetPole(other);

                        //spawn trees
                        SpawnTreesHighDensity(other);

                        //spawn Houses
                        SpawnHouse(other);

                        //spawn billboard
                        //SpawnBillboard(other);
                        break;

                    default:
                        //Spawn pole
                        SpawnStreetPole(other);
                        break;
                }
                

                //spawn bumps according to playerpref
            }
            if (other.tag == "Tunnel_Open")
            {
                

                other.tag = "Tunnel_Close";
                //Debug.Log("tunnel open detected: " + other.gameObject.GetInstanceID());

                //Enable Road
                other.transform.Find("[ROAD]").gameObject.SetActive(true);


                

                //spawn bumpyness

                //9=Road
                //12=CurveLeft
                //13=CurveRight
                //14=Straight
                switch (sceneSpawner._level_Bumps)
                {
                    case 0:
                        Debug.Log("No Bumps");
                        other.gameObject.layer = 9;
                        break;
                    case 1:
                        frequency_Bumps = 3;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                    case 2:
                        frequency_Bumps = 2;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                    case 3:
                        frequency_Bumps = 1;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                    default:
                        frequency_Bumps = 3;
                        SpawnBumps(other, frequency_Bumps, other.gameObject.layer);
                        break;
                }

                //spawn sign according to playerpref
                switch (sceneSpawner._level_Countryside_ObjectComplexity)
                {
                    case 0:
                        //spawn pillar
                        SpawnPillar(other);
                        
                        break;

                    case 1:
                        //spawn pillar
                        SpawnPillar(other);

                        //Spawn Sign 1
                        SpawnSign01(other);

                        break;
                    case 2:
                        //spawn pillar
                        SpawnPillar(other);

                        //Spawn Sign 1
                        SpawnSign01(other);

                        //Spawn Sign 2
                        SpawnSign02(other);

                        break;

                    case 3:
                        //spawn pillar
                        SpawnPillar(other);

                        //Spawn Sign 1
                        SpawnSign01(other);

                        //Spawn Sign 2
                        SpawnSign02(other);

                        //Spawn Sign 3
                        SpawnSign03(other);

                        break;

                    default:
                        //spawn pillar
                        SpawnPillar(other);
                        break;
                }

                //spawn light according to playerpref
                targetTransform = other.transform.Find("[LIGHT_LUM]").Find("LightPos");
                switch (sceneSpawner._level_Tunnel_lightColor)
                {
                    case 0:
                        tempObject = Instantiate(tunnel_Light_Orange, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                        //orange
                        break;
                    case 1:
                        tempObject = Instantiate(tunnel_Light_Yellow, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                        //yellow
                        break;
                    case 2:
                        //white
                        tempObject = Instantiate(tunnel_Light_White, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                        break;
                    default:
                        //orange
                        tempObject = Instantiate(tunnel_Light_Orange, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                        break;
                }

                //spawn bumps according to playerpref

            }

            //spawn terrain
            if (other.tag == "Terrain_Open")
            {
                other.tag = "Terrain_Close";
                SpawnTerrain();
            }
        }

        private void SpawnTerrain()
        {
            //get current grid
            terrain_x = (int)carPosition.position.x / 500;
            terrain_z = (int)carPosition.position.z / 500;




            //get target grid pos
            int terrain_x_up = terrain_x;
            int terrain_z_up = terrain_z + 1;
            int terrain_x_right = terrain_x + 1;
            int terrain_z_right = terrain_z;
            int terrain_x_upRight = terrain_x + 1;
            int terrain_z_upRight = terrain_z + 1;
            int terrain_x_left = terrain_x - 1;
            int terrain_z_left = terrain_z;
            int terrain_x_upLeft = terrain_x - 1;
            int terrain_z_upLeft = terrain_z + 1;

            //up
            SpawnTerrainChecked(terrain_x_up, terrain_z_up);
            SpawnTerrainChecked(terrain_x_left, terrain_z_left);
            SpawnTerrainChecked(terrain_x_right, terrain_z_right);
            SpawnTerrainChecked(terrain_x_upLeft, terrain_z_upLeft);
            SpawnTerrainChecked(terrain_x_upRight, terrain_z_upRight);
        }

        private void SpawnTerrainChecked(int input_x,int input_z)
        {
            //check if collide with list
            bool terrainGridCollideCheck = false;
            for (int i = 0; i < terrainHold.Count; i++)
            {
                if (input_x == terrainHold[i].Terrain_x && input_z == terrainHold[i].Terrain_z)
                {
                    terrainGridCollideCheck = true;
                }
            }
            //insert new grid into list and spawn terrain
            if (!terrainGridCollideCheck)
            {
                //Debug.Log("X: " + input_x + " Z: " + input_z);
                terrainHold.Add(new SepList_TerrainHold(input_x, input_z));
                input_x *= 500;
                input_z *= 500;
                terrainAnchor.position = new Vector3(input_x, -0.5f, input_z);
                GameObject tempObject;
                tempObject = Instantiate(prefab_terrain, terrainAnchor.position, terrainAnchor.rotation);
                terrainCleaner.Add(tempObject);

                if (terrainCleaner.Count > 15)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        //Debug.Log("on deleting " + existedObjectHolder[0].name + "transform is " + existedObjectHolder[0].transform.position);
                        Destroy(terrainCleaner[0]);
                        terrainCleaner.RemoveAt(0);
                        //Debug.Log("remove time:" + i);
                    }
                }
            }
        }

        private void SpawnStreetPole(Collider other)
        {
            targetTransforms = 
                other.transform.Find("[DECORATION]").Find("StreetLamps").GetComponentsInChildren<Transform>();
            if (targetTransforms != null)
            {
                for(int i = 1; i < targetTransforms.Length; i++)
                {
                    tempObject = Instantiate(countryside_streetPole, targetTransforms[i].position, targetTransforms[i].rotation);
                    tempObject.transform.parent = targetTransforms[i];
                    if (sceneSpawner._level_TimeOfDay == 3)
                    {
                        tempObject.transform.Find("Cube").gameObject.SetActive(true);
                    }
                    else
                    {
                        tempObject.transform.Find("Cube").gameObject.SetActive(false);
                    }
                }
            }
        }
        private void SpawnTreesLowDensity(Collider other)
        {
            targetTransforms =
                            other.transform.Find("[DECORATION]").Find("Trees_LowDensity").GetComponentsInChildren<Transform>();
            if (targetTransforms != null)
            {
                for (int i = 1; i < targetTransforms.Length; i++)
                {
                    tempObject = Instantiate(countryside_Trees, targetTransforms[i].position, targetTransforms[i].rotation);
                    //randomize trees angle
                    tempObject.transform.eulerAngles = new Vector3(
                        tempObject.transform.eulerAngles.x,
                        tempObject.transform.eulerAngles.y + (float)Random.Range(0, 360),
                        tempObject.transform.eulerAngles.z);
                    tempObject.transform.parent = targetTransforms[i];
                }
            }
        }

        private void SpawnTreesHighDensity(Collider other)
        {
            targetTransforms =
                other.transform.Find("[DECORATION]").Find("Trees_HighDensity").GetComponentsInChildren<Transform>();
            if (targetTransforms != null)
            {
                for (int i = 1; i < targetTransforms.Length; i++)
                {
                    tempObject = Instantiate(countryside_Trees, targetTransforms[i].position, targetTransforms[i].rotation);
                    //randomize trees angle
                    tempObject.transform.eulerAngles = new Vector3(
                        tempObject.transform.eulerAngles.x,
                        tempObject.transform.eulerAngles.y + (float)Random.Range(0, 360),
                        tempObject.transform.eulerAngles.z);
                    tempObject.transform.parent = targetTransforms[i];
                }
            }
        }

        private void SpawnPillar(Collider other)
        {
            targetTransform = other.transform.Find("[DECORATION]").Find("PillarPos");
            tempObject = Instantiate(tunnel_Pillar, targetTransform.position, targetTransform.rotation);
            tempObject.transform.parent = targetTransform;
        }

        private void SpawnSign01(Collider other)
        {
            //check if frequency met
            if (frequency_Tun_Sign01_Check == 0)
            {
                targetTransform = other.transform.Find("[DECORATION]").Find("RoadSigns").Find("RoadSign01");
                if (targetTransform != null)
                {
                    tempObject = Instantiate(tunnel_Sign01, targetTransform.position, targetTransform.rotation);
                    tempObject.transform.parent = targetTransform;
                }
            }

            //add loop
            frequency_Tun_Sign01_Check++;
            if (frequency_Tun_Sign01_Check == frequency_Tun_Sign01)
            {
                frequency_Tun_Sign01_Check = 0;
            }

        }

        private void SpawnSign02(Collider other)
        {
            //check if frequency met
            if (frequency_Tun_Sign02_Check == 0)
            {
                targetTransform = other.transform.Find("[DECORATION]").Find("RoadSigns").Find("RoadSign02");
                if (targetTransform != null)
                {
                    tempObject = Instantiate(tunnel_Sign02, targetTransform.position, targetTransform.rotation);
                    tempObject.transform.parent = targetTransform;
                }
            }
                

            //add loop
            frequency_Tun_Sign02_Check++;
            if (frequency_Tun_Sign02_Check == frequency_Tun_Sign02)
            {
                frequency_Tun_Sign02_Check = 0;
            }
        }

        private void SpawnSign03(Collider other)
        {
            targetTransform = other.transform.Find("[DECORATION]").Find("RoadSigns").Find("RoadSign03");
            if (targetTransform != null)
            {
                targetTransforms= other.transform.Find("[DECORATION]").Find("RoadSigns").Find("RoadSign03").GetComponentsInChildren<Transform>();
                if (targetTransforms != null)
                {
                    for (int i = 1; i < targetTransforms.Length; i++)
                    {
                        tempObject = Instantiate(tunnel_Sign03, targetTransforms[i].position, targetTransforms[i].rotation);
                        tempObject.transform.parent = targetTransforms[i];
                    }
                }
            }
        }

        private void SpawnHouse(Collider other)
        {
            //spawn house near
            //check if frequency met
            if (frequency_House_Near_Check == 0)
            {
                //decide direction
                if (Random.Range(0, 2) == 0)
                {
                    //left
                    Debug.Log("left near");
                    targetTransform = other.transform.Find("[DECORATION]").Find("Houses_Near").Find("left");
                    if (targetTransform != null)
                    {
                        tempObject = Instantiate(countryside_Building_near, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                    }
                }
                else
                {
                    //right
                    Debug.Log("right near");
                    targetTransform = other.transform.Find("[DECORATION]").Find("Houses_Near").Find("right");
                    if (targetTransform != null)
                    {
                        tempObject = Instantiate(countryside_Building_near, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                    }
                }
            }
            //spawn houses far
            if (frequency_House_Far_Check == 0)
            {
                //decide direction
                if (Random.Range(0, 2) == 0)
                {
                    //left
                    targetTransform = other.transform.Find("[DECORATION]").Find("Houses_Far").Find("left");
                    if (targetTransform != null)
                    {
                        tempObject = Instantiate(countryside_Building_far, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                    }
                }
                else
                {
                    //right
                    targetTransform = other.transform.Find("[DECORATION]").Find("Houses_Far").Find("right");
                    if (targetTransform != null)
                    {
                        tempObject = Instantiate(countryside_Building_far, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                    }
                }
            }


            //add loop
            frequency_House_Near_Check++;
            frequency_House_Far_Check++;
            if (frequency_House_Far_Check == frequency_House_Far)
            {
                frequency_House_Far_Check = 0;
            }
            if (frequency_House_Near_Check == frequency_House_Near)
            {
                frequency_House_Near_Check = 0;
            }
        }

        //9=Road
        //12=CurveLeft
        //13=CurveRight
        //14=Straight
        private void SpawnBumps(Collider other, int frequencyLevel,int layer)
        {
            Debug.Log("Bumps");
            GameObject targetObject;
            switch (layer)
            {
                case 12:
                    other.gameObject.layer = 9;
                    targetObject = bump_curve_left_lv1;
                    break;
                case 13:
                    other.gameObject.layer = 9;
                    targetObject = bump_curve_right_lv1;
                    break;
                case 14:
                    other.gameObject.layer = 9;
                    targetObject = bump_straight_lv1;
                    break;
                default:
                    other.gameObject.layer = 9;
                    targetObject = bump_straight_lv1;
                    break;
            }

            //check if frequency met
            if (frequency_Bumps_Check == 0)
            {
                targetTransform = other.transform.Find("[BUMPS]");
                if (targetTransform != null)
                {
                    tempObject = Instantiate(targetObject, targetTransform.position, targetTransform.rotation);
                    tempObject.transform.parent = targetTransform;
                }
            }
            //add loop
            frequency_Bumps_Check++;
            if (frequency_Bumps_Check == frequency_Bumps)
            {
                frequency_Bumps_Check = 0;
            }
        }
        private void SpawnBillboard(Collider other)
        {
            //check if frequency met
            if (frequency_Billboar_Check == 0)
            {
                //decide direction
                if (Random.Range(0, 2) == 0)
                {
                    //left
                    Debug.Log("left billboard");
                    targetTransform = other.transform.Find("[DECORATION]").Find("Billboards").Find("left");
                    if (targetTransform != null)
                    {
                        tempObject = Instantiate(countryside_Billboard, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                    }
                }
                else
                {
                    //right
                    Debug.Log("right billboard");
                    targetTransform = other.transform.Find("[DECORATION]").Find("Billboards").Find("right");
                    if (targetTransform != null)
                    {
                        tempObject = Instantiate(countryside_Billboard, targetTransform.position, targetTransform.rotation);
                        tempObject.transform.parent = targetTransform;
                    }
                }
            }

            //add loop
            frequency_Billboar_Check++;
            if (frequency_Billboar_Check == frequency_Billboard)
            {
                frequency_Billboar_Check = 0;
            }
        }
    }
}

