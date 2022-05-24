#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using AreYouFruits.Common.ComponentGeneration;
using UnityEngine;

namespace Biosearcher.Player.Interactions.Hand
{
    public sealed class Hand : MonoBehaviour
    {
        #region Properties

        [SerializeField] private float _maxRaycastDistance;
        [SerializeField] private float _maxInteractDistance;
        [SerializeField] private float _maxInteractDistanceCamera;
        // todo
        [SerializeField] private float _carryHeight;
        [Space]
#nullable disable
        [SerializeField] private Transform _player;
#nullable enable

        private CarryInfo? _carryInfo;
        private readonly RaycastHit[] _raycastBuffer = new RaycastHit[10];

        internal Ray _lookRay;

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        private Quaternion GrabbedDefaultRotation => transform.rotation;

        #endregion

        #region Methods

        private void OnDestroy() => _cancellationSource.Cancel();

        public void TryGrab()
        {
            if (!TryGetClosestHit(_lookRay, out RaycastHit hit)
             || !(Vector3.Distance(hit.point, _player.position) < _maxInteractDistance)
             || !hit.collider.gameObject.TryGetHeldItem(out IGrabbable grabbed))
            {
                return;
            }

            _carryInfo = new CarryInfo(grabbed, hit.collider.gameObject.transform);
            grabbed.HandleGrab();

            Carrying();
        }

        internal void TryDrop()
        {
            _carryInfo?.Grabbable.HandleDrop();
            _carryInfo = null;
        }

        internal void TryInsert()
        {
            if (_carryInfo == null)
            {
                return;
            }

            InsertableInfo? insertInfo = _carryInfo.InsertInfo;

            if (insertInfo != null && InsertAction.TryInsert(insertInfo.InsertFriendly, insertInfo.Insertable))
            {
                _carryInfo = null;
            }
        }

        private void CarryOnCustomDistance(Vector3 hitPoint)
        {
            if (_carryInfo is null)
            {
                throw new InvalidOperationException();
            }

            Vector3 playerPosition = _player.position;
            Vector3 prefferedPosition;

            if (Vector3.Distance(hitPoint, playerPosition) <= _maxInteractDistance)
            {
                prefferedPosition = hitPoint;
            }
            else
            {
                prefferedPosition =
                    (hitPoint - _player.position).normalized * _maxInteractDistance + playerPosition;
            }

            _carryInfo.Transform.SetPositionAndRotation(prefferedPosition, GrabbedDefaultRotation);
        }

        private void CarryOnMaxDistance(Ray checkRay)
        {
            if (_carryInfo is null)
            {
                throw new InvalidOperationException();
            }

            Vector3 prefferedPosition =
                checkRay.direction * _maxInteractDistanceCamera + _lookRay.origin /*_camera.transform.position*/;

            _carryInfo.Transform.SetPositionAndRotation(prefferedPosition, GrabbedDefaultRotation);
        }

        private async void Carrying()
        {
            while (_carryInfo != null && enabled)
            {
                Carry(_lookRay);

                await Task.Yield();
            }
        }

        private void Carry(Ray castRay)
        {
            if (_carryInfo is null)
            {
                throw new InvalidOperationException();
            }

            if (!TryGetClosestHit(castRay, out RaycastHit hit, _carryInfo.Transform.gameObject))
            {
                CarryOnMaxDistance(castRay);
            }
            else if (_carryInfo.Grabbable is IInsertable insertable
             && hit.collider.gameObject.TryGetHeldItem(out IInsertFriendly insertFriendly)
             && InsertAction.CanInsert(insertFriendly, insertable))
            {
                if (insertFriendly != _carryInfo.InsertInfo?.InsertFriendly)
                {
                    InsertAction.TryAlignStart(insertFriendly, insertable);
                    _carryInfo.InsertInfo = new InsertableInfo(insertable, insertFriendly);
                }

                return;
            }
            else
            {
                CarryOnCustomDistance(hit.point);
            }

            if (_carryInfo.InsertInfo is not null)
            {
                InsertAction.HandleAlignEnd(_carryInfo.InsertInfo.InsertFriendly, _carryInfo.InsertInfo.Insertable);
                _carryInfo.InsertInfo = null;
            }
        }

        private bool TryGetClosestHit(Ray checkRay, out RaycastHit hit, GameObject? passObject = null)
        {
            int count = Physics.RaycastNonAlloc(checkRay, _raycastBuffer, _maxRaycastDistance);

            RaycastHit? resultHit = null;

            for (int i = 0; i < count; i++)
            {
                if (_raycastBuffer[i].collider.gameObject != passObject
                 && (resultHit == null
                     || Vector3.Distance(checkRay.origin, _raycastBuffer[i].point)
                      < Vector3.Distance(checkRay.origin, resultHit.Value.point)))
                {
                    resultHit = _raycastBuffer[i];
                }
            }

            hit = resultHit.GetValueOrDefault();

            return resultHit != null;
        }

        #endregion

        #region Classes

        private sealed class CarryInfo
        {
            public IGrabbable Grabbable { get; }
            public Transform Transform { get; }
            public InsertableInfo? InsertInfo { get; set; }

            public CarryInfo(IGrabbable grabbable, Transform transform)
            {
                Grabbable = grabbable;
                Transform = transform;
                InsertInfo = null;
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
