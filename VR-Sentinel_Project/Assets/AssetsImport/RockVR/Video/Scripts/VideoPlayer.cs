using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RockVR.Video {
    public class VideoPlayer : MonoBehaviour {
#if UNITY_5_6_OR_NEWER
        /// <summary>
        /// Save the video files.
        /// </summary>
        public List<string> videoFiles = new List<string>();
        public List<string> currentVideoFiles = new List<string>();
        /// <summary>
        /// Play video properties.
        /// </summary>
        [SerializeField] private UnityEngine.Video.VideoPlayer videoPlayerImpl;

        [SerializeField] private float extendSizeDuration;
        [SerializeField] private Vector3 maxSize;
        private Vector3 baseSize;

        private int index = 0;

        //private VideoMenu videoMenu;

        public bool pauseActive { get; set; }

        //public Transform videoPanel { get; set; }
        public bool videoPlayerActive { get; set; }
        public bool saving { get; set; }

        public static VideoPlayer instance;
        private void Awake() {
            if(instance == null) {
                instance = this;
            }
        }

        private void Update() {
            if(videoPlayerImpl != null && videoPlayerImpl.isPlaying == false &&
                videoPlayerImpl.isPaused == false && videoPlayerActive) {
                ExitVideoMode();
            }
        }

        /*public void SetVideoMenuButtons(VideoMenu _videoMenu) {
            videoMenu = _videoMenu;
        }*/

        public void SetVideoPlayerImplCameraTarget(Camera _cam) {
            videoPlayerImpl.targetCamera = _cam;
        }

        public void SetTransformVideoPlayerImpl(Transform _propertiesObject) {
            videoPlayerImpl.transform.localPosition = _propertiesObject.localPosition;
            videoPlayerImpl.transform.GetComponent<RectTransform>().localRotation =
                _propertiesObject.GetComponent<RectTransform>().localRotation;
        }

        public void SetParentVideoPlayerImpl(Transform _parentObject) {
            videoPlayerImpl.transform.SetParent(_parentObject);

        }

        public void SetBaseSizeVideoPlayerImpl() {
            baseSize = videoPlayerImpl.transform.localScale;
        }

        public void SetPauseVideoPlayer() {
            if(videoPlayerImpl.isPlaying) {
                videoPlayerImpl.Pause();
                pauseActive = true;
            } else {
                videoPlayerImpl.Play();
                pauseActive = false;
            }
        }

        public void SetCloseWindowVideoPlayer() {
            ExitVideoMode();
        }

        /// <summary>
        /// Add video file to video file list.
        /// </summary>
        public void SetRootFolder() {
            if(Directory.Exists(PathConfig.SaveFolder)) {
                DirectoryInfo direction = new DirectoryInfo(PathConfig.SaveFolder);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                videoFiles.Clear();
                for(int i = 0; i < files.Length; i++) {
                    if(files[i].Name.EndsWith(".mp4")) {
                        videoFiles.Add(PathConfig.SaveFolder + files[i].Name);
                        continue;
                    }
                }
            }
            // Init VideoPlayer properties.
            //videoPlayerImpl = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
            //videoPlayerImpl = FindObjectOfType<UnityEngine.Video.VideoPlayer>();
            videoPlayerImpl.source = UnityEngine.Video.VideoSource.Url;
            videoPlayerImpl.playOnAwake = false;
            //videoPlayerImpl.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
            //SetVideoPlayerImplCameraTarget(Camera.main);
            videoPlayerImpl.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
            videoPlayerImpl.controlledAudioTrackCount = 1;
            videoPlayerImpl.aspectRatio = UnityEngine.Video.VideoAspectRatio.Stretch;
            if(gameObject.GetComponent<AudioSource>() != null) {
                videoPlayerImpl.SetTargetAudioSource(0, gameObject.GetComponent<AudioSource>());
                gameObject.GetComponent<AudioSource>().clip = null;
            }
        }
        /// <summary>
        /// Play video process.
        /// </summary>
        public void PlayVideo() {
            if(index >= videoFiles.Count) {
                return;
            }

            ResetScaleVideoPlayer();

            //videoMenu.ActiveButtonsObjects(true);

            videoPlayerImpl.url = "file://" + videoFiles[index];
            videoPlayerImpl.GetComponent<RawImage>().enabled = true;
            Debug.Log("[VideoPlayer::PlayVideo] Video Path:" + "video : " + index + " " + videoFiles[index]);

            AnimateVideoPlayer();

            videoPlayerImpl.Play();
        }

        public void PlayVideoAtIndex(int _index) {
            if(_index >= currentVideoFiles.Count) {
                return;
            }

            ResetScaleVideoPlayer();

            //videoMenu.ActiveButtonsObjects(true);

            videoPlayerImpl.url = "file://" + currentVideoFiles[_index];
            videoPlayerImpl.GetComponent<RawImage>().enabled = true;
            Debug.Log("[VideoPlayer::PlayVideo] Video Path:" + "video : " + _index + " " + currentVideoFiles[_index]);

            AnimateVideoPlayer();

            videoPlayerImpl.Play();
        }

        private void AnimateVideoPlayer() {
            videoPlayerImpl.transform.DOScale(maxSize, extendSizeDuration);
        }

        private void ResetScaleVideoPlayer() {
            videoPlayerImpl.transform.localScale = baseSize;
        }

        /// <summary>
        /// Turn to next video
        /// </summary>
        public void NextVideo() {
            if(index < videoFiles.Count) {
                index++;
            } else {
                Debug.LogWarning("[VideoPlayer::NextVideo] All videos have already been played.");
            }
        }

        public void PreviousVideo() {
            if(index > 0) {
                index--;
                // Play capture video.
                PlayVideo();
            } else {
                Debug.LogWarning("[VideoPlayer::PreviousVideo] First video already selected.");
            }
        }

        public void ExitVideoMode() {

            videoPlayerActive = false;
            videoPlayerImpl.Stop();
            //SetVideoPlayerImplCameraTarget(null);
            //Camera.main.GetComponent<CameraManager>().SetActiveReplayVideoCanvas(false);
            videoPlayerImpl.GetComponent<RawImage>().enabled = false;
            //ResetScaleVideoPlayer();
            //GameManager.Instance.replayVideoIsPlaying = false;

            pauseActive = false;
            //videoMenu.SetButtonPauseSprite();
            //videoMenu.ActiveButtonsObjects(false);

            //videoPlayerImpl.targetCamera = null;
            //videoPlayerImpl.GetComponent<RawImage>().enabled = false;
            //ResetScaleVideoPlayer();
        }


#endif
    }
}
