using System.Collections;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    public class TestBehaviour : MonoBehaviour
    {
        private void Start()
        {
            RefactorManager.OnUnityStart();
        }
    }
}