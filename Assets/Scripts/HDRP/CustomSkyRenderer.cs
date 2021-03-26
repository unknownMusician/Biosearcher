using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Biosearcher.HDRP
{
    public class CustomSkyRenderer : SkyRenderer
    {
        public static readonly int _PixelCoordToViewDirWS = Shader.PropertyToID("_PixelCoordToViewDirWS");

        Material m_NewSkyMaterial; // Renders a cubemap into a render texture (can be cube or 2D)
        MaterialPropertyBlock m_PropertyBlock = new MaterialPropertyBlock();

        private static int m_RenderCubemapID = 0; // FragBaking
        private static int m_RenderFullscreenSkyID = 1; // FragRender

        public override void Build() { }
        public override void Cleanup() { }

        protected override bool Update(BuiltinSkyParameters builtinParams)
        {
            return false;
        }

        public override void RenderSky(BuiltinSkyParameters builtinParams, bool renderForCubemap, bool renderSunDisk)
        {
            // todo
            //using (new ProfilingSample(builtinParams.commandBuffer, "Draw sky"))
            using (new ProfilingScope(builtinParams.commandBuffer, null))
            {
                var newSky = builtinParams.skySettings as CustomSky;
                Material skyMaterial = newSky.skyMaterial;

                int passID = renderForCubemap ? m_RenderCubemapID : m_RenderFullscreenSkyID;

                m_PropertyBlock.SetMatrix(_PixelCoordToViewDirWS, builtinParams.pixelCoordToViewDirMatrix);

                CoreUtils.DrawFullScreen(builtinParams.commandBuffer, skyMaterial, m_PropertyBlock, passID);
            }
        }
    }
}