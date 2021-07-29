#ifndef CUSTOM_TESSELATION
#define CUSTOM_TESSELATION

#if defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_VULKAN) || defined(SHADER_API_METAL) || defined(SHADER_API_PSSL)
#define UNITY_CAN_COMPILE_TESSELLATION 1
#   define UNITY_domain                 domain
#   define UNITY_partitioning           partitioning
#   define UNITY_outputtopology         outputtopology
#   define UNITY_patchconstantfunc      patchconstantfunc
#   define UNITY_outputcontrolpoints    outputcontrolpoints
#endif



// The structure definition defines which variables it contains.
// This example uses the Attributes structure as an input structure in
// the vertex shader.

// the original vertex struct
//struct Attributes
//{
//    float4 vertex : POSITION;
//    float3 normal : NORMAL;
//    float4 color : COLOR;
//    float2 uv : TEXCOORD0;
//};

//// Extra vertex struct
//struct ControlPoint
//{
//    float4 vertex : INTERNALTESSPOS;
//    float3 normal : NORMAL;
//    float4 color : COLOR;
//    float2 uv : TEXCOORD0;
//};

//// vertex to fragment struct
//struct Varyings
//{
//	float4 vertex : SV_POSITION;
//	float3 normal : NORMAL;
//	float4 color : COLOR;
//	float2 uv : TEXCOORD0;
//};


// tessellation data
struct TessellationFactors
{
	float edge[3] : SV_TessFactor;
	float inside : SV_InsideTessFactor;
};

// tessellation variables, add these to your shader properties
float _Tess;
float _MinTessDistance;
float _MaxTessDistance;

// info so the GPU knows what to do (triangles) and how to set it up , clockwise, fractional division
// hull takes the original vertices and outputs more
//[UNITY_domain("tri")]
//[UNITY_outputcontrolpoints(3)]
//[UNITY_outputtopology("triangle_cw")]
////[UNITY_partitioning("fractional_odd")]
////[UNITY_partitioning("fractional_even")]
////[UNITY_partitioning("pow2")]
//[UNITY_partitioning("integer")]
//[UNITY_patchconstantfunc("patchConstantFunction")]
//ControlPoint HullPass(InputPatch<ControlPoint, 3> patch, uint id : SV_OutputControlPointID)
//{
//	return patch[id];
//}

#define HULL_PASS(HullName, ControlPointStruct, partitioning) \
	[UNITY_domain("tri")] \
	[UNITY_outputcontrolpoints(3)] \
	[UNITY_outputtopology("triangle_cw")] \
	[UNITY_partitioning(partitioning)] \
	[UNITY_patchconstantfunc("patchConstantFunction")] \
	ControlPointStruct HullName(InputPatch<ControlPointStruct, 3> patch, uint id : SV_OutputControlPointID) \
	{ \
	    return patch[id]; \
	}


// fade tessellation at a distance
float CalcDistanceTessFactor(float4 vertex, float minDist, float maxDist, float tess)
{
    float3 worldPosition = TransformObjectToWorld(vertex.xyz);
    float dist = distance(worldPosition, _WorldSpaceCameraPos);
	return clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
}

// tessellation
TessellationFactors patchConstantFunction(InputPatch<ControlPoint, 3> patch)
{
	// values for distance fading the tessellation
	//const float minDist = 5.0;
    float minDist = _MinTessDistance;
	float maxDist = _MaxTessDistance;

	TessellationFactors f;

	float edge0 = CalcDistanceTessFactor(patch[0].positionOS, minDist, maxDist, _Tess);
    float edge1 = CalcDistanceTessFactor(patch[1].positionOS, minDist, maxDist, _Tess);
    float edge2 = CalcDistanceTessFactor(patch[2].positionOS, minDist, maxDist, _Tess);

	// make sure there are no gaps between different tessellated distances, by averaging the edges out.
	f.inside = (edge0 + edge1 + edge2) / 3;
	f.edge[0] = f.inside;
	f.edge[1] = f.inside;
    f.edge[2] = f.inside;
	//f.edge[0] = (edge1 + edge2) / 2;
	//f.edge[1] = (edge2 + edge0) / 2;
	//f.edge[2] = (edge0 + edge1) / 2;
	return f;
}

#define DOMAIN_TRANSFER_FIELD(v, patch, barycentricCoordinates, fieldName) \
	v.fieldName = \
	patch[0].fieldName * barycentricCoordinates.x + \
	patch[1].fieldName * barycentricCoordinates.y + \
	patch[2].fieldName * barycentricCoordinates.z;

#endif