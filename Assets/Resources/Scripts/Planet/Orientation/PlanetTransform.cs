using UnityEngine;

namespace Biosearcher.Planet.Orientation
{
    [ExecuteAlways]
    public class PlanetTransform : MonoBehaviour
    {
        [SerializeField] protected Coordinates coordinates;
        protected readonly Vector3 planetPosition = Vector3.zero;
        protected readonly Vector3 planetRotationAxis = Vector3.up;
        protected Vector3 PositionRelativeToPlanet => transform.position - planetPosition;
        public Quaternion UniverseToPlanetRotation => Quaternion.FromToRotation(PositionRelativeToPlanet, planetRotationAxis);
        public Quaternion PlanetToUniverseRotation => Quaternion.FromToRotation(planetRotationAxis, PositionRelativeToPlanet);

        public Coordinates Coordinates
        {
            get => new Coordinates(Height, Latitude, Longitude);
            set => transform.position = ToPositionRelativeToPlanet(value) + planetPosition;
        }

        public float Height => ToHeight(PositionRelativeToPlanet);
        public float Latitude => ToLatitude(PositionRelativeToPlanet);
        public float Longitude => ToLongitude(PositionRelativeToPlanet);

        public Quaternion planetRotation
        {
            get => ToPlanet(transform.rotation);
            set => transform.rotation = ToUniverse(value);
        }

        // todo: Add planetEulerAngles

        protected void Update() => coordinates = Coordinates;

        protected void OnValidate()
        {
            Coordinates = coordinates;
        }

        protected float ToHeight(Vector3 positionRelativeToPlanet)
        {
            Vector3 p = positionRelativeToPlanet;
            return Mathf.Sqrt(p.x * p.x + p.z * p.z + p.y * p.y);
        }
        protected float ToLatitude(Vector3 positionRelativeToPlanet)
        {
            if (positionRelativeToPlanet == Vector3.zero)
            {
                return 0;
            }
            Vector3 p = positionRelativeToPlanet;
            return Mathf.Asin(p.y / Mathf.Sqrt(p.x * p.x + p.y * p.y + p.z * p.z)) * Mathf.Rad2Deg;
        }
        protected float ToLongitude(Vector3 positionRelativeToPlanet)
        {
            Vector3 p = positionRelativeToPlanet;
            return Mathf.Atan2(p.z, p.x) * Mathf.Rad2Deg;
        }

        public Coordinates ToCoordinates(Vector3 positionRelativeToPlanet)
        {
            return new Coordinates(ToHeight(positionRelativeToPlanet), ToLatitude(positionRelativeToPlanet), ToLongitude(positionRelativeToPlanet));
        }

        public Vector3 ToPositionRelativeToPlanet(Coordinates coordinates)
        {
            float x = coordinates.height * Mathf.Cos(coordinates.latitude * Mathf.Deg2Rad) * Mathf.Cos(coordinates.longitude * Mathf.Deg2Rad);
            float z = coordinates.height * Mathf.Cos(coordinates.latitude * Mathf.Deg2Rad) * Mathf.Sin(coordinates.longitude * Mathf.Deg2Rad);
            float y = coordinates.height * Mathf.Sin(coordinates.latitude * Mathf.Deg2Rad);

            return new Vector3(x, y, z);
        }

        public Quaternion ToPlanet(Quaternion universeRotation)
        {
            return UniverseToPlanetRotation * universeRotation;
        }
        public Quaternion ToUniverse(Quaternion planetRotation)
        {
            return PlanetToUniverseRotation * planetRotation;
        }
        public Vector3 ToPlanet(Vector3 universeRotation)
        {
            return UniverseToPlanetRotation * universeRotation;
        }
        public Vector3 ToUniverse(Vector3 planetRotation)
        {
            return PlanetToUniverseRotation * planetRotation;
        }
    }
}