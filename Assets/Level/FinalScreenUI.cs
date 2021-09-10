using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Biosearcher.Level
{
    public class FinalScreenUI : MonoBehaviour
    {
        [SerializeField] private GameObject _finalScreen;
        [SerializeField] private Day _day;
        [SerializeField] private Score _score;
        [SerializeField] private Text _scoreText;

        private void Awake() => _day.OnEnd += ShowScreen;
        private void OnDestroy() => _day.OnEnd -= ShowScreen;

        private void Start() => _finalScreen.SetActive(false);

        private void ShowScreen()
        {
            _finalScreen.SetActive(true);
            _scoreText.text = _score.Value.ToString();
        }

        public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        public void Exit() => Application.Quit();
    }
}