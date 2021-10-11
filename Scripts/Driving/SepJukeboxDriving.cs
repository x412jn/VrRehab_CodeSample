using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepJukeboxDriving : MonoBehaviour
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


        [Header("CORE")]
        SepSceneSpawner sceneSpawner;

        [Space]
        [Header("General player")]
        private AudioSource audioSource;

        public SepPause pause;

        [Space]
        [Header("Conditional clips")]
        public AudioClip[] clips_Conditional;

        
        


        // Start is called before the first frame update
        void Start()
        {
            sceneSpawner = GameObject.Find("Spawner").GetComponent<SepSceneSpawner>();
            pause = GameObject.Find("Pause").GetComponent<SepPause>();
            audioSource = this.GetComponent<AudioSource>();
            audioSource.loop = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (pause.onPause)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.UnPause();
                if (sceneSpawner.callEmergencyBgsChange)
                {
                    sceneSpawner.callEmergencyBgsChange = false;
                    StartCoroutine("EmergencyChangeSound");
                }
                else
                {
                    PlayMusic_Conditional((sceneSpawner._level_Weather > 1), sceneSpawner.onTunnel);
                }
            }
        }


        void PlayMusic_Conditional(bool onRain,bool onTunnel)
        {
            //0=!rain,!tunnel
            //1=!rain,tunnel
            //2=rain,!tunnel
            //3=rain,tunnel
            int commandParam_ClipSelection = 0;
            if (!onRain && !onTunnel)
            {
                commandParam_ClipSelection = 0;
            }
            if (!onRain && onTunnel)
            {
                commandParam_ClipSelection = 1;
            }
            if (onRain && !onTunnel)
            {
                commandParam_ClipSelection = 2;
            }
            if (onRain && onTunnel)
            {
                commandParam_ClipSelection = 3;
            }


            if (!audioSource.isPlaying)
            {
                audioSource.clip = GetNextMusic_Conditional(commandParam_ClipSelection);
                audioSource.Play();
                Debug.Log("On Playing Sound " + audioSource.clip.name);
            }
        }

        AudioClip GetNextMusic_Conditional(int inputCommand)
        {
            AudioClip targetClip;
            targetClip = clips_Conditional[inputCommand];
            return targetClip;
        }


        IEnumerator EmergencyChangeSound()
        {
            bool onRain = (sceneSpawner._level_Weather > 1);
            bool onTunnel = sceneSpawner.onTunnel;

            //0=!rain,!tunnel
            //1=!rain,tunnel
            //2=rain,!tunnel
            //3=rain,tunnel
            int commandParam_ClipSelection = 0;
            if (!onRain && !onTunnel)
            {
                commandParam_ClipSelection = 0;
            }
            if (!onRain && onTunnel)
            {
                commandParam_ClipSelection = 1;
            }
            if (onRain && !onTunnel)
            {
                commandParam_ClipSelection = 2;
            }
            if (onRain && onTunnel)
            {
                commandParam_ClipSelection = 3;
            }
            AudioClip targetClip;
            targetClip = clips_Conditional[commandParam_ClipSelection];
            audioSource.clip = targetClip;
            audioSource.Play();
            yield return new WaitForSeconds(1f);

        }
    }

}

