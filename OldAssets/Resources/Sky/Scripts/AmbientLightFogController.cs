using UnityEngine;

namespace Biosearcher.Sky
{
    [ExecuteAlways]
    public class AmbientLightFogController : MonoBehaviour
    {
        [SerializeField] private Transform _mainStar;
        [SerializeField] private Material _fogMaterial;
        [SerializeField] [ColorUsage(true, true)] private Color _nightAmbientColor;
        [SerializeField] [ColorUsage(true, true)] private Color _dayAmbientColor;
        [SerializeField] [ColorUsage(true, true)] private Color _nightFogColor;
        [SerializeField] [ColorUsage(true, true)] private Color _dayFogColor;

        private void Update()
        {
            float lerp = Mathf.Clamp01(1.5f - Vector3.Angle(_mainStar.position, transform.position) * (1f / 90f));

            RenderSettings.ambientLight = Color.Lerp(_nightAmbientColor, _dayAmbientColor, lerp);
            _fogMaterial.SetColor("_Color", Color.Lerp(_nightFogColor, _dayFogColor, lerp));
        }
    }
}
