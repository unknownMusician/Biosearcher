using System;
using UnityEngine;

namespace Biosearcher.Player.Interactions
{
    public interface IGrabbable
    {
        public event Action OnGrab;

        public void HandleGrab();
        public void HandleDrop();
    }
}
