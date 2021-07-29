#ifndef CUSTOM_GEOMETRY
#define CUSTOM_GEOMETRY

GeometryOutput SetupVertex(float3 positionWS, float3 normalWS, float2 uv, float colorLerp)
{
    GeometryOutput output = (GeometryOutput) 0;
    output.positionWS = positionWS;
    output.normalWS = normalWS;
    output.uv = uv;
    output.colorLerp = colorLerp;

    output.positionCS = CalculatePositionCSWithShadowCasterLogic(positionWS, normalWS);
    return output;
}

void SetupAndOutputTriangle(inout TriangleStream<GeometryOutput> outputStream, VertexOutput a, VertexOutput b, VertexOutput c, float colorLerpA = 0.0f, float colorLerpB = 0.0f, float colorLerpC = 1.0f)
{
    outputStream.RestartStrip();
    
    float3 normalWS = GetNormalFromTriangle(a.positionWS, b.positionWS, c.positionWS);

    outputStream.Append(SetupVertex(a.positionWS, normalWS, a.uv, colorLerpA));
    outputStream.Append(SetupVertex(b.positionWS, normalWS, b.uv, colorLerpB));
    outputStream.Append(SetupVertex(c.positionWS, normalWS, c.uv, colorLerpC));
}

#endif