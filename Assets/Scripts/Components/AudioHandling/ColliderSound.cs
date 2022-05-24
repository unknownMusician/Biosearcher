using Biosearcher.Level;
using UnityEngine;

namespace Biosearcher.AudioHandling
{
    public sealed class ColliderSound : MonoBehaviour
    {
#nullable disable
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ColliderListener _listener;
        [SerializeField] private float _speedDifferenceToPlaySound;
        [SerializeField] private AudioClip _collidingSound;
#nullable enable

        private void Awake()
        {
            _listener.OnCollide += other =>
            {
                if (other.relativeVelocity.magnitude >= _speedDifferenceToPlaySound)
                {
                    _audioSource.PlayOneShot(_collidingSound);
                }
            };
        }
    }
}
