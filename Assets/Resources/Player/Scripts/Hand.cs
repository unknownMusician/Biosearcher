using Biosearcher.InputHandling;
using System.Collections;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(PlanetTransform))]
    public sealed class Hand : MonoBehaviour
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
        private HandInput _input;

        private CarryInfo _carryInfo;

        private Quaternion GrabbedDefaultRotation => _planetTransform.ToUniverse(Quaternion.Euler(Vector3.up));

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            _planetTransform = GetComponent<PlanetTransform>();

            _input = new HandInput(new Presenter(this));
        }
        private void OnDestroy() => _input.Dispose();
        private void OnEnable() => _input.OnEnable();
        private void OnDisable() => _input.OnDisable();

        #endregion

        #region Methods

        private void TryGrab()
        {
            if (TryGetClosestHit(_camera.ScreenPointToRay(_input.MousePosition), out RaycastHit hit) &&
                Vector3.Distance(hit.point, _player.position) < _maxInteractDistance &&
                hit.collider.TryGetComponent(out IGrabbable grabbed))
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
            else if ((_carryInfo.Grabbable is IInsertable insertable) && hit.collider.TryGetComponent(out IInsertFriendly insertFriendly))
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
            return Physics.Raycast(checkRay, out hit, _maxRaycastDistance, ~GrabbableExtensions.GrabbableMask);
        }

        #endregion

        #region Classes

        public sealed class Presenter
        {
            private readonly Hand _hand;

            public Presenter(Hand hand) => _hand = hand;

            public void Grab() => _hand.TryGrab();
            public void Drop() => _hand.TryDrop();
            public void Insert() => _hand.TryInsert();
        }

        private sealed class CarryInfo
        {
            public IGrabbable Grabbable { get; }
            public Transform Transform { get; }
            public InsertableInfo InsertInfo { get; set; }

            public CarryInfo(IGrabbable grabbable)
            {
                Grabbable = grabbable;
                Transform = ((MonoBehaviour)grabbable).transform;
            }
        }

        public sealed class InsertableInfo
        {
            public IInsertable Insertable { get; }
            public IInsertFriendly InsertFriendly { get; }

            public InsertableInfo(IInsertable insertable, IInsertFriendly insertFriendly)
            {
                Insertable = insertable;
                InsertFriendly = insertFriendly;
            }
        }

        #endregion
    }
}
