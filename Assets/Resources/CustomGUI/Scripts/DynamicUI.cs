using Biosearcher.Refactoring;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.CustomGUI
{
    //public class DynamicUI : MonoBehaviour
    //{
    //    private static IUIWindowContainer lastUIContainer;

    //    [NeedsRefactor]
    //    private void Update()
    //    {
    //        //Vector3 worldLookPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //        //Ray raycastRay = new Ray
    //        //{
    //        //    origin = worldLookPoint,
    //        //    direction = worldLookPoint - Camera.main.transform.position
    //        //};
    //        //const float maxDistance = 50.0f;
    //        //if (!Physics.Raycast(raycastRay, out RaycastHit hit, maxDistance))
    //        //{
    //        //    return;
    //        //}

    //        //if (hit.collider.TryGetComponent(out IUIWindowContainer container))
    //        //{
    //        //    if (lastUIContainer == container)
    //        //    {
    //        //        return;
    //        //    }
    //        //    else
    //        //    {
    //        //        if (lastUIContainer != default)
    //        //        {
    //        //            DestroyWindow(lastUIContainer);
    //        //        }
    //        //        CreateWindow(container, container.CreateUIWindow());
    //        //    }
    //        //}
    //        //else if (lastUIContainer != default)
    //        //{
    //        //    DestroyWindow(lastUIContainer);
    //        //}
    //    }

    //    [NeedsRefactor(Needs.Implementation)]
    //    private static void CreateWindow(IUIWindowContainer container, UIWindowParameters parameters)
    //    {
    //        if (lastUIContainer != default)
    //        {
    //            Debug.LogError("New Window created, when last window still exists.");
    //        }

    //        //create window
    //        Debug.Log("Created Window");

    //        lastUIContainer = container;
    //    }

    //    [NeedsRefactor(Needs.Implementation)]
    //    private static void DestroyWindow(IUIWindowContainer container)
    //    {
    //        if (lastUIContainer == default)
    //        {
    //            Debug.LogError("Nothing to destroy.");
    //        }

    //        //destroy window
    //        Debug.Log("Destroyed Window");

    //        lastUIContainer = default;
    //    }
    //}
}