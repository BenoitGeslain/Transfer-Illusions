using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
public class CubeMesh : MonoBehaviour {
	public float size;

	public Material[] materials;
    // Start is called before the first frame update
    void Start() {
    	createCube();
    }

    // Update is called once per frame
    void createCube() {
        Vector3[] vertices = {
			new Vector3(-0.5f, -0.5f, -0.5f)*size,	// 0
			new Vector3(0.5f, -0.5f, -0.5f)*size,	// 1
			new Vector3(0.5f, 0.5f, -0.5f)*size,	// 2
			new Vector3(-0.5f, 0.5f, -0.5f)*size,	// 3

			new Vector3(0.5f, 0.5f, -0.5f)*size,	// 2
			new Vector3(-0.5f, 0.5f, -0.5f)*size,	// 3
			new Vector3(-0.5f, 0.5f, 0.5f)*size,	// 4
			new Vector3(0.5f, 0.5f, 0.5f)*size,		// 5

			new Vector3(0.5f, -0.5f, -0.5f)*size,	// 1
			new Vector3(0.5f, 0.5f, -0.5f)*size,	// 2
			new Vector3(0.5f, 0.5f, 0.5f)*size,		// 5
			new Vector3(0.5f, -0.5f, 0.5f)*size,	// 6

			new Vector3(-0.5f, -0.5f, -0.5f)*size,	// 0
			new Vector3(-0.5f, 0.5f, -0.5f)*size,	// 3
			new Vector3(-0.5f, 0.5f, 0.5f)*size,	// 4
			new Vector3(-0.5f, -0.5f, 0.5f)*size, 	// 7

			new Vector3(-0.5f, -0.5f, -0.5f)*size,	// 0
			new Vector3(0.5f, -0.5f, -0.5f)*size,	// 1
			new Vector3(0.5f, -0.5f, 0.5f)*size,	// 6
			new Vector3(-0.5f, -0.5f, 0.5f)*size, 	// 7

			new Vector3(-0.5f, 0.5f, 0.5f)*size,	// 4
			new Vector3(0.5f, 0.5f, 0.5f)*size,		// 5
			new Vector3(0.5f, -0.5f, 0.5f)*size,	// 6
			new Vector3(-0.5f, -0.5f, 0.5f)*size, 	// 7
		};

		int[] triangles = {
			0, 2, 1, //face front
			0, 3, 2,

			4, 5, 6, //face top
			4, 6, 7,

			8, 9, 10, //face right
			8, 10, 11,

			12, 15, 14, //face left
			12, 14, 13,

			16, 18, 19, //face bottom
			16, 17, 18,

			20, 22, 21, //face back
			20, 23, 22
		};

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.subMeshCount = 2;

		int[] subTriangles = new int[6];	// Front face submesh
		Array.Copy(triangles, 30, subTriangles, 0, 6);
		mesh.SetTriangles(subTriangles, 0);

		subTriangles = new int[30];			// Rest of cube submesh
		Array.Copy(triangles, 0, subTriangles, 0, 30);
		mesh.SetTriangles(subTriangles, 1);

		mesh.RecalculateNormals();

		Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        mesh.uv = uvs;

		MeshRenderer mR = GetComponent<MeshRenderer>();
		mR.sharedMaterials = materials;

		Renderer r = GetComponent<Renderer>();
		r.materials = materials;
    }
}
