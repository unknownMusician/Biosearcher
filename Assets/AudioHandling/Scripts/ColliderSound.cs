using UnityEngine;

namespace Biosearcher.AudioHandling
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ColliderSound : MonoBehaviour
    {
        #region Components

        private AudioSource _audioSource;

        #endregion
        
        #region Settings

        [SerializeField] private float _speedDifferenceToPlaySound;
        [SerializeField] private AudioClip _collidingSound;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision other)
        {
            float speedDifference = other.relativeVelocity.magnitude;
            
            if (speedDifference > _speedDifferenceToPlaySound)
            {
                _audioSource.clip = _collidingSound;
                _audioSource.Play();
            }
        }

        #endregion
    }
}
