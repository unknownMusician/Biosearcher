using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Biosearcher.HDRP
{
    [VolumeComponentMenu("Sky/Custom Sky")]
    [SkyUniqueID(NEW_SKY_UNIQUE_ID)]
    public class CustomSky : SkySettings
    {
        const int NEW_SKY_UNIQUE_ID = 20382390;

        public Material skyMaterial;

        public override Type GetSkyRendererType() => typeof(CustomSkyRenderer);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}