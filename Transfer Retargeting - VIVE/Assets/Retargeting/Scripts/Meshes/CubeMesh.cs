using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
public class CubeMesh : MonoBehaviour
{
	public float size;

	public Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
    	createCube();
    }

    // Update is called once per frame
    void createCube()
    {
        Vector3[] vertices = {
			new Vector3(-0.5f, -0.5f, -0.5f)*size,
			new Vector3(0.5f, -0.5f, -0.5f)*size,
			new Vector3(0.5f, 0.5f, -0.5f)*size,
			new Vector3(-0.5f, 0.5f, -0.5f)*size,
			new Vector3(-0.5f, 0.5f, 0.5f)*size,
			new Vector3(0.5f, 0.5f, 0.5f)*size,
			new Vector3(0.5f, -0.5f, 0.5f)*size,
			new Vector3(-0.5f, -0.5f, 0.5f)*size,
		};

		int[] triangles = {
			5, 4, 7, //face back
			5, 7, 6,
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			0, 6, 7, //face bottom
			0, 1, 6
		};

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.RecalculateNormals();

		mesh.subMeshCount = 2;

		int[] subTriangles = new int[6];	// Front face submesh
		Array.Copy(triangles, 0, subTriangles, 0, 6);
		mesh.SetTriangles(subTriangles, 0);

		subTriangles = new int[30];
		Array.Copy(triangles, 6, subTriangles, 0, 30);
		mesh.SetTriangles(subTriangles, 1);

		MeshRenderer mR = GetComponent<MeshRenderer>();
		mR.sharedMaterials = materials;

		Renderer r = GetComponent<Renderer>();
		r.materials = materials;
    }
}
