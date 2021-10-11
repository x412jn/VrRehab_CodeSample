using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

namespace BCCH_VR_Therapy
{
    public class SepShelf : MonoBehaviour
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

        [Header("CORE")]
        public Transform hintAnchor;
        
        public ActionBasedContinuousMoveProvider continuousMoveProvider;

        [Space]
        [Header("Parameters")]
        public bool developerMode;
        public int level_density = 1;
        public int level_contrast = 1;
        public int amount_targetBox = 3;
        public bool taskEnabled = false;
        public int level_speed = 1;

        public int amount_Shelf = 24;

        [Space]
        [Header("Prefabs")]
        //private SepConfigHolder configHolder;
        public GameObject prefab_Shelf;
        public Transform anchor_Shelf_01;
        public Transform anchor_Shelf_02;
        
        public Texture[] patternTextures;
        public Texture[] patternTextures_Target;

        public GameObject box_den1;
        public GameObject box_den2;
        public GameObject box_den3;

        public GameObject hint_Collected;
        public GameObject hint_Complete;
        
        [HideInInspector] public int _level_density = 1;
        [HideInInspector] public int _level_contrast = 1;
        [HideInInspector] public int _amount_targetBox = 3;
        [HideInInspector] public int _taskCount = 0;
        [HideInInspector] public bool _taskEnabled = false;

        [HideInInspector] public int _level_speed = 1;

        [Space]
        [Header("DON'T TOUCH ANYTHING BELOW")]
        [SerializeField] private int _boxCount = 0;
        
        [SerializeField] private GameObject[] boxIndex;
        private int boxIndex_Index = 0;

        List<SepList_ShelfRowHold> listRowAnchorHolder;

        int[,,] rgbPairs_HC = new int[,,]
        {
            {
                {255,0,0 },
                {0,255,0 }
            },
            {
                {255,255,0 },
                {114,0,255 }
            },
            {
                {0,0,255 },
                {255,216,0 },
            }
        };

        int[,,] rgbPrais_MC = new int[,,]
        {
            {
                {229,57,57 },
                {40,0,51 }
            },
            {
                {252,12,132 },
                {255,159,0 }
            },
            {
                {255,127,208 },
                {50,129,255 }
            }
        };

        int[,,] rgbPairs_LC = new int[,,]
        {
            {
                {127,90,57 },
                {153,153,153 }
            },
            {
                {127,78,57 },
                {102,102,102 }
            },
            {
                {127,57,57 },
                {178,106,62 }
            }
        };

        

        //LEGACY

        /// <summary>
        /// 0=small|1=medium|2=Large|3=Epic
        /// </summary>
        [HideInInspector] public int spawnMode = 0;


        [HideInInspector] public Transform[] d1_row1;
        [HideInInspector] public Transform[] d1_row2;
        [HideInInspector] public Transform[] d1_row3;
        [HideInInspector] public Transform[] d1_row4;

        [Space]
        [Header("density2")]
        [HideInInspector] public Transform[] d2_row1;
        [HideInInspector] public Transform[] d2_row2;
        [HideInInspector] public Transform[] d2_row3;
        [HideInInspector] public Transform[] d2_row4;

        [Space]
        [Header("density3")]
        [HideInInspector] public Transform[] d3_row1;
        [HideInInspector] public Transform[] d3_row2;
        [HideInInspector] public Transform[] d3_row3;
        [HideInInspector] public Transform[] d3_row4;

        private void Start()
        {
            listRowAnchorHolder = new List<SepList_ShelfRowHold>();

            if (developerMode)
            {
                _level_contrast = level_contrast;
                _level_density = level_density;
                _amount_targetBox = amount_targetBox;
                _taskEnabled = taskEnabled;
                _level_speed = level_speed;
            }
            else
            {
                //configHolder = GameObject.Find("ConfigHolder").GetComponent<SepConfigHolder>();
                _level_contrast = PlayerPrefs.GetInt("Grocery_Contrast", 1);
                _level_density = PlayerPrefs.GetInt("Grocery_Density", 1);
                _amount_targetBox = PlayerPrefs.GetInt("Grocery_TargetAmount", 3);

                _level_speed = PlayerPrefs.GetInt("Grocery_Speed", 1);

                //get task enabled
                if (PlayerPrefs.GetInt("Grocery_TaskEnabled") == 1)
                {
                    _taskEnabled = true;
                }
                //default
                else if(PlayerPrefs.GetInt("Grocery_TaskEnabled") == 0)
                {
                    _taskEnabled = false;
                }
                else
                {
                    _taskEnabled = taskEnabled;
                }
            }


            //SpawnObjects_0_0();
            //SpawnObjects_0_1();
            //SpawnObjects_0_2();
            ChangeSpeed();
            SpawnShielf_0_1();
        }

