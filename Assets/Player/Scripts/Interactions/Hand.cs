using Biosearcher.InputHandling;
using System.Collections;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Player.Interactions.Hand
{
    public sealed class Hand : MonoBehaviour
    {
        #region Properties

        [SerializeField] private float _maxRaycastDistance;
        [SerializeField] private float _maxInteractDistance;
        [SerializeField] private float _maxInteractDistanceCamera;
        [NeedsRefactor]
        [SerializeField] private float _carryHeight;
        [Space]
        [SerializeField] private Transform _player;

        private CarryInfo _carryInfo;

        internal Ray _lookRay;

        private Quaternion GrabbedDefaultRotation => transform.rotation;

        #endregion

        #region Methods

        internal void TryGrab()
        {
            if (TryGetClosestHit(_lookRay, out RaycastHit hit) &&
                Vector3.Distance(hit.point, _player.position) < _maxInteractDistance &&
                hit.collider.TryGetComponent(out IGrabbable grabbed))
            {
                _carryInfo = new CarryInfo(grabbed);
                grabbed.HandleGrab();
                StartCoroutine(Carrying());
            }
        }
        internal void TryDrop()
        {
            if (_carryInfo != default)
            {
                _carryInfo.Grabbable.HandleDrop();
                _carryInfo = default;
            }
        }
        internal void TryInsert()
        {
            InsertableInfo insertInfo = _carryInfo.InsertInfo;
            if (insertInfo != null && insertInfo.Insertable.TryInsertIn(insertInfo.InsertFriendly))
            {
                _carryInfo = null;
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
            Vector3 prefferedPosition = checkRay.direction * _maxInteractDistanceCamera + _lookRay.origin/*_camera.transform.position*/;

            _carryInfo.Transform.SetPositionAndRotation(prefferedPosition, GrabbedDefaultRotation);
        }

        private IEnumerator Carrying()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            while (_carryInfo != default)
            {
                Carry(_lookRay);

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
            else if ((_carryInfo.Grabbable is IInsertable insertable) 
                && hit.collider.TryGetComponent(out IInsertFriendly insertFriendly)
                && insertable.TryAlignWith(insertFriendly))
            {
                _carryInfo.InsertInfo = new InsertableInfo(insertable, insertFriendly);
            }
            else
            {
                CarryOnCustomDistance(hit.point);
            }
        }

        private bool TryGetClosestHit(Ray checkRay, out RaycastHit hit)
        {
            return Physics.Raycast(checkRay, out hit, _maxRaycastDistance, ~(1 << GrabbableExtensions.GrabbedLayerIndex));
        }

        #endregion

        #region Classes

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
