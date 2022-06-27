using UnityEngine;

namespace Environment.Terrain
{
    public static class MeshGenerator
    {
	    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier,
		    AnimationCurve heightCurveKeys, int levelOfDetail)
	    {
		    AnimationCurve heightCurve = new AnimationCurve(heightCurveKeys.keys);

		    int meshSimplificationIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;

		    int borderedSize = heightMap.GetLength(0);
		    int meshSize = borderedSize - 2 * meshSimplificationIncrement;
		    int meshSizeUnsimplified = borderedSize - 2;

		    float topLeftX = (meshSizeUnsimplified - 1) / -2f;
		    float topLeftZ = (meshSizeUnsimplified - 1) / 2f;


		    int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

		    MeshData meshData = new MeshData(verticesPerLine);

		    int[,] vertexIndicesMap = new int[borderedSize, borderedSize];
		    int meshVertexIndex = 0;
		    int borderVertexIndex = -1;

		    for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
		    {
			    for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
			    {
				    bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

				    if (isBorderVertex)
				    {
					    vertexIndicesMap[x, y] = borderVertexIndex;
					    borderVertexIndex--;
				    }
				    else
				    {
					    vertexIndicesMap[x, y] = meshVertexIndex;
					    meshVertexIndex++;
				    }
			    }
		    }

		    for (int y = 0; y < borderedSize; y += meshSimplificationIncrement)
		    {
			    for (int x = 0; x < borderedSize; x += meshSimplificationIncrement)
			    {
				    int vertexIndex = vertexIndicesMap[x, y];
				    Vector2 percent = new Vector2((x - meshSimplificationIncrement) / (float)meshSize,
					    (y - meshSimplificationIncrement) / (float)meshSize);
				    float height = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
				    Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, height,
					    topLeftZ - percent.y * meshSizeUnsimplified);

				    meshData.AddVertex(vertexPosition, percent, vertexIndex);

				    if (x < borderedSize - 1 && y < borderedSize - 1)
				    {
					    int a = vertexIndicesMap[x, y];
					    int b = vertexIndicesMap[x + meshSimplificationIncrement, y];
					    int c = vertexIndicesMap[x, y + meshSimplificationIncrement];
					    int d = vertexIndicesMap[x + meshSimplificationIncrement, y + meshSimplificationIncrement];
					    meshData.AddTriangle(a, d, c);
					    meshData.AddTriangle(d, a, b);
				    }

				    vertexIndex++;
			    }
		    }

		    meshData.ProcessMeshType();
		    return meshData;
	    }

	    public static float GetMapCenter(MapGenerator generator)
	    {
		    AnimationCurve heightCurve = new AnimationCurve(generator.MeshHeightCurve.keys);
		    int chunkSize = 47;
		    int seed = generator.Seed;
		    float scale = generator.NoiseScale;
		    int octaves = generator.Octaves;
		    float persistance = generator.Persistance;
		    float lacunarity = generator.Lacunarity;
		    Vector2 offset = generator.Offset;
		    float heightValue = Noise.GetNoiseValue(chunkSize/2, chunkSize/2, chunkSize, chunkSize,
			    seed, scale, octaves, persistance, lacunarity, offset, NormalizeMode.Local);
		    float height = heightCurve.Evaluate(heightValue) * generator.HeightMultiplier;
		    return height; 
	    }
	   
	    
    }

    public class MeshData {
        private readonly int[] _triangles;
        private Vector3[] _vertices;
        private Vector2[] _uvs;
        
        private readonly Vector3[] _borderVertices;
        private readonly int[] _borderTriangles;
        
        private Vector3[] _bakedNormals;

        private int _borderTriangleIndex;
        private int _triangleIndex;
        
        public MeshData(int verticesPerLine) {
            _vertices = new Vector3[verticesPerLine * verticesPerLine];
            _uvs = new Vector2[verticesPerLine * verticesPerLine];
            _triangles = new int[(verticesPerLine-1)*(verticesPerLine-1)*6];
            _borderVertices = new Vector3[verticesPerLine * 4 + 4];
            _borderTriangles = new int[24 * verticesPerLine];
        }

        public void AddTriangle(int a, int b, int c) {
            if (a < 0 || b < 0 || c < 0)
            {
                _borderTriangles [_borderTriangleIndex] = a;
                _borderTriangles[_borderTriangleIndex + 1] = b;
                _borderTriangles[_borderTriangleIndex + 2] = c;
                _borderTriangleIndex += 3;
            }
            else
            {
                _triangles [_triangleIndex] = a;
                _triangles [_triangleIndex + 1] = b;
                _triangles [_triangleIndex + 2] = c;
                _triangleIndex += 3;   
            }
        }

        public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
        {
            if (vertexIndex < 0)
            {
                _borderVertices[-vertexIndex - 1] = vertexPosition;
            }
            else
            {
                _vertices[vertexIndex] = vertexPosition;
                _uvs[vertexIndex] = uv;
            }
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh
            {
                vertices = _vertices,
                triangles = _triangles,
                uv = _uvs
            };
            mesh.RecalculateNormals();
            return mesh;
        }

        public void ProcessMeshType()
        {
	        FlatShading();
        }

        private void FlatShading()
        {
	        Vector3[] flatShaded = new Vector3[_triangles.Length];
	        Vector2[] flatShadedUV = new Vector2[_triangles.Length];
	        for (int i = 0; i < _triangles.Length; i++)
	        {
		        flatShaded[i] = _vertices[_triangles[i]];
		        flatShadedUV[i] = _uvs[_triangles[i]];
		        _triangles[i] = i;
	        }

	        _vertices = flatShaded;
	        _uvs = flatShadedUV;
        }
    }
}