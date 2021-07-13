using System.Collections;
using UnityEngine;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private MusicSettings _musicSettings;
        private AudioSource _audioSource;
        private AudioClip[] _shuffledTracks;
        private bool _isAlive = true;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = _musicSettings.Volume;
            _shuffledTracks = (AudioClip[])_musicSettings.Tracks.Clone();
            
            StartCoroutine(PlayMusic());
        }
        private void Update()
        {
            _audioSource.volume = _musicSettings.Volume;
        }
        private void OnDestroy()
        {
            _isAlive = false;
            StopCoroutine(PlayMusic());
        }

        private IEnumerator PlayMusic()
        {
            yield return new WaitForSeconds(_musicSettings.Pause);
            while (_isAlive)
            {
                Shuffle(_shuffledTracks);
                foreach (AudioClip track in _shuffledTracks)
                {
                    _audioSource.clip = track;
                    _audioSource.Play();
                    yield return new WaitForSeconds(_audioSource.clip.length + _musicSettings.Pause);
                }
                
            }
        }

        // Fisher-Yates Shuffle algorithm
        private static void Shuffle<T>(T[] array)
        {
            System.Random rnd = new System.Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                T temp = array[i];
                int randIndex = rnd.Next(0, i + 1);
                array[i] = array[randIndex];
                array[randIndex] = temp;
            }
        }

    }
}
