using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Biosearcher.Sky
{
    public class CustomSkyRenderer : SkyRenderer
    {
        public static readonly int _PixelCoordToViewDirWS = Shader.PropertyToID("_PixelCoordToViewDirWS");
        public static readonly int _Intensity = Shader.PropertyToID("_Intensity");

        private MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        private Material customSkyMaterial;

        private static readonly int RenderCubemapID = 0; // FragBaking
        private static readonly int RenderFullscreenSkyID = 1; // FragRender

        public override void Build()
        {
            customSkyMaterial = CoreUtils.CreateEngineMaterial(GetNewSkyShader());
        }

        // Project dependent way to retrieve a shader.
        Shader GetNewSkyShader()
        {
            return Resources.Load<Shader>("Sky/Scripts/CustomSky");
            //return Resources.Load<Shader>("Shaders/CustomSkybox");
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(customSkyMaterial);
        }


        protected override bool Update(BuiltinSkyParameters builtinParams)
        {
            return true;
        }

        public override void RenderSky(BuiltinSkyParameters builtinParams, bool renderForCubemap, bool renderSunDisk)
        {
            // todo
            //using (new ProfilingSample(builtinParams.commandBuffer, "Draw sky"))
            using (new ProfilingScope(builtinParams.commandBuffer, new ProfilingSampler("Draw sky")))
            {
                var customSky = builtinParams.skySettings as CustomSky;

                int passID = renderForCubemap ? RenderCubemapID : RenderFullscreenSkyID;

                float intensity = GetSkyIntensity(customSky, builtinParams.debugSettings);
                propertyBlock.SetFloat(_Intensity, intensity);
                propertyBlock.SetMatrix(_PixelCoordToViewDirWS, builtinParams.pixelCoordToViewDirMatrix);

                SetShaderGameData(customSky);

                CoreUtils.DrawFullScreen(builtinParams.commandBuffer, customSkyMaterial, propertyBlock, passID);
            }
        }

        protected void SetShaderGameData(CustomSky customSky)
        {
            propertyBlock.SetColor("_SkyColor", SkyGameManager.SkyColor);
            propertyBlock.SetFloat("_NightSkyIntensity", customSky.nightSkyIntensity.value * SkyGameManager.NightSkyIntensity);
            propertyBlock.SetFloat("_StarsIntensity", customSky.starsIntensity.value);
            propertyBlock.SetFloat("_GalaxyIntensity", customSky.galaxyIntensity.value);
            propertyBlock.SetFloat("_OverallIntensity", customSky.overallIntensity.value);
            propertyBlock.SetFloat("_StarsNoiseScale", customSky.starsNoiseScale.value);
            propertyBlock.SetFloat("_StarsNoiseStep", customSky.starsNoiseStep.value);
        }
    }
}
    