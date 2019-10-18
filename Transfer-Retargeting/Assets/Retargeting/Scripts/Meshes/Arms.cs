using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;

	GameObject obstacles;

	AudioSource collisionSound;

	ScoreManager scoreManager;

    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        obstacles = GameObject.Find("Obstacles");

        collisionSound = obstacles.GetComponent<AudioSource>();

        scoreManager = GameObject.Find("World").GetComponent<ScoreManager>();
    }

	public void UpdateCollider() {
		Mesh colliderMesh = new Mesh();
		meshRenderer.BakeMesh(colliderMesh);
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = colliderMesh;
	}

	void OnTriggerEnter (Collider col) {
        if(col.transform.parent.gameObject == obstacles) {
            print("colliding");
            scoreManager.Collision();
            collisionSound.Play();
        }
    }
}
