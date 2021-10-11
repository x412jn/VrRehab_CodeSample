using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
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

    public class SepWiper : MonoBehaviour
    {
        public Transform wiper;
        public Transform wiperStart;
        public Transform wiperEnd;
        [SerializeField]
        [Range(0f,1f)]
        public float lerpPct = 0f;
        public float lerpCD = 10f;
        public float lerpSpeed = 2f;

        public SepSceneSpawner spawner;

        private Transform from;
        private Transform to;
        private Transform tempHolder;

        private bool onStart2End = true;
        private bool onLerping = false;



        private void Start()
        {
            if (onStart2End)
            {
                from = wiperStart;
                to = wiperEnd;
            }
            else
            {
                from = wiperEnd;
                to = wiperStart;
            }
            wiper.rotation = from.rotation;

            if (spawner._level_Weather < 2)
            {
                this.GetComponent<SepWiper>().enabled = false;
            }

            if (spawner._level_Weather == 2)
            {
                lerpSpeed = 2f;
                lerpCD = 0.5f;
            }
            if (spawner._level_Weather == 3)
            {
                lerpSpeed = 4f;
                lerpCD = 0.25f;
            }
        }


        // Update is called once per frame
        void Update()
        {

            if (!onLerping)
            {
                onLerping = true;
                lerpPct = 0;
                Debug.Log("On Call WIPER");
                StartCoroutine("LerpingCD");
            }
            else
            {
                wiper.rotation = Quaternion.Lerp(from.rotation, to.rotation, lerpPct);
                lerpPct += Time.deltaTime * lerpSpeed;
            }

        }

        IEnumerator LerpingCD()
        {
            yield return new WaitForSeconds(lerpCD);
            tempHolder = from;
            from = to;
            to = tempHolder;
            wiper.rotation = from.rotation;
            
            onLerping = false;
        }

        //void Lerping_Normal()
        //{

        //}

        //IEnumerator Lerping()
        //{
        //    onLerping = true;
        //    if (onStart2End)
        //    {
        //        float t = 0f;
        //        while (t < lerpPct)
        //        {
        //            Debug.Log("On Start2End");
        //            t += Time.deltaTime;
        //            wiper.rotation = Quaternion.Slerp(wiperStart.rotation, wiperEnd.rotation, t);
                    
        //        }
        //        yield return new WaitForSeconds(lerpCD);
        //        Debug.Log("Start2End Over");
        //        onStart2End = false;
        //        onLerping = false;
        //    }
        //    else
        //    {
        //        float t = 0f;
        //        while (t < lerpPct)
        //        {
        //            Debug.Log("On End2Start");
        //            t += Time.deltaTime;
        //            wiper.rotation = Quaternion.Slerp(wiperEnd.rotation, wiperStart.rotation, t);
                    
        //        }
        //        yield return new WaitForSeconds(lerpCD);
        //        Debug.Log("End2Start Over");
        //        onStart2End = true;
        //        onLerping = false;
        //    }
        //}

        //IEnumerator LerpStart2End()
        //{
        //    var t = 0.0;
        //    while (t < lerpPct)
        //    {
        //        t += Time.deltaTime;
        //        wiper.rotation = Quaternion.Lerp(wiperStart.rotation, wiperEnd.rotation, lerpPct);
        //        yield return new WaitForSeconds(lerpCD);
        //        onStart2End = false;
        //    }
        //}

        //IEnumerator LerpEnd2Start()
        //{
        //    var t = 0.0;
        //    while (t < lerpPct)
        //    {
        //        t += Time.deltaTime;
        //        wiper.rotation = Quaternion.Lerp(wiperEnd.rotation, wiperStart.rotation, lerpPct);
        //        yield return new WaitForSeconds(lerpCD);
        //        onStart2End = true;
        //    }
        //}

    }
}

