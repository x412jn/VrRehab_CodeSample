using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace BCCH_VR_Therapy
{
    public class SepVideoPlayer : MonoBehaviour
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

        public GameObject btn_play;
        public GameObject btn_Skip;
        public GameObject video;
        public VideoPlayer videoPlayer;
        public VideoClip[] videoClips;
        public GameObject nextClipVisualCue;

        public int currentIndex = 0;
        public bool isPlayingVideo = false;

        public float checkCD = 1f;
        bool onCheckCD = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!onCheckCD)
            {
                onCheckCD = true;
                StartCoroutine("CheckDelay");
            }
        }

        IEnumerator CheckDelay()
        {
            yield return new WaitForSeconds(1);
            PlayStateCheck();
            onCheckCD = false;
        }

        void PlayStateCheck()
        {
            if (isPlayingVideo && !videoPlayer.isPlaying)
            {
                if (currentIndex >= videoClips.Length)
                {
                    //video over, turn off boolean
                    video.SetActive(false);
                    isPlayingVideo = false;
                    btn_play.SetActive(true);
                    btn_Skip.SetActive(false);
                    nextClipVisualCue.SetActive(false);
                }
                else
                {
                    Debug.Log("Play Check, next video");
                    PlayNextClip();
                }
            }
        }

        public void PlayFirstClip()
        {
            video.SetActive(true);
            currentIndex = 0;
            videoPlayer.clip = videoClips[currentIndex];
            videoPlayer.Play();
            currentIndex++;
            btn_play.SetActive(false);
            btn_Skip.SetActive(true);
            nextClipVisualCue.SetActive(true);
            isPlayingVideo = true;
        }

        public void StopPlaying()
        {
            video.SetActive(false);
            isPlayingVideo = false;

            videoPlayer.Stop();
            currentIndex = 0;

            btn_play.SetActive(true);
            btn_Skip.SetActive(false);
            nextClipVisualCue.SetActive(false);
        }

        public void PlayNextClip()
        {
            if (isPlayingVideo)
            {
                if (currentIndex < videoClips.Length)
                {
                    videoPlayer.Stop();
                    videoPlayer.clip = videoClips[currentIndex];
                    videoPlayer.Play();
                    currentIndex++;
                }
                else
                {
                    //current video reach to end;
                    StopPlaying();
                }
            }
        }

    }
}