        void ChangeSpeed()
        {
            switch (_level_speed)
            {
                case 1:
                    continuousMoveProvider.moveSpeed = 1f;
                    break;
                case 2:
                    continuousMoveProvider.moveSpeed = 1.5f;
                    break;
                case 3:
                    continuousMoveProvider.moveSpeed = 2f;
                    break;
                default:
                    continuousMoveProvider.moveSpeed = 1f;
                    break;
            }
        }
        void SpawnShielf_0_1()
        {
            
            //define next shelf anchor
            Transform targetTransform_NextShelf = anchor_Shelf_01;

            GameObject targetBox;

            //SpawnShelf
            GameObject tempObj;

            switch (_level_density)
            {
                case 1:
                    targetBox = box_den1;
                    break;
                case 2:
                    targetBox = box_den2;
                    break;
                case 3:
                    targetBox = box_den3;
                    break;
                default:
                    targetBox = box_den1;
                    break;
            }

            for (int i = 0; i < amount_Shelf; i++)
            {
                
                if (i < (amount_Shelf / 2))
                {
                    tempObj = Instantiate(prefab_Shelf, targetTransform_NextShelf);
                    tempObj.transform.parent = anchor_Shelf_01;
                    targetTransform_NextShelf = tempObj.transform.Find("Anchor_NextShelf");

                    switch (_level_density)
                    {
                        case 1:
                            Func_AssignAnchors("Density1", tempObj);
                            break;
                        case 2:
                            Func_AssignAnchors("Density2", tempObj);
                            break;
                        case 3:
                            Func_AssignAnchors("Density3", tempObj);
                            break;
                        default:
                            Func_AssignAnchors("Density4", tempObj);
                            break;
                    }

                }
                else if (i==(amount_Shelf/2))
                {
                    targetTransform_NextShelf = anchor_Shelf_02;
                    tempObj = Instantiate(prefab_Shelf, targetTransform_NextShelf);
                    tempObj.transform.parent = anchor_Shelf_02;
                    targetTransform_NextShelf = tempObj.transform.Find("Anchor_NextShelf");

                    switch (_level_density)
                    {
                        case 1:
                            Func_AssignAnchors("Density1", tempObj);
                            break;
                        case 2:
                            Func_AssignAnchors("Density2", tempObj);
                            break;
                        case 3:
                            Func_AssignAnchors("Density3", tempObj);
                            break;
                        default:
                            Func_AssignAnchors("Density4", tempObj);
                            break;
                    }
                }
                else
                {
                    tempObj = Instantiate(prefab_Shelf, targetTransform_NextShelf);
                    tempObj.transform.parent = anchor_Shelf_02;
                    targetTransform_NextShelf = tempObj.transform.Find("Anchor_NextShelf");

                    switch (_level_density)
                    {
                        case 1:
                            Func_AssignAnchors("Density1", tempObj);
                            break;
                        case 2:
                            Func_AssignAnchors("Density2", tempObj);
                            break;
                        case 3:
                            Func_AssignAnchors("Density3", tempObj);
                            break;
                        default:
                            Func_AssignAnchors("Density4", tempObj);
                            break;
                    }
                }
            }

            boxIndex = new GameObject[_boxCount];

            //==============================================
            //SpawnBox[major section]

            Transform targetTransform_box;
            Texture targetTexture;

            int randomNum;

            //define target materials
            int[,,] targetRgbArray;
            int[] color01 = new int[3];
            int[] color02 = new int[3];

            //get contrast level
            switch (_level_contrast)
            {
                case 1:
                    targetRgbArray = rgbPairs_LC;
                    break;
                case 2:
                    targetRgbArray = rgbPrais_MC;
                    break;
                case 3:
                    targetRgbArray = rgbPairs_HC;
                    break;
                default:
                    targetRgbArray = rgbPairs_LC;
                    break;
            }

            //spawn box
            for(int i = 0; i < listRowAnchorHolder.Count; i++)
            {
                //DO NOT SPAWN TARGET BOX HERE

                //define two mats by arrays
                
                randomNum = Random.Range(0, targetRgbArray.GetLength(0));
                for (int colorCount_1 = 0; colorCount_1 < targetRgbArray.GetLength(2); colorCount_1++)
                {
                    color01[colorCount_1] = targetRgbArray[randomNum, 0, colorCount_1];
                    color02[colorCount_1] = targetRgbArray[randomNum, 1, colorCount_1];
                }

                

                for (int j = 0; j < listRowAnchorHolder[i].row01.Length; j++)
                {
                    //Select random pattern
                    randomNum = Random.Range(0, patternTextures.Length);
                    targetTexture = patternTextures[randomNum];


                    targetTransform_box = listRowAnchorHolder[i].row01[j];
                    tempObj = Instantiate(targetBox, targetTransform_box.position, targetTransform_box.rotation);
                    tempObj.transform.parent = targetTransform_box;
                    
                    var spawnedMat_Color = tempObj.GetComponentInChildren<Renderer>().materials[1];

                    //change pattern and color
                    tempObj.GetComponentInChildren<Renderer>().materials[0].SetTexture("_MainTex", targetTexture);
                    spawnedMat_Color.SetColor("_Color", new Color((float)color01[0] / 255f, (float)color01[1] / 255f, (float)color01[2] / 255f, 1f));

                    //Index plus
                    boxIndex[boxIndex_Index] = tempObj;
                    boxIndex_Index++;
                    _boxCount--;
                }

                

                for (int k = 0; k < listRowAnchorHolder[i].row02.Length; k++)
                {
                    //Select random pattern
                    randomNum = Random.Range(0, patternTextures.Length);
                    targetTexture = patternTextures[randomNum];

                    targetTransform_box = listRowAnchorHolder[i].row02[k];
                    tempObj = Instantiate(targetBox, targetTransform_box.position, targetTransform_box.rotation);
                    tempObj.transform.parent = targetTransform_box;

                    var spawnedMat_Color = tempObj.GetComponentInChildren<Renderer>().materials[1];

                    //change pattern and color
                    tempObj.GetComponentInChildren<Renderer>().materials[0].SetTexture("_MainTex", targetTexture);
                    spawnedMat_Color.SetColor("_Color", new Color((float)color02[0] / 255f, (float)color02[1] / 255f, (float)color02[2] / 255f, 1f));

                    //Index plus
                    boxIndex[boxIndex_Index] = tempObj;
                    boxIndex_Index++;
                    _boxCount--;
                }
            }

            //============================================================

            //Spawn target[major section]
            if (_taskEnabled)
            {
                //Get index to spawn
                List<int> pastIndexHolder = new List<int>();
                for (int i = 0; i < _amount_targetBox; i++)
                {
                    randomNum = Random.Range(0, boxIndex.Length);
                    pastIndexHolder.Add(randomNum);
                }

                //spawn target boxes and tag them
                for (int i = 0; i < pastIndexHolder.Count; i++)
                {
                    //Get material
                    randomNum = Random.Range(0, patternTextures_Target.Length);
                    targetTexture = patternTextures_Target[randomNum];
                    if (boxIndex[pastIndexHolder[i]].tag != "Box_Target")
                    {
                        _taskCount++;
                        boxIndex[pastIndexHolder[i]].GetComponent<SepBoxInteraction>().sepShelf = this;
                    }
                    boxIndex[pastIndexHolder[i]].GetComponentInChildren<Renderer>().materials[0].SetTexture("_MainTex", targetTexture);
                    boxIndex[pastIndexHolder[i]].tag = "Box_Target";

                    boxIndex[pastIndexHolder[i]].transform.parent.parent.parent.parent.Find("TargetDot").gameObject.SetActive(true);
                }

            }

        }

