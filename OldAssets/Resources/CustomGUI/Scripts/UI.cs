using Biosearcher.Refactoring;
using System;
using UnityEngine;

namespace Biosearcher.CustomGUI
{
    public sealed class UITest : MonoBehaviour, IUIWindowContainer
    {
        private float _time = 0;

        public bool IsOk { get; private set; }

        private void Update()
        {
            _time += Time.deltaTime;

            if (_time > 1.0f)
            {
                _time = 0;
                IsOk = !IsOk;
            }
        }

        public UIWindowParameters CreateUIWindow()
        {
            return new UIWindowParameters
            {
                titleProvider = () => nameof(UITest),
                descriptionProvider = () => "This is an UITest component. if you are seeing this, then it works fine. Probably...",
                fieldProviders = new Func<string>[] { () => $"Status: {(IsOk ? "Ok" : "Govna kusok")}" }
            };
        }
    }

    //[NeedsRefactor]
    //public static class UI
    //{
    //    private static IUIWindowContainer lastUIContainer;

    //    [NeedsRefactor]
    //    private static void MouseMove(Vector2 mousePosition)
    //    {
    //        Vector3 worldLookPoint = Camera.current.ScreenToWorldPoint(mousePosition);
    //        Ray raycastRay = new Ray
    //        {
    //            origin = worldLookPoint,
    //            direction = worldLookPoint - Camera.current.transform.position
    //        };
    //        const float maxDistance = 5000.0f;
    //        if (!Physics.Raycast(raycastRay, out RaycastHit hit, maxDistance))
    //        {
    //            return;
    //        }

    //        if (hit.collider.TryGetComponent(out IUIWindowContainer container))
    //        {
    //            if (lastUIContainer == container)
    //            {
    //                return;
    //            }
    //            else
    //            {
    //                if (lastUIContainer != default)
    //                {
    //                    DestroyWindow(lastUIContainer);
    //                }
    //                CreateWindow(container, container.CreateUIWindow());
    //            }
    //        }
    //        else if (lastUIContainer != default)
    //        {
    //            DestroyWindow(lastUIContainer);
    //        }
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

    public struct UIWindowParameters
    {
        public Func<string> titleProvider;
        public Func<string>[] fieldProviders;
        public Func<string> descriptionProvider;
    }

    public interface IUIWindowContainer
    {
        public UIWindowParameters CreateUIWindow();
    }
}