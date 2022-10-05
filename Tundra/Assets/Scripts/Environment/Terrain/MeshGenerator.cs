using UnityEngine;

namespace Environment.Terrain
{
	/// <summary>
	/// This class must be in scene only once. Generates Meshes for planes.
	/// </summary>
    public static class MeshGenerator
    {
	    /// <summary>
	    /// Generates MeshData with passed parameters.
	    /// </summary>
	    /// <param name="heightMap">Noise map for current coordinates</param>
	    /// <param name="heightMultiplier">Multiplier of how hilly will terrain be</param>
	    /// <param name="heightCurveKeys">Curve for certain heightmap</param>
	    /// <returns>MeshData to build chunk mesh from</returns>
	    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier,
		    AnimationCurve heightCurveKeys)
	    {
		    AnimationCurve heightCurve = new AnimationCurve(heightCurveKeys.keys);

		    int meshSimplificationIncrement = 1;

		    int borderedSize = heightMap.GetLength(0);
		    int meshSize = borderedSize - 2;
		    int meshSizeUnsimplified = borderedSize - 2;

		    float topLeftX = (meshSizeUnsimplified - 1) / -2f;
		    float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

		    MeshData meshData = new MeshData(meshSize);

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
					    int b = vertexIndicesMap[x + 1, y];
					    int c = vertexIndicesMap[x, y + 1];
					    int d = vertexIndicesMap[x + 1, y + 1];
					    meshData.AddTriangle(a, d, c);
					    meshData.AddTriangle(d, a, b);
				    }
			    }
		    }

		    //meshData.ProcessMeshType();
		    return meshData;
	    }
	}

	/// <summary>
	/// Describes the values to create and apply Mesh.
	/// </summary>
    public class MeshData {
        private readonly int[] _triangles;
        private Vector3[] _vertices;
        private Vector2[] _uvs;
        
        private readonly Vector3[] _borderVertices;
        private readonly int[] _borderTriangles;
        
        private Vector3[] _bakedNormals;

        private int _borderTriangleIndex;
        private int _triangleIndex;
        
        /// <summary>
        /// MeshData constructor.
        /// </summary>
        /// <param name="verticesPerLine"></param>
        public MeshData(int verticesPerLine) {
            _vertices = new Vector3[verticesPerLine * verticesPerLine];
            _uvs = new Vector2[verticesPerLine * verticesPerLine];
            _triangles = new int[(verticesPerLine-1)*(verticesPerLine-1)*6];
            _borderVertices = new Vector3[verticesPerLine * 4 + 4];
            _borderTriangles = new int[24 * verticesPerLine];
        }

        /// <summary>
        /// Adds triangle to triangles array for Meshes.
        /// </summary>
        /// <param name="a">Index of A</param>
        /// <param name="b">Index of B</param>
        /// <param name="c">Index of C</param>
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
	            _triangles[_triangleIndex] = a;
	            _triangles[_triangleIndex + 1] = b;
	            _triangles[_triangleIndex + 2] = c;
                _triangleIndex += 3;   
            }
        }

        /// <summary>
        /// Adds points by position to array.
        /// </summary>
        /// <param name="vertexPosition">Position of a point</param>
        /// <param name="uv">Texture coordinates</param>
        /// <param name="vertexIndex">Index of this index corresponding to whole array of vertexes</param>
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
        
        /// <summary>
        /// Generates mesh from saved values.
        /// </summary>
        /// <returns>Mesh to apply for GameObject</returns>
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

        /*// <summary>
        /// Applies low poly appearance to triangles.
        /// </summary>
        public void ProcessMeshType()
        {
	        FlatShading();
        }*/
        
        /// <summary>
        /// Makes triangles' shadows look more in low-poly style.
        /// </summary>
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