using UnityEngine;

namespace Biosearcher.Player.Interactions
{
    public interface IGrabbable
    {
        LayerMask DefaultLayer { get; internal set; }

        void HandleGrab();
        void HandleDrop();
    }
}