        void Func_AssignAnchors(string densityString, GameObject tempObj)
        {
            //define temp anchor for boxes
            Transform[] tempAnchorRow1;
            Transform[] tempAnchorRow2;
            Transform[] tempAnchor;

            tempAnchor = tempObj.transform.Find(densityString).Find("Row1").GetComponentsInChildren<Transform>();
            tempAnchorRow1 = new Transform[tempAnchor.Length - 1];
            for (int j = 0; j < tempAnchorRow1.Length; j++)
            {
                tempAnchorRow1[j] = tempAnchor[j + 1];
                _boxCount++;
            }
            tempAnchor = tempObj.transform.Find(densityString).Find("Row2").GetComponentsInChildren<Transform>();
            tempAnchorRow2 = new Transform[tempAnchor.Length - 1];
            for (int j = 0; j < tempAnchorRow2.Length; j++)
            {
                tempAnchorRow2[j] = tempAnchor[j + 1];
                _boxCount++;
            }
            listRowAnchorHolder.Add(new SepList_ShelfRowHold(tempAnchorRow1, tempAnchorRow2));

            tempAnchor = tempObj.transform.Find(densityString).Find("Row3").GetComponentsInChildren<Transform>();
            tempAnchorRow1 = new Transform[tempAnchor.Length - 1];
            for (int j = 0; j < tempAnchorRow1.Length; j++)
            {
                tempAnchorRow1[j] = tempAnchor[j + 1];
                _boxCount++;
            }
            tempAnchor=tempObj.transform.Find(densityString).Find("Row4").GetComponentsInChildren<Transform>();
            tempAnchorRow2 = new Transform[tempAnchor.Length - 1];
            for (int j = 0; j < tempAnchorRow2.Length; j++)
            {
                tempAnchorRow2[j] = tempAnchor[j + 1];
                _boxCount++;
            }
            listRowAnchorHolder.Add(new SepList_ShelfRowHold(tempAnchorRow1, tempAnchorRow2));

        }

