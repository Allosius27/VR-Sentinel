﻿using UnityEngine;
namespace AllosiusDev {
    [System.Serializable]
    public struct Sound {
        public AudioClip Clip => clip;
        public SoundParameters Parameters => parameters;

        [SerializeField] private AudioClip clip;
        [SerializeField] private SoundParameters parameters;

        public Sound(AudioClip clip = null, float volume = 1, float volume3d = 1, float pitch = 1, bool loop = false) {
            this.clip = clip;
            parameters = new SoundParameters(volume, volume3d, pitch, loop);
        }
        public Sound(AudioClip clip, SoundParameters parameters) {
            this.clip = clip;
            this.parameters = parameters;
        }

        public void ActiveSpacialBlend(int _value)
        {
            this.parameters.SetSpacialBlend(_value);
        }
    }
}