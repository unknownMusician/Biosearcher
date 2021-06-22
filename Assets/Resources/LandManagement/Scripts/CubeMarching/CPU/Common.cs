using UnityEngine;

namespace Biosearcher.LandManagement.CubeMarching.CPU
{
    internal sealed class Common
    {
        private ConstantBuffer _constantBuffer;

        internal Common(ref ConstantBuffer constantBuffer)
        {
            _constantBuffer = constantBuffer;
        }

        internal int MatrixId2ArrayId(int x, int y, int z, int @base)
        {
            return x + y * @base + z * @base * @base;
        }

        internal int MatrixId2ArrayId(Vector3Int id, int @base)
        {
            return MatrixId2ArrayId(id.x, id.y, id.z, @base);
        }

        internal MarchCube GenerateCube(MarchPoint[] points, Vector3Int id)
        {
            var cubePoints = new MarchPoint[8];

            int x, y, z;
            for (int i = 0; i < 8; i++)
            {
                x = i & 1;
                z = (i & 2) >> 1;
                y = (i & 4) >> 2;
                int localXIndex = ((1 - x) & z) | (x & (1 - z));
                int localYIndex = y;
                int localZIndex = 1 - z;
                cubePoints[i] = points[MatrixId2ArrayId(id.x + localXIndex, id.y + localYIndex, id.z + localZIndex, _constantBuffer.pointsPerChunk)];
            }

            return new MarchCube { points = cubePoints };
        }

        internal int GetPointsHash(MarchCube cube, float surfaceValue)
        {
            int cubeIndex = 0;
            // Find which vertices are inside of the surface and which are outside
            for (int i = 0; i < 8; i++)
            {
                if (cube.points[i].value <= surfaceValue)
                {
                    cubeIndex |= 1 << i;
                }
            }
            return cubeIndex;
        }

        internal Vector3 Interpolate(MarchPoint point1, MarchPoint point2, float surfaceValue)
        {
            float lerp = (surfaceValue - point1.value) / (point2.value - point1.value);
            Vector3 deltaPosition = point2.position - point1.position;
            return point1.position + deltaPosition * lerp;

            // return (point1.Position + point2.Position) / 2;
        }

        internal void AddFace(Vector3[] face, int cubeArrayId, ref TempMeshBuffer tempBuffer)
        {
            if (cubeArrayId >= _constantBuffer.cubesPerChunk * _constantBuffer.cubesPerChunk * _constantBuffer.cubesPerChunk)
            {
                return;
            }
            int counter = tempBuffer.counter;
            tempBuffer.vertices.AddRange(face);
            tempBuffer.triangles.AddRange(new int[] { counter, counter + 1, counter + 2 });
            tempBuffer.counter += 3;
        }

        internal void March(MarchCube cube, float surfaceValue, int cubeArrayId, ref TempMeshBuffer tempBuffer)
        {
            int pointsHash = GetPointsHash(cube, surfaceValue);

            // Find which edges are intersected by the surface
            int edgesHash = _constantBuffer.pointsHash2EdgesHash[pointsHash];

            // If the cube is entirely inside or outside of the surface, then there will be no intersections
            if (edgesHash == 0)
            {
                return;
            }

            // Save the triangles that were found. There can be up to five per cube
            for (int i = 0; i < 5; i++)
            {
                if (_constantBuffer.pointsHash2EdgesIndexes[pointsHash, 3 * i] == 20.0f)
                {
                    break;
                }

                var face = new Vector3[3];

                for (int j = 0; j < 3; j++)
                {
                    int edgeIndex = _constantBuffer.pointsHash2EdgesIndexes[pointsHash, 3 * i + j];

                    int edgePoint1Index = _constantBuffer.edgeIndex2PointIndexes[edgeIndex, 0];
                    int edgePoint2Index = _constantBuffer.edgeIndex2PointIndexes[edgeIndex, 1];

                    MarchPoint edgePoint1 = cube.points[edgePoint1Index];
                    MarchPoint edgePoint2 = cube.points[edgePoint2Index];

                    face[j] = Interpolate(edgePoint1, edgePoint2, surfaceValue);
                }

                AddFace(face, cubeArrayId, ref tempBuffer);
            }
        }
    }
}