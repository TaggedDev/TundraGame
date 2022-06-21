using UnityEngine;

namespace Environment
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier,
            AnimationCurve heightCurve, int levelOfDetail) {
            
            AnimationCurve heightCurveKeys = new AnimationCurve (heightCurve.keys);

            int width = heightMap.GetLength (0);
            int height = heightMap.GetLength (1);
            float topLeftX = (width - 1) / -2f;
            float topLeftZ = (height - 1) / 2f;

            int meshSimplificationIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
            int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

            MeshData meshData = new MeshData (verticesPerLine, verticesPerLine);
            int vertexIndex = 0;

            for (int y = 0; y < height; y += meshSimplificationIncrement) {
                for (int x = 0; x < width; x += meshSimplificationIncrement)
                {
                    meshData.Vertices[vertexIndex] = new Vector3(topLeftX + x,
                        heightCurveKeys.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                    meshData.Uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

                    if (x < width - 1 && y < height - 1)
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1,
                            vertexIndex + verticesPerLine);
                        meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }
            return meshData;
        }
    }

    public class MeshData {
        private readonly int[] _triangles;
        private int _triangleIndex;

        public Vector3[] Vertices { get; }

        public Vector2[] Uvs { get; }


        public MeshData(int meshWidth, int meshHeight) {
            Vertices = new Vector3[meshWidth * meshHeight];
            Uvs = new Vector2[meshWidth * meshHeight];
            _triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
        }

        public void AddTriangle(int a, int b, int c) {
            _triangles [_triangleIndex] = a;
            _triangles [_triangleIndex + 1] = b;
            _triangles [_triangleIndex + 2] = c;
            _triangleIndex += 3;
        }

        public Mesh CreateMesh() {
            Mesh mesh = new Mesh
            {
                vertices = Vertices,
                triangles = _triangles,
                uv = Uvs
            };
            mesh.RecalculateNormals ();
            return mesh;
        }
    }
}