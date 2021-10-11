using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCCH_VR_Therapy
{
    public class SepJukebox : MonoBehaviour
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

        [Space]
        [Header("General player")]
        public AudioClip[] clips;
        private AudioSource audioSource;
        int listIndex = 0;

        public bool delay = false;
        public float delayCD = 10f;
        private bool onDelayCD = false;

        public SepPause pause;

        [Space]
        [Header("Conditional clips")]
        public AudioClip[] clips_Conditional;
        public bool enableConditionalClips;

        // Start is called before the first frame update
        void Start()
        {
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
                PlayMusic_General();
            }

        }

        private void PlayMusic_General()
        {
            if (!delay)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = GetNextMusic();
                    audioSource.Play();
                    Debug.Log("On Playing Sound " + audioSource.clip.name);
                }
            }
            else
            {
                if (!onDelayCD && !audioSource.isPlaying)
                {
                    audioSource.clip = GetNextMusic();
                    audioSource.Play();
                }
                else if (!onDelayCD && audioSource.isPlaying)
                {
                    onDelayCD = true;
                    StartCoroutine("DelaySound");
                }
            }
        }

        AudioClip GetNextMusic()
        {
            AudioClip targetClip;
            targetClip = clips[listIndex];
            listIndex++;
            if (listIndex >= clips.Length)
            {
                listIndex = 0;
            }
            return targetClip;
        }

        IEnumerator DelaySound()
        {
            yield return new WaitForSeconds(delayCD);
            onDelayCD = false;
        }
    }

}
