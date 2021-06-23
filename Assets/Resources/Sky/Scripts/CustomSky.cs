using Biosearcher.Refactoring;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Biosearcher.Sky
{
    [VolumeComponentMenu("Sky/Custom Sky")]
    [SkyUniqueID(NEW_SKY_UNIQUE_ID)]
    public class CustomSky : SkySettings
    {
        const int NEW_SKY_UNIQUE_ID = 20382391;

        public FloatParameter overallIntensity = new FloatParameter(1);
        public FloatParameter nightSkyIntensity = new FloatParameter(1);
        public FloatParameter starsIntensity = new FloatParameter(1);
        public FloatParameter galaxyIntensity = new FloatParameter(1);
        public FloatParameter starsNoiseScale = new FloatParameter(100);
        public FloatParameter starsNoiseStep = new FloatParameter(0.07f);

        public override Type GetSkyRendererType() => typeof(CustomSkyRenderer);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [NeedsRefactor]
        public override int GetHashCode(Camera camera)
        {
            // Implement if your sky depends on the camera settings (like position for instance)
            return GetHashCode();
            // todo
        }
    }
}