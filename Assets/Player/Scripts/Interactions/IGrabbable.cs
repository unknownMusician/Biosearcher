using System;
using UnityEngine;

namespace Biosearcher.Player.Interactions
{
    public interface IGrabbable
    {
        Action OnGrab { get; internal set; }
        LayerMask DefaultLayer { get; internal set; }

        void HandleGrab();
        void HandleDrop();
    }
}
