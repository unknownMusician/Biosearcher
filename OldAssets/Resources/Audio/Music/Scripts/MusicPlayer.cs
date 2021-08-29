using System.Collections;
using UnityEngine;

namespace Biosearcher.Audio.Music
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private MusicSettings _settings;
        private AudioSource _audioSource;
        private AudioClip[] _shuffledTracks;
        private bool _isAlive = true;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            //_audioSource.volume = _settings.Volume;
            _shuffledTracks = (AudioClip[])_settings.Tracks.Clone();

            _settings.Validate += OnValidate;
            StartCoroutine(PlayMusic());
        }
        private void OnDestroy()
        {
            _isAlive = false;
            _settings.Validate -= OnValidate;
        }
        private void OnValidate()
        {
            if (_settings != null)
            {
                GetComponent<AudioSource>().volume = _settings.Volume;
            }
        }
        private IEnumerator PlayMusic()
        {
            yield return new WaitForSeconds(_settings.Pause);
            while (_isAlive)
            {
                Shuffle(_shuffledTracks);
                foreach (AudioClip track in _shuffledTracks)
                {
                    _audioSource.clip = track;
                    _audioSource.Play();
                    yield return new WaitForSeconds(_audioSource.clip.length + _settings.Pause);
                    if (!_isAlive)
                    {
                        break;
                    }
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
