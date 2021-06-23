using Biosearcher.Refactoring;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Biosearcher.Sky
{
    public class SkyGameManager : MonoBehaviour
    {
        [NeedsRefactor("ScriptableObject")]
        protected static SkyGameManager instance;

        public static Vector3 playerPosition;
        public static Vector3 planetPosition;
        public static Vector3 mainStarPosition;

        public static float GlobalIntensity => 1 - Vector3.Angle(playerPosition - planetPosition, mainStarPosition - planetPosition) / 90;

        public static float MainStarIntensity => instance?.settings.GetMainStarIntensity(GlobalIntensity) ?? default;
        public static float MainStarEmission => instance?.settings.GetMainStarEmission(GlobalIntensity) ?? default;
        public static Color SkyColor => instance?.settings.GetSkyColor(GlobalIntensity) ?? default;
        public static float NightSkyIntensity => instance?.settings.GetNightSkyIntensity(GlobalIntensity) ?? default;

        [SerializeField] protected Light mainStarLight;
        [SerializeField] protected HDAdditionalLightData mainStarLightData;
        [SerializeField] protected Material mainStarMaterial;
        [SerializeField] protected CustomSkySettings settings;

        protected void Awake() => instance = this;
        protected void OnDestroy()
        {
            instance = null;
            mainStarMaterial.SetFloat("_EmissionStrenght", 0);
        }

        [NeedsRefactor(Needs.RemoveTodo)]
        protected void Update()
        {
            mainStarLight.intensity = MainStarIntensity;
            mainStarMaterial.SetFloat("_EmissionStrenght", MainStarEmission);
            //mainStarLightData.SetIntensity(MainStarIntensity); // todo: WTF?!
        }
    }
}
