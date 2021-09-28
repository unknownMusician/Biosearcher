using System.Collections;
using UnityEngine;

namespace Biosearcher.AudioHandling
{
    public sealed class CameraSound : MonoBehaviour
    {
        public void PlaySound(AudioClip sound)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            
            audioSource.clip = sound;
            audioSource.Play();
            
            StartCoroutine(DeleteAudioSource(audioSource, sound.length));
        }

        private IEnumerator DeleteAudioSource(AudioSource audioSource, float timeToDelete = 0)
        {
            yield return new WaitForSeconds(timeToDelete);
            Destroy(audioSource);
        }
    }
}
