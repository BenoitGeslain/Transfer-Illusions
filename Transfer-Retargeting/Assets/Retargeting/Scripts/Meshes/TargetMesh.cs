﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
public class TargetMesh : MonoBehaviour {
	
	public Material[] materials;

	public float size = 1f;
	float l = 0.2f;

    void Start() {
        createMesh();
    }

    void createMesh() {
        Vector3[] vertices = {
        	new Vector3(0.5f*size, 0, 0.5f*size),
        	new Vector3(0.5f*size-l*size, 0, 0.5f*size),
        	new Vector3(0.5f*size, 0, -0.5f*size),
        	new Vector3(0.5f*size-l*size, 0, -0.5f*size),
        	new Vector3(0.5f*size-l*size, 0, -0.5f*size+l*size),

        	new Vector3(-0.5f*size+l*size, 0, -0.5f*size+l*size),
        	new Vector3(-0.5f*size+l*size, 0, -0.5f*size),
        	new Vector3(-0.5f*size*size, 0, -0.5f*size),
        	new Vector3(-0.5f*size, 0, 0.5f*size),
        	new Vector3(-0.5f*size+l*size, 0, 0.5f*size),

        	new Vector3(-0.5f*size+l*size, 0, 0.5f*size-l*size),
        	new Vector3(0.5f*size-l*size, 0, 0.5f*size-l*size),
        };

        for (int i=0; i<vertices.Length; i++)
        	vertices[i].y =- 0.5f*size;

        int[] triangles = {
        	0, 3, 1,
        	3, 0, 2,

        	4, 3, 6,
        	6, 5, 4,

        	6, 7, 9,
        	8, 9, 7,

        	11, 4, 10,
        	5, 10, 4,

        	1, 10, 9,
        	10, 1, 11
        };

        Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.subMeshCount = 3;

		int[] subTriangles = new int[18];	// U bleu
		Array.Copy(triangles, 0, subTriangles, 0, 18);
		mesh.SetTriangles(subTriangles, 0);

		subTriangles = new int[6];			// Numéro
		Array.Copy(triangles, 18, subTriangles, 0, 6);
		mesh.SetTriangles(subTriangles, 1);

		subTriangles = new int[6];			// Bande blanche
		Array.Copy(triangles, 24, subTriangles, 0, 6);
		mesh.SetTriangles(subTriangles, 2);

		Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2((-vertices[i].x*1.25f+0.5f*1.025f), -vertices[i].z*1.25f-0.5f*1.025f);
        }
        mesh.uv = uvs;

        mesh.RecalculateNormals ();

		MeshRenderer mR = GetComponent<MeshRenderer>();
		mR.sharedMaterials = materials;

		Renderer r = GetComponent<Renderer>();
		r.materials = materials;
    }
}
