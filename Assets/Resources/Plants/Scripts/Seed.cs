using Biosearcher.Player;
using UnityEngine;

namespace Biosearcher.Plants
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Seed : MonoBehaviour, IInsertable, IGrabbable
    {
        #region Properties

        [SerializeField] private float _minDistanceToGround;
        [SerializeField] private LayerMask _groundMask;
        [Space]
        [SerializeField] private PlantSettings _plantSettings;

        private Rigidbody _rigidbody;
        private Collider _collider;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        #endregion

        #region Methods

        public Plant Plant(Vector3 position, Quaternion rotation, Transform parent) 
        {
            var prefab = Resources.Load<GameObject>("Plants/Prefabs/Plant");
            var plantObject = Instantiate(prefab, position, rotation, parent);
            var plant = plantObject.GetComponent<Plant>();
            plant.Initialize(_plantSettings);

            return plant; 
        }

        public void Drop()
        {
            if (Physics.OverlapSphere(transform.position, _minDistanceToGround, _groundMask).Length == 0)
            {
                _rigidbody.isKinematic = false;
            }
            _collider.enabled = true;
        }
        public void Grab()
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }

        #endregion
    }
}
