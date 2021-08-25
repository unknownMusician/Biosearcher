using Biosearcher.Common;
using Biosearcher.Common.States;
using UnityEngine;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(Walker))]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class WalkerAudio : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Vector2 _engineAudioPitchMultiplier = new Vector3(0.02f, 0.3f);
        [SerializeField] private Vector2 _engineAudioVolumeMultiplier = new Vector3(0.01f, 0.02f);
        [SerializeField] private float _engineAudioSmoothTime = 0.5f;
        [SerializeField] private AudioSource _engineAudioSource;
        [SerializeField] private AudioSource _wheelsAudioSource;

        private Walker _walker;
        private Rigidbody _rigidbody;

        private HookableStateManager<WalkerState> _state;

        private float _pitchVelocity;
        private float _volumeVelocity;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            this.GetComponents(out _walker, out _rigidbody);
            RegisterStates();
        }

        private void Start()
        {
            _state.HookTo(_walker.StateHook);
        }

        private void OnDestroy() => _state.Dispose();

        private void Update() => _state.Active.Invoke(SetAudioPitch);

        #endregion

        #region Methods

        private void RegisterStates()
        {
            _state = new HookableStateManager<WalkerState>();

            _state.Register(WalkerState.OnGroundState)
                .Register(SetAudioPitch, SetAudioPitch);

            _state.Register(WalkerState.InAirState)
                .Register(SetAudioPitch, SetAudioPitchInAir);
        }
        private void SetAudioPitch()
        {
            PitchSmoothDamp(_rigidbody.velocity.magnitude);
            _wheelsAudioSource.volume = _rigidbody.velocity.magnitude;
        }
        private void SetAudioPitchInAir()
        {
            PitchSmoothDamp(0f);
            _wheelsAudioSource.volume = 0f;
        }

        private void PitchSmoothDamp(float targetVelocity)
        {
            float targetPitch = targetVelocity * _engineAudioPitchMultiplier.x + _engineAudioPitchMultiplier.y;
            float targetVolume = targetVelocity * _engineAudioVolumeMultiplier.x + _engineAudioVolumeMultiplier.y;
            _engineAudioSource.pitch = Mathf.SmoothDamp(_engineAudioSource.pitch, targetPitch, ref _pitchVelocity, _engineAudioSmoothTime);
            _engineAudioSource.volume = Mathf.SmoothDamp(_engineAudioSource.volume, targetVolume, ref _volumeVelocity, _engineAudioSmoothTime);
        }

        #endregion
    }
}
