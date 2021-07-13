﻿using UnityEngine;

namespace Biosearcher.Player
{
    [CreateAssetMenu(fileName = "Music Settings", menuName = "Music Settings", order = 53)]
    public sealed class MusicSettings : ScriptableObject
    {
        [SerializeField] private AudioClip[] _tracks;
        [Range(0f, 1f)]
        [SerializeField] private float _volume;
        [SerializeField] private int _pause;

        public AudioClip[] Tracks => _tracks;
        public float Volume => _volume;
        public int Pause => _pause;
    }
}
