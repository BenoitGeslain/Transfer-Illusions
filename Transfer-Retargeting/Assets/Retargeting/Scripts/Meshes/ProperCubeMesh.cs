using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
public class ProperCubeMesh : MonoBehaviour {

	public float size = 1f;

	public Material[] materials;
	void Start () {
		createCube ();
	}

	void createCube () {
		Vector3[] vertices = {
			new Vector3(-0.5f*size, size*0.5f, -size*0.5f),
			new Vector3(-size*0.5f, -size*0.5f, -size*0.5f),
			new Vector3(size*0.5f, size*0.5f, -size*0.5f),
			new Vector3(size*0.5f, -size*0.5f, -size*0.5f),

			new Vector3(-size*0.5f, -size*0.5f, size*0.5f),
			new Vector3(size*0.5f, -size*0.5f, size*0.5f),
			new Vector3(-size*0.5f, size*0.5f, size*0.5f),
			new Vector3(size*0.5f, size*0.5f, size*0.5f),

			new Vector3(-size*0.5f, size*0.5f, -size*0.5f),
			new Vector3(size*0.5f, size*0.5f, -size*0.5f),

			new Vector3(-size*0.5f, size*0.5f, -size*0.5f),
			new Vector3(-size*0.5f, size*0.5f, size*0.5f),

			new Vector3(size*0.5f, size*0.5f, -size*0.5f),
			new Vector3(size*0.5f, size*0.5f, size*0.5f),
		};
		/*Vector3[] vertices = {
			new Vector3(0, size, 0),
			new Vector3(0, 0, 0),
			new Vector3(size, size, 0),
			new Vector3(size, 0, 0),

			new Vector3(0, 0, size),
			new Vector3(size, 0, size),
			new Vector3(0, size, size),
			new Vector3(size, size, size),

			new Vector3(0, size, 0),
			new Vector3(size, size, 0),

			new Vector3(0, size, 0),
			new Vector3(0, size, size),

			new Vector3(size, size, 0),
			new Vector3(size, size, size),
		};*/

		int[] triangles = {
			4, 5, 6, // back
			5, 7, 6,
			1, 2, 3, // front
			0, 2, 1,
			6, 7, 8, //top
			7, 9 ,8, 
			1, 3, 4, //bottom
			3, 5, 4,
			1, 11,10,// left
			1, 4, 11,
			3, 12, 5,//right
			5, 12, 13
		};

		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.subMeshCount = 2;

		int[] subTriangles = new int[6];	// Front face submesh
		Array.Copy(triangles, 0, subTriangles, 0, 6);
		mesh.SetTriangles(subTriangles, 0);

		subTriangles = new int[30];			// Rest of cube submesh
		Array.Copy(triangles, 6, subTriangles, 0, 30);
		mesh.SetTriangles(subTriangles, 1);

		Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(-vertices[i].x+0.5f, vertices[i].y+0.5f);
        }
        mesh.uv = uvs;
		
		mesh.RecalculateNormals ();

		MeshRenderer mR = GetComponent<MeshRenderer>();
		mR.sharedMaterials = materials;

		Renderer r = GetComponent<Renderer>();
		r.materials = materials;
	}
}