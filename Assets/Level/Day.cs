using System;
using System.Collections;
using UnityEngine;

namespace Biosearcher.Level
{
    public sealed class Day : MonoBehaviour
    {
        private const float DefaultDayLenght = 2 * 60;
        private const int DefaultFullSecondsSpent = 0;

        /// <summary> In seconds. </summary>
        [Tooltip("In seconds.")]
        [SerializeField] private float _dayLength = DefaultDayLenght;

        private float _timeSpent = 0;
        private float _oneOverDayLenght = 1.0f / DefaultDayLenght;

        public event Action OnEnd;
        public event Action OnSecondSpent;

        public int FullSecondsSpent { get; private set; } = DefaultFullSecondsSpent;
        public int SecondsLeft { get; private set; } = GetSecondsLeft(DefaultFullSecondsSpent, DefaultDayLenght);
        public float DayLength => _dayLength;
        public float DayLerp => _dayLength * _oneOverDayLenght;

        private void OnValidate() => _oneOverDayLenght = 1.0f / _dayLength;

        private void Awake() => SecondsLeft = GetSecondsLeft(FullSecondsSpent, _dayLength);

        private void Start() => StartCoroutine(Counting());

        private static int GetSecondsLeft(int fullSeconds, float dayLength) => Mathf.FloorToInt(dayLength) - fullSeconds;

        private IEnumerator Counting()
        {
            for (; ; )
            {
                _timeSpent += Time.deltaTime;

                int fullSeconds = Mathf.FloorToInt(_timeSpent);
                if (fullSeconds > FullSecondsSpent)
                {
                    FullSecondsSpent = fullSeconds;
                    SecondsLeft = GetSecondsLeft(fullSeconds, _dayLength);
                    OnSecondSpent?.Invoke();
                }

                if (_timeSpent > _dayLength)
                {
                    break;
                }

                yield return null;
            }

            OnEnd?.Invoke();
        }
    }
}