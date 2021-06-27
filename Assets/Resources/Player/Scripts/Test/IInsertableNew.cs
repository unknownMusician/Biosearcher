using Biosearcher.Planet.Orientation;
using Biosearcher.Refactoring;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Biosearcher.Assets.Resources.Player.Scripts.Test
{
    public interface IGrabbableNew
    {
        void HandleGrab();
        void HandleDrop();
    }

    public static class GrabbableExtensionNew
    {
        [NeedsRefactor]
        public static LayerMask GrabbableMask { get; private set; } = LayerMask.NameToLayer("Grabbable");

        public static void HandleGrabDefault<TGrabbable>(this TGrabbable grabbable, out LayerMask realMask)
            where TGrabbable : MonoBehaviour, IGrabbableNew
        {
            realMask = HandleGrabDefault(grabbable);
        }
        public static LayerMask HandleGrabDefault<TGrabbable>(this TGrabbable grabbable)
            where TGrabbable : MonoBehaviour, IGrabbableNew
        {
            LayerMask realMask = grabbable.gameObject.layer;
            grabbable.gameObject.layer = GrabbableMask;
            return realMask;
        }

        public static void HandleDropDefault<TGrabbable>(this TGrabbable grabbable, LayerMask realMask)
            where TGrabbable : MonoBehaviour, IGrabbableNew
        {
            grabbable.gameObject.layer = realMask;
        }
    }

    public static class InsertableExtensionNew
    {
        public static void HandleInsertDefault<TInsertable>(this TInsertable insertable, LayerMask realMask)
            where TInsertable : MonoBehaviour, IInsertableNew
        {
            insertable.HandleDropDefault(realMask);
        }
    }

    public interface IInsertableNew : IGrabbableNew
    {
        void HandleInsert();

        //bool IsCompatible(IInsertFriendly insertFriendly);

        bool TryInsertIn(IInsertFriendlyNew insertFriendly);
        bool TryAlignWith(IInsertFriendlyNew insertFriendly);
    }

    public static class InsertableExtensionsNew
    {
        /// <summary>
        /// Use IsCompatible() instead unless you are trying to call this from IsCompatible()
        /// </summary>
        public static bool IsCompatibleGeneric<TInsertable>(this TInsertable insertable, IInsertFriendlyNew insertFriendly)
            where TInsertable : IInsertableNew
        {
            return insertFriendly is IInsertFriendlyNew<TInsertable>;
        }

        /// <summary>
        /// Use TryInsertIn() instead unless you are trying to call this from TryInsertIn()
        /// </summary>
        public static bool TryInsertInGeneric<TInsertable>(this TInsertable insertable, IInsertFriendlyNew insertFriendly)
            where TInsertable : IInsertableNew
        {
            if (insertable.IsCompatibleGeneric(insertFriendly))
            {
                return (insertFriendly as IInsertFriendlyNew<TInsertable>).TryInsert(insertable);
            }
            return false;
        }

        /// <summary>
        /// Use TryAlignWith() instead unless you are trying to call this from TryAlignWith()
        /// </summary>
        public static bool TryAlignWithGeneric<TInsertable>(this TInsertable insertable, IInsertFriendlyNew insertFriendly)
            where TInsertable : IInsertableNew
        {
            if (insertable.IsCompatibleGeneric(insertFriendly))
            {
                return (insertFriendly as IInsertFriendlyNew<TInsertable>).TryAlign(insertable);
            }
            return false;
        }
    }

    public static class InsertFriendlyExtensionsNew
    {
        /// <summary>
        /// Use IInsertable.IsCompatible() instead
        /// </summary>
        //public static bool IsCompatible(this IInsertFriendly insertFriendly, IInsertable insertable)
        //{
        //    return insertable.IsCompatible(insertFriendly);
        //}

        /// <summary>
        /// Use TryInsert() instead unless you are trying to call this from TryInsert()
        /// </summary>
        public static bool TryInsertGeneric<TInsertable>(this IInsertFriendlyNew<TInsertable> insertFriendly, IInsertableNew insertable) where TInsertable : IInsertableNew
        {
            if (insertable.IsCompatibleGeneric(insertFriendly))
            {
                return insertFriendly.TryInsert((TInsertable)insertable);
            }
            return false;
        }
    }

    public interface IInsertFriendlyNew { }

    public interface IInsertFriendlyNew<TInsertable> : IInsertFriendlyNew where TInsertable : IInsertableNew
    {
        bool TryInsert(TInsertable insertable);
        bool TryAlign(TInsertable insertable);
    }

    public sealed class SeedNew : MonoBehaviour, IInsertableNew
    {
        private LayerMask _realMask;

        //public bool IsCompatible(IInsertFriendly insertFriendly) => this.Compatible(insertFriendly);
        public bool TryInsertIn(IInsertFriendlyNew insertFriendly) => this.TryInsertInGeneric(insertFriendly);
        public bool TryAlignWith(IInsertFriendlyNew insertFriendly) => this.TryAlignWithGeneric(insertFriendly);

        public void HandleInsert()
        {
            this.HandleInsertDefault(_realMask);
            // main logic...
        }

        public void HandleGrab()
        {
            this.HandleGrabDefault(out _realMask);
            // main logic...
        }

        public void HandleDrop()
        {
            this.HandleDropDefault(_realMask);
            // main logic...
        }
    }

    public sealed class SeedConsumerNew : MonoBehaviour, IInsertFriendlyNew<SeedNew>
    {
        public bool TryInsert(IInsertableNew insertable) => this.TryInsertGeneric(insertable);

        public bool TryAlign(SeedNew insertable)
        {
            // insertable.transform ...
            return false;
        }

        public bool TryInsert(SeedNew insertable)
        {
            // main logic...
            return false;
        }
    }

    public sealed class CoinNew : MonoBehaviour, IInsertableNew
    {
        private LayerMask _realMask;

        //public bool IsCompatible(IInsertFriendly insertFriendly) => this.Compatible(insertFriendly);
        public bool TryInsertIn(IInsertFriendlyNew insertFriendly) => this.TryInsertInGeneric(insertFriendly);
        public bool TryAlignWith(IInsertFriendlyNew insertFriendly) => this.TryAlignWithGeneric(insertFriendly);

        public void HandleInsert()
        {
            this.HandleInsertDefault(_realMask);
            // main logic...
        }

        public void HandleGrab()
        {
            this.HandleGrabDefault(out _realMask);
            // main logic...
        }

        public void HandleDrop()
        {
            this.HandleDropDefault(_realMask);
            // main logic...
        }
    }

    public sealed class SeedAndCoinConsumerNew : MonoBehaviour, IInsertFriendlyNew<SeedNew>, IInsertFriendlyNew<CoinNew>
    {
        public bool TryAlign(SeedNew insertable)
        {
            // insertable.transform ...
            return false;
        }

        public bool TryAlign(CoinNew insertable)
        {
            // insertable.transform ...
            return false;
        }

        public bool TryInsert(SeedNew insertable)
        {
            // main logic...
            insertable.HandleInsert();
            return false;
        }

        public bool TryInsert(CoinNew insertable)
        {
            // main logic...
            insertable.HandleInsert();
            return false;
        }
    }






    [RequireComponent(typeof(PlanetTransform))]
    public sealed class HandNew : MonoBehaviour
    {
        #region Properties

        [SerializeField] private float _maxRaycastDistance;
        [SerializeField] private float _maxInteractDistance;
        [SerializeField] private float _maxInteractDistanceCamera;
        [NeedsRefactor]
        [SerializeField] private float _carryHeight;
        [Space]
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _player;

        private PlanetTransform _planetTransform;
        private InputHandling.GrabberInput _input;

        private CarryInfo _carryInfo;

        private Quaternion GrabbedDefaultRotation => _planetTransform.ToUniverse(Quaternion.Euler(Vector3.up));

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            _planetTransform = GetComponent<PlanetTransform>();

            // todo
            _input = new InputHandling.GrabberInput(new Biosearcher.Player.Grabber.Presenter(new Biosearcher.Player.Grabber()));
        }
        //protected void OnDestroy() => _input.Dispose();

        //protected void OnEnable() => _input.OnEnable();
        //protected void OnDisable() => _input.OnDisable();

        #endregion

        #region Methods

        private void TryGrab()
        {
            if (TryGetClosestHit(_camera.ScreenPointToRay(_input.MousePosition), out RaycastHit hit) &&
                Vector3.Distance(hit.point, _player.position) < _maxInteractDistance &&
                hit.collider.TryGetComponent(out IGrabbableNew grabbed))
            {
                _carryInfo = new CarryInfo(grabbed);
                grabbed.HandleGrab();
                StartCoroutine(Carrying());
            }
        }
        private void TryDrop()
        {
            if (_carryInfo != default)
            {
                _carryInfo.Grabbable.HandleDrop();
                _carryInfo = default;
            }
        }
        private void TryInsert()
        {
            InsertableInfo insertInfo = _carryInfo.InsertInfo;
            if (insertInfo != null)
            {
                insertInfo.Insertable.TryInsertIn(insertInfo.InsertFriendly);
            }
        }

        private void CarryOnCustomDistance(Vector3 hitPoint)
        {
            Vector3 playerPosition = _player.position;
            Vector3 prefferedPosition;
            if (Vector3.Distance(hitPoint, playerPosition) <= _maxInteractDistance)
            {
                prefferedPosition = hitPoint;
            }
            else
            {
                prefferedPosition = (hitPoint - _player.position).normalized * _maxInteractDistance + playerPosition;
            }

            _carryInfo.Transform.SetPositionAndRotation(prefferedPosition, GrabbedDefaultRotation);
        }
        private void CarryOnMaxDistance(Ray checkRay)
        {
            Vector3 prefferedPosition = checkRay.direction * _maxInteractDistanceCamera + _camera.transform.position;

            _carryInfo.Transform.SetPositionAndRotation(prefferedPosition, GrabbedDefaultRotation);
        }

        private IEnumerator Carrying()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            while (_carryInfo != default)
            {
                Carry(_camera.ScreenPointToRay(_input.MousePosition));

                yield return waitForFixedUpdate;
            }
        }

        private void Carry(Ray castRay)
        {
            _carryInfo.InsertInfo = null;

            if (!TryGetClosestHit(castRay, out RaycastHit hit))
            {
                CarryOnMaxDistance(castRay);
            }
            else if ((_carryInfo.Grabbable is IInsertableNew insertable) && hit.collider.TryGetComponent(out IInsertFriendlyNew insertFriendly))
            {
                _carryInfo.InsertInfo = new InsertableInfo(insertable, insertFriendly);

                insertable.TryAlignWith(insertFriendly);
            }
            else
            {
                CarryOnCustomDistance(hit.point);
            }
        }

        private bool TryGetClosestHit(Ray checkRay, out RaycastHit hit)
        {
            return Physics.Raycast(checkRay, out hit, _maxRaycastDistance, ~GrabbableExtensionNew.GrabbableMask);
        }


        #endregion

        #region Classes

        public sealed class Presenter
        {
            private readonly HandNew _grabber;

            public Presenter(HandNew grabber) => _grabber = grabber;

            public void Grab() => _grabber.TryGrab();
            public void Drop() => _grabber.TryDrop();
            public void Insert() => _grabber.TryInsert();
        }

        private sealed class CarryInfo
        {
            public IGrabbableNew Grabbable { get; }
            public Transform Transform { get; }
            public InsertableInfo InsertInfo { get; set; }

            public CarryInfo(IGrabbableNew grabbable)
            {
                Grabbable = grabbable;
                Transform = ((MonoBehaviour)grabbable).transform;
            }
        }

        public sealed class InsertableInfo
        {
            public IInsertableNew Insertable { get; }
            public IInsertFriendlyNew InsertFriendly { get; }

            public InsertableInfo(IInsertableNew insertable, IInsertFriendlyNew insertFriendly)
            {
                Insertable = insertable;
                InsertFriendly = insertFriendly;
            }
        }

        #endregion
    }
}