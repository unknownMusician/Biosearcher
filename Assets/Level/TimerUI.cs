using Biosearcher.Refactoring;
using UnityEngine;
using UnityEngine.UI;

namespace Biosearcher.Level
{
    public sealed class TimerUI : MonoBehaviour
    {
        [SerializeField] private Text _timerText;
        [Space]
        [SerializeField] private Day _day;

        private void Awake() => _day.OnSecondSpent += HandleSecondSpent;
        private void OnDestroy() => _day.OnSecondSpent -= HandleSecondSpent;

        private void Start() => HandleSecondSpent();

        private void HandleSecondSpent()
        {
            _timerText.text = ToMinutesAndSeconds(_day.SecondsLeft);
        }

        [NeedsRefactor]
        private string ToMinutesAndSeconds(int time)
        {
            int minutes = time / 60;
            int seconds = time - minutes * 60;

            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}