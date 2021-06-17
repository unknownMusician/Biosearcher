using TMPro;
using UnityEngine;

namespace Biosearcher.UserInterface.Sensors
{
    public abstract class ASensor : MonoBehaviour
    {
        [SerializeField] protected TMP_Text text;

        public abstract void UpdateSensors();
    }
}