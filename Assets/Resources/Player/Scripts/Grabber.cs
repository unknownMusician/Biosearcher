using System;
using Biosearcher.InputHandling;
using System.Collections;
using System.Linq;
using Biosearcher.Planet.Orientation;
using UnityEngine;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(PlanetTransform))]
    public class Grabber : MonoBehaviour
    {
        #region Properties

        [SerializeField] protected LayerMask _grabbableMask;
        [SerializeField] protected float _maxRaycastDistance;
        [SerializeField] protected float _maxInteractDistance;
        [SerializeField] protected float _maxInteractDistanceCamera;
        [SerializeField] protected float _carryHeight;
        [Space]
        [SerializeField] protected Camera _camera;
        [SerializeField] protected Transform _player;

        protected PlanetTransform _planetTransform;
        protected Inserter _inserter;
        
        protected GrabberInput _input;
        
        protected IGrabbable _grabbed;

        #endregion

        #region MonoBehaviour methods

        protected void Awake()
        {
            _planetTransform = GetComponent<PlanetTransform>();
            _inserter = GetComponent<Inserter>();
            
            _input = new GrabberInput(new Presenter(this));
        }
        protected void OnDestroy() => _input.Dispose();

        protected void OnEnable() => _input.OnEnable();
        protected void OnDisable() => _input.OnDisable();

        #endregion

        #region Methods

        protected void TryGrab(Vector2 mouseScreenPosition)
        {
            if (TryGetClosestHit(_camera.ScreenPointToRay(mouseScreenPosition), out RaycastHit hit) &&
                Vector3.Distance(hit.point, _player.position) < _maxInteractDistance &&
                hit.collider.TryGetComponent(out _grabbed))
            {
                _grabbed.Grab();
                StartCoroutine(Carrying());
            }
        }
        public void TryDrop()
        {
            if (_grabbed != default)
            {
                _grabbed.Drop();
                _grabbed = default;
            }
        }

        protected void PrepareToInsert(RaycastHit hit, IInsertFriendly insertFriendly, IInsertable insertable, ref Vector3 carryPosition)
        {
            foreach (Type insertableType in insertFriendly.GetInsertableType())
            {
                if (insertableType != insertable.GetType())
                {
                    continue;
                }
                            
                _inserter.SetInsertStuff(insertable, insertFriendly);
                carryPosition = insertFriendly.GetAlignmentPosition();

                return;
            }
            GrabOnCustomDistance(hit, ref carryPosition);
        }
        protected void GrabOnCustomDistance(RaycastHit hit, ref Vector3 carryPosition)
        {
            Vector3 playerPosition = _player.position;
            if (Vector3.Distance(hit.point, playerPosition) <= _maxInteractDistance)
            {
                carryPosition = hit.point;
            }
            else
            {
                carryPosition = (hit.point - _player.position).normalized * _maxInteractDistance + playerPosition;
            }
        }
        protected void GrabOnMaxDistance(Ray checkRay, ref Vector3 carryPosition)
        {
            carryPosition = checkRay.direction * _maxInteractDistanceCamera + _camera.transform.position;
        }

        protected IEnumerator Carrying()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            yield return waitForFixedUpdate;
            while (_grabbed != default)
            {
                Ray checkRay = _camera.ScreenPointToRay(_input.MousePosition);

                MonoBehaviour grabbedMonoBehaviour = (MonoBehaviour) _grabbed;
                Vector3 carryPosition = default;
                if (TryGetClosestHit(checkRay, out RaycastHit hit, ((MonoBehaviour) _grabbed).gameObject))
                {
                    if (grabbedMonoBehaviour.TryGetComponent(out IInsertable insertable) &&
                        hit.collider.TryGetComponent(out IInsertFriendly insertFriendly))
                    {
                        PrepareToInsert(hit, insertFriendly, insertable, ref carryPosition);
                    }
                    else
                    {
                        GrabOnCustomDistance(hit, ref carryPosition);
                    }
                }
                else
                {
                    GrabOnMaxDistance(checkRay, ref carryPosition);
                }

                grabbedMonoBehaviour.transform.position = carryPosition;
                grabbedMonoBehaviour.transform.rotation = _planetTransform.ToUniverse(Quaternion.Euler(Vector3.up));;

                yield return waitForFixedUpdate;
            }
        }

        protected bool TryGetClosestHit(Ray checkRay, out RaycastHit hit, GameObject noRaycast = null)
        {
            RaycastHit[] hits = Physics.RaycastAll(checkRay, _maxRaycastDistance, _grabbableMask)
                .Where(hit => hit.collider.gameObject != noRaycast)
                .Where(hit => hit.collider.transform.parent == null || hit.collider.transform.parent.gameObject != noRaycast)
                .ToArray();

            if (hits.Length == 0)
            {
                hit = default;
                return false;
            }
            
            hit = hits.OrderBy(hit => Vector3.SqrMagnitude(hit.point - _camera.transform.position)).First();
            return true;
        }

        #endregion

        #region Classes

        public class Presenter
        {
            protected readonly Grabber _grabber;

            public Presenter(Grabber grabber) => _grabber = grabber;

            public void Grab(Vector2 mouseScreenPosition) => _grabber.TryGrab(mouseScreenPosition);
            public void Drop() => _grabber.TryDrop();
        }

        #endregion
    }
}
