using System;
using UnityEngine;

namespace Biosearcher.Level
{
    public sealed class Score : MonoBehaviour
    {
        public int Value { get; private set; } = 0;

        public event Action OnChange;

        public void AddScore(int score)
        {
            Value += score;
            OnChange?.Invoke();
        }

        public void SubtractScore(int score)
        {
            Value -= score;
            OnChange?.Invoke();
        }
    }
}