        public void CallHint()
        {
            GameObject tempObj;
            
            if (_taskCount <= 1)
            {
                tempObj = Instantiate(hint_Complete, hintAnchor);
            }
            else
            {
                tempObj = Instantiate(hint_Collected, hintAnchor);
            }
        }

        //void SpawnObjects_0_2()
        //{
        //    //define target gameobject
        //    GameObject targetGameobject;

        //    //define target position
        //    Transform[][] allRows = new Transform[4][];

        //    //define target materials
        //    int[,,] targetRgbArray;
        //    int[] color01 = new int[3];
        //    int[] color02 = new int[3];


        //    //get density level
        //    switch (_level_density)
        //    {
        //        case 1:
        //            targetGameobject = box_den1;
        //            allRows[0] = d1_row1;
        //            allRows[1] = d1_row2;
        //            allRows[2] = d1_row3;
        //            allRows[3] = d1_row4;
        //            break;
        //        case 2:
        //            targetGameobject = box_den2;
        //            allRows[0] = d2_row1;
        //            allRows[1] = d2_row2;
        //            allRows[2] = d2_row3;
        //            allRows[3] = d2_row4;
        //            break;
        //        case 3:
        //            targetGameobject = box_den3;
        //            allRows[0] = d3_row1;
        //            allRows[1] = d3_row2;
        //            allRows[2] = d3_row3;
        //            allRows[3] = d3_row4;
        //            break;
        //        default:
        //            targetGameobject = box_den1;
        //            allRows[0] = d1_row1;
        //            allRows[1] = d1_row2;
        //            allRows[2] = d1_row3;
        //            allRows[3] = d1_row4;
        //            break;
        //    }


        //    //get contrast level
        //    switch (_level_contrast)
        //    {
        //        case 1:
        //            targetRgbArray = rgbPairs_LC;
        //            break;
        //        case 2:
        //            targetRgbArray = rgbPrais_MC;
        //            break;
        //        case 3:
        //            targetRgbArray = rgbPairs_HC;
        //            break;
        //        default:
        //            targetRgbArray = rgbPairs_LC;
        //            break;
        //    }

        //    //define two mats by arrays
        //    int randomNum;
        //    randomNum = Random.Range(0, targetRgbArray.GetLength(0));
        //    for(int i = 0; i < targetRgbArray.GetLength(2); i++)
        //    {
        //        color01[i] = targetRgbArray[randomNum, 0, i];
        //        color02[i] = targetRgbArray[randomNum, 1, i];
        //    }

        //    //start spawn object;
        //    bool rowCheck = true;
        //    GameObject tempObj;
        //    for (int row = 0; row < 4; row++)
        //    {
        //        for (int i = 0; i < allRows[row].Length; i++)
        //        {
        //            tempObj = Instantiate(targetGameobject, allRows[row][i].position, allRows[row][i].rotation);
        //            var targetMat = tempObj.GetComponentInChildren<Renderer>().materials[1];
        //            if (rowCheck)
        //            {
        //                targetMat.SetColor("_Color", new Color((float)color01[0] / 255f, (float)color01[1] / 255f, (float)color01[2] / 255f, 1f));
        //                Debug.Log("color01 " + color01[0] + " " + color01[1] + " " + color01[2]);
        //            }
        //            else
        //            {
        //                targetMat.SetColor("_Color", new Color((float)color02[0] / 255f, (float)color02[1] / 255f, (float)color02[2] / 255f, 1f));
        //                Debug.Log("color01 " + color02[0] + " " + color02[1] + " " + color02[2]);
        //            }
        //        }
        //        rowCheck = !rowCheck;
        //    }
        //}


