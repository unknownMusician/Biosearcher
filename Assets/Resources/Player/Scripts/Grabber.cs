using Biosearcher.InputHandling;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Biosearcher.Player
{
    public class Grabber : MonoBehaviour
    {
        #region Properties

        [SerializeField] protected LayerMask _grabbableMask;
        [SerializeField] protected float _maxRaycastDistance;
        [SerializeField] protected float _maxInteractDistance;
        [SerializeField] protected float _maxInteractDistanceCamera;
        [SerializeField] protected float _carryHeight;

        [SerializeField] protected Camera _camera;
        [SerializeField] protected Transform _player;

        protected IGrabbable _grabbed;
        protected GrabberInput _input;

        #endregion

        #region MonoBehaviour methods

        protected void Awake() => _input = new GrabberInput(new Presenter(this));
        protected void OnDestroy() => _input.Dispose();

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
        protected void TryDrop()
        {
            if (_grabbed != default)
            {
                _grabbed.Drop();
                _grabbed = default;
            }
        }

        protected IEnumerator Carrying()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            yield return waitForFixedUpdate;
            while (_grabbed != default)
            {
                Ray checkRay = _camera.ScreenPointToRay(_input.MousePosition);

                Vector3 carryPosition;
                if (TryGetClosestHit(checkRay, out RaycastHit hit, ((MonoBehaviour) _grabbed).gameObject))
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
                else
                {
                    carryPosition = checkRay.direction * _maxInteractDistanceCamera + _camera.transform.position;
                }

                ((MonoBehaviour) _grabbed).transform.position = carryPosition;

                yield return waitForFixedUpdate;
            }
        }

        protected bool TryGetClosestHit(Ray checkRay, out RaycastHit hit, GameObject noRaycast = null)
        {
            RaycastHit[] hits = Physics.RaycastAll(checkRay, _maxRaycastDistance, _grabbableMask)
                .Where(hit => hit.collider.gameObject != noRaycast).ToArray();
            
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