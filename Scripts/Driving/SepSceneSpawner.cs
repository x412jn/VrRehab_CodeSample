using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BCCH_VR_Therapy
{
    

    public class SepSceneSpawner : MonoBehaviour
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

        //for checking wether car inside tunnel
        [HideInInspector] public bool onTunnel = false;
        //Use to control the emergency change bgs
        [HideInInspector] public bool callEmergencyBgsChange = false;

        //All PARAM
        //private SepConfigHolder configHolder;
        
        public bool developerMode = false;

        //CORE
        [Header("CORE")]
        public EnviroSky enviroSky;
        public EnviroSkyMgr enviroSkyMgr;
        
        

        //PREFABS
        [Space]
        [Header("Prefabs")]
        public GameObject[] prefabsCountryside_straight;
        public GameObject[] prefabsCountryside_curve;
        public GameObject[] prefabTunnel_straight;
        public GameObject[] prefabTunnel_curve;

        public EnviroWeatherPreset weather_clearSky;
        public EnviroWeatherPreset weather_cloudy;
        public EnviroWeatherPreset weather_rain;

        [Space]
        [Header("Spawn Mode Perameters")]
        public bool enableTunnel = true;
        public bool enableCountryside = true;
        //one road equals 35 meters(approximatly)
        [Range(1, 20)]
        public int length_Tunnel = 20;
        [Range(1, 20)]
        public int length_Countryside = 20;
        [Range(0, 3)]
        public int level_Bumps = 0;
        [Range(0, 2)]
        public int level_Tunnel_lightColor = 0;
        [Range(0, 3)]
        public int level_Countryside_ObjectComplexity = 0;

        public int level_Traffic = 0;
        [Range(0, 3)]
        public int level_Tunnel_Roadsign = 0;
        [Range(0, 100)]
        public int randomCheckAmount_Curve = 0;
        [Range(0, 3)]
        public int level_TimeOfDay = 0;
        [Range(0,3)]
        public int level_Weather = 0;

        


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
        
        public int BG_SpawnTimes = 10;
        public int InitialSceneSpawnTimes = 200;

        [Space]
        [Header("tracking anchor")]
        public Transform BG_Anchor;

        [Space]
        [Header("Terrain Perameters")]
        public GameObject prefab_terrain;
        public Transform terrain_anchor;
        
        
        private List<GameObject> existedTerrainHolder;
        private List<GameObject> tunnelsForChecking;
        private List<int[]> coordinationCheck;
        public int terrain_Length = 80;
        public int terrain_Width = 50;

        //[Space]
        //[Header("DON'T TOUCH ANYTHING BELOW!")]
        [HideInInspector] public bool _enableTunnel;
        [HideInInspector] public bool _enableCountryside;

        [HideInInspector] public int _lenght_Tunnel = 20;
        [HideInInspector] public int _length_Countryside = 20;
        [HideInInspector] public int _level_Bumps = 0;
        [HideInInspector] public int _level_Tunnel_lightColor = 0;
        [HideInInspector] public int _level_Countryside_ObjectComplexity = 0;
        [HideInInspector] public int _level_Traffic = 0;
        [HideInInspector] public int _level_Tunnel_Roadsign = 0;
        [HideInInspector] public int _randomCheckAmount_Curve = 0;

        [HideInInspector] public int _level_TimeOfDay = 0;
        [HideInInspector] public int _level_Weather = 0;

        [HideInInspector] public List<GameObject> existedObjectHolder;
        [HideInInspector] private Transform startPoint;
        [HideInInspector] private Transform endPoint;
        [HideInInspector] public List<Transform> pathNodes;
        /// <summary>
        /// to tell engine where to spawn new prefabs
        /// </summary>
        [HideInInspector] public Transform currentPathEnd;


        private GameObject tempObject;
        private GameObject tempObjectBackward;

        private void Awake()
        {
            

            //DEFINE MAJOR PARAMETERS FROM DATABASE
            if (!developerMode)
            {
                _randomCheckAmount_Curve = PlayerPrefs.GetInt("Driving_Turns", randomCheckAmount_Curve);
                if (PlayerPrefs.GetInt("EnableTunnel") == 1)
                {
                    _enableTunnel = true;
                }
                else if (PlayerPrefs.GetInt("EnableTunnel") == 0)
                {
                    _enableTunnel = false;
                }
                else
                {
                    _enableTunnel = true;
                }

                if (PlayerPrefs.GetInt("EnableCountryside") == 1)
                {
                    _enableCountryside = true;
                }
                else if (PlayerPrefs.GetInt("EnableCountryside") == 0)
                {
                    _enableCountryside = false;
                }
                else
                {
                    _enableCountryside = true;
                }

                _lenght_Tunnel = PlayerPrefs.GetInt("Driving_Length_Tunnel", length_Tunnel);
                _length_Countryside = PlayerPrefs.GetInt("Driving_Length_Countryside", length_Countryside);
                _level_Bumps = PlayerPrefs.GetInt("Driving_Bumps", level_Bumps);
                _level_Tunnel_lightColor = PlayerPrefs.GetInt("Driving_Tunnel_LightColor", level_Tunnel_lightColor);
                _level_Countryside_ObjectComplexity = PlayerPrefs.GetInt("Driving_Countryside_TreesDensity", level_Countryside_ObjectComplexity);
                _level_Traffic = PlayerPrefs.GetInt("Driving_Traffic", level_Traffic);
                _level_Tunnel_Roadsign = PlayerPrefs.GetInt("Driving_Tunnel_Roadsign", level_Tunnel_Roadsign);

                _level_TimeOfDay = PlayerPrefs.GetInt("Driving_TimeOfDay", level_TimeOfDay);
                _level_Weather = PlayerPrefs.GetInt("Driving_Weather", level_Weather);
                //Debug.Log("level of weather= " + _level_Weather);
                
            }
            else
            {
                _randomCheckAmount_Curve = randomCheckAmount_Curve;
                _enableTunnel = enableTunnel;

                _enableCountryside = enableCountryside;
                _lenght_Tunnel = length_Tunnel;
                _length_Countryside = length_Countryside;
                _level_Bumps = level_Bumps;
                _level_Tunnel_lightColor = level_Tunnel_lightColor;
                _level_Countryside_ObjectComplexity = level_Countryside_ObjectComplexity;
                _level_Traffic = level_Traffic;
                _level_Tunnel_Roadsign = level_Tunnel_Roadsign;

                _level_TimeOfDay = level_TimeOfDay;
                _level_Weather = level_Weather;

                
            }
        }
        void Start()
        {
            

            //configHolder = GameObject.Find("ConfigHolder").GetComponent<SepConfigHolder>();


            pathNodes = new List<Transform>();
            existedObjectHolder = new List<GameObject>();
            existedTerrainHolder = new List<GameObject>();
            tunnelsForChecking = new List<GameObject>();
            coordinationCheck = new List<int[]>();

            TimeCheck();
            WeatherCheck();

            if (_enableTunnel && _enableCountryside)
            {
                currentSpawnRemain = _length_Countryside;
                onSpawningTunnel = false;
            }
            if (_enableTunnel && !_enableCountryside)
            {
                currentSpawnRemain = _lenght_Tunnel;
                onSpawningTunnel = true;
            }
            if (!_enableTunnel && _enableCountryside)
            {
                currentSpawnRemain = _length_Countryside;
                onSpawningTunnel = false;
            }
            if (!_enableTunnel && !_enableCountryside)
            {
                currentSpawnRemain = _length_Countryside;
                onSpawningTunnel = false;
            }

            onTunnel = onSpawningTunnel;

            SpawnPrefab(this.transform, InitialSceneSpawnTimes, _randomCheckAmount_Curve);
            spawnBG(BG_Anchor, BG_SpawnTimes);

            Debug.Log(enviroSkyMgr.Weather.currentActiveWeatherPreset);
            //SpawnTerrainStatic(terrain_Length, terrain_Width);

        }

        private void WeatherCheck()
        {
            switch (_level_Weather)
            {
                case 0:
                    //clear sky
                    enviroSkyMgr.Weather.startWeatherPreset=(weather_clearSky);
                    Debug.Log("clear sky");
                    break;
                case 1:
                    //cloud
                    enviroSkyMgr.Weather.startWeatherPreset = (weather_cloudy);
                    Debug.Log("cloud");
                    break;
                case 2:
                    //rain
                    enviroSkyMgr.Weather.startWeatherPreset = (weather_rain);
                    Debug.Log("rain");
                    break;

                case 3:
                    enviroSkyMgr.Weather.startWeatherPreset = (weather_rain);
                    Debug.Log("rain");
                    break;

                default:
                    //clear sky
                    enviroSkyMgr.Weather.startWeatherPreset = (weather_clearSky);
                    Debug.Log("clear sky default");
                    break;
            }
            
        }

        private void TimeCheck()
        {
            switch (_level_TimeOfDay)
            {
                case 0:
                    //noon
                    enviroSky.SetTime(2021, 168, 12, 0, 0);
                    enviroSky.skySettings.rayleigh = 5.15f;
                    enviroSky.skySettings.mie = 8f;
                    break;
                case 1:
                    //afternoon
                    enviroSky.SetTime(2021, 168, 8, 0, 0);
                    enviroSky.skySettings.rayleigh = 5.15f;
                    enviroSky.skySettings.mie = 8f;
                    break;
                case 2:
                    //dawn
                    enviroSky.SetTime(2021, 168, 6, 0, 0);
                    enviroSky.skySettings.rayleigh = 5.15f;
                    enviroSky.skySettings.mie = 8f;
                    break;
                case 3:
                    //night
                    enviroSky.SetTime(2021, 168, 21, 0, 0);
                    enviroSky.skySettings.rayleigh = 0.29f;
                    enviroSky.skySettings.mie = 1.31f;
                    break;
                default:
                    break;
            }
            
        }

        public void SpawnModeCheck()
        {
            //spawnModeTarget_straight = prefabTunnel_straight;
            //spawnModeTarget_curve = prefabTunnel_curve;

            if (_enableTunnel && !_enableCountryside)
            {
                spawnModeTarget_straight = prefabTunnel_straight;
                spawnModeTarget_curve = prefabTunnel_curve;
            }
            else if (!_enableTunnel && _enableCountryside)
            {
                spawnModeTarget_straight = prefabsCountryside_straight;
                spawnModeTarget_curve = prefabsCountryside_curve;
            }
            else if (_enableTunnel && _enableCountryside)
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
                        currentSpawnRemain = _lenght_Tunnel;
                    }
                    else
                    {
                        currentSpawnRemain = _length_Countryside;
                    }
                }
            }
            else if (!_enableTunnel && !_enableCountryside)
            {
                spawnModeTarget_straight = prefabsCountryside_straight;
                spawnModeTarget_curve = prefabsCountryside_curve;
            }
            else
            {
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
                arrayRnd = Random.Range(0, spawnModeTarget_straight.Length);
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
                if (frequencyRnd >= _randomCheckAmount_Curve)
                {
                    arrayRnd_object01 = Random.Range(0, spawnModeTarget_straight.Length);
                    spawningObject_target = spawnModeTarget_straight[arrayRnd_object01];
                    tempObject = Instantiate(spawningObject_target, spawningPos);

                    tunnelsForChecking.Add(tempObject);
                    
                    //insert list

                    tempObject.transform.parent = this.transform;
                    existedObjectHolder.Add(tempObject);
                    spawningPos = tempObject.transform.Find("EndPoint").transform;

                    //startPoint = tempObject.transform.Find("WayPoints").Find("wp1");
                    //pathNodes.Add(startPoint);
                    GetWaypoints(tempObject.transform.Find("WayPoints"));

                    //HoldCleaner();
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
                    
                    //insert list

                    tempObject.transform.parent = this.transform;
                    existedObjectHolder.Add(tempObject);
                    spawningPos = tempObject.transform.Find("EndPoint").transform;

                    //startPoint = tempObject.transform.Find("WayPoints").Find("wp1");
                    //pathNodes.Add(startPoint);
                    GetWaypoints(tempObject.transform.Find("WayPoints"));

                    //HoldCleaner();
                }
                currentPathEnd = spawningPos;
                
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
            if (existedObjectHolder.Count > 300)
            {
                for (int i = 0; i < 100; i++)
                {
                    //Debug.Log("on deleting " + existedObjectHolder[0].name + "transform is " + existedObjectHolder[0].transform.position);
                    Destroy(existedObjectHolder[0]);
                    existedObjectHolder.RemoveAt(0);
                    //Debug.Log("remove time:" + i);
                }
                //Debug.Log("current object list 0 is " + existedObjectHolder[0].name + "transform is " + existedObjectHolder[0].transform.position);
            }

        }
    }
}