        //void SpawnObjects_0_1()
        //{
        //    //define target position
        //    Transform[][] allRows = new Transform[4][];

        //    //define target materials
        //    Material[] targetMatArray;
        //    Material targetMat01;
        //    Material targetMat02;

        //    //get density level
        //    switch (_level_density)
        //    {
        //        case 1:
        //            allRows[0] = d1_row1;
        //            allRows[1] = d1_row2;
        //            allRows[2] = d1_row3;
        //            allRows[3] = d1_row4;
        //            break;
        //        case 2:
        //            allRows[0] = d2_row1;
        //            allRows[1] = d2_row2;
        //            allRows[2] = d2_row3;
        //            allRows[3] = d2_row4;
        //            break;
        //        case 3:
        //            allRows[0] = d3_row1;
        //            allRows[1] = d3_row2;
        //            allRows[2] = d3_row3;
        //            allRows[3] = d3_row4;
        //            break;
        //        default:
        //            allRows[0] = d1_row1;
        //            allRows[1] = d1_row2;
        //            allRows[2] = d1_row3;
        //            allRows[3] = d1_row4;
        //            break;
        //    }


        //    //get contrast level
        //    switch (_level_contrast)
        //    {
        //        case 1:
        //            targetMatArray = LC_Mat;
        //            break;
        //        case 2:
        //            targetMatArray = MC_Mat;
        //            break;
        //        case 3:
        //            targetMatArray = HC_Mat;
        //            break;
        //        default:
        //            targetMatArray = LC_Mat;
        //            break;
        //    }

        //    //define two mats
        //    bool matAssigning = true;
        //    int randomNum;
        //    int randomHold;
        //    randomNum = Random.Range(0, targetMatArray.Length - 1);
        //    randomHold = randomNum;
        //    targetMat01 = targetMatArray[randomNum];
        //    targetMat02 = targetMatArray[randomNum];

        //    while (matAssigning)
        //    {
        //        randomNum = Random.Range(0, targetMatArray.Length - 1);
        //        if (randomNum != randomHold)
        //        {
        //            targetMat02 = targetMatArray[randomNum];
        //            matAssigning = false;
        //        }
        //    }

        //    //start spawn object;
        //    bool rowCheck = true;
        //    GameObject tempObj;
        //    for (int row = 0; row < 4; row++)
        //    {
        //        for (int i = 0; i < allRows[row].Length; i++)
        //        {
        //            tempObj = Instantiate(box_den1, allRows[row][i].position, allRows[row][i].rotation);
        //            if (rowCheck)
        //            {
        //                tempObj.GetComponentInChildren<Renderer>().material = targetMat01;
        //            }
        //            else
        //            {
        //                tempObj.GetComponentInChildren<Renderer>().material = targetMat02;
        //            }
        //        }
        //        rowCheck = !rowCheck;
        //    }
        //}


        //void SpawnObjects_0_0()
        //{
        //    GameObject[] targetObjectArray;
        //    Transform[] targetTransformArray;

        //    switch (spawnMode)
        //    {
        //        case 0:
        //            targetObjectArray = objectSmall;
        //            targetTransformArray = transformSmall;
        //            break;
        //        case 1:
        //            targetObjectArray = objectMedium;
        //            targetTransformArray = transformMedium;
        //            break;
        //        case 2:
        //            targetObjectArray = objectLarge;
        //            targetTransformArray = transformLarge;
        //            break;
        //        case 3:
        //            targetObjectArray = objectEpic;
        //            targetTransformArray = transformEpic;
        //            break;
        //        default:
        //            targetObjectArray = objectSmall;
        //            targetTransformArray = transformSmall;
        //            break;

        //    }

        //    for (int i = 0; i < targetTransformArray.Length; i++)
        //    {
        //        int random = Random.Range(0, targetObjectArray.Length - 1);
        //        Instantiate(targetObjectArray[random], targetTransformArray[i].position, targetTransformArray[i].rotation);
        //    }


        //}
    }
}

