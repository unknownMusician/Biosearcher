using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher
{
    [CreateAssetMenu(fileName = "Custom Sky Settings", menuName = "Custom Sky Settings", order = 51)]
    public class CustomSkySettings : ScriptableObject
    {
        [SerializeField] protected AnimationCurve mainStarIntensity;
        [SerializeField] protected AnimationCurve mainStarEmission;
        [Space]
        [SerializeField] [ColorUsage(true, true)] protected Color nightColor;
        [SerializeField] [ColorUsage(true, true)] protected Color eveningColor;
        [SerializeField] [ColorUsage(true, true)] protected Color dayColor;
        [Tooltip("-1 - Night Color, 0 - Evening Color, 1 - Day Color.")]
        [SerializeField] protected AnimationCurve skyColor;
        [Space]
        [Tooltip("0 - Clear sky, 1 - Full of stars.")]
        [SerializeField] protected AnimationCurve starsIntensity;

        public float GetMainStarIntensity(float globalIntensity) => mainStarIntensity.Evaluate(globalIntensity);
        public float GetMainStarEmission(float globalIntensity) => mainStarEmission.Evaluate(globalIntensity);
        public Color GetSkyColor(float globalIntensity)
        {
            float intensity = Mathf.Clamp(skyColor.Evaluate(globalIntensity), -1, 1);
            Color firstColor = eveningColor;
            Color secondColor = intensity > 0 ? dayColor : nightColor;
            return Color.Lerp(firstColor, secondColor, Mathf.Abs(intensity));
        }
        public float GetNightSkyIntensity(float globalIntensity) => Mathf.Clamp01(starsIntensity.Evaluate(globalIntensity));
    }
}
