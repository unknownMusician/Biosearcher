using UnityEngine;
using UnityEngine.UI;

namespace Biosearcher.Level
{
    public sealed class ScoreUI : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private Score _score;

        private void Awake() => _score.OnChange += ShowScore;
        private void OnDestroy() => _score.OnChange -= ShowScore;

        private void Start() => ShowScore();

        private void ShowScore() => _scoreText.text = _score.Value.ToString();
    }
}