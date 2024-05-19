
using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Material = UnityEngine.Material;

public class OpenKeyWall : MonoBehaviour
{

    public Material testMaterial;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateKeyWall()
    {
        MRUKRoom room = FindAnyObjectByType<MRUKRoom>();

        Vector2 wallSize = Vector2.zero;
        MRUKAnchor keywall = room.GetKeyWall(out wallSize);

        foreach (Transform child in keywall.gameObject.transform)
        {
            // Try to get an existing MeshRenderer
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            MeshCollider meshCollider = child.GetComponent<MeshCollider>();

            // If a MeshCollider component exists, remove it
            if (meshCollider != null)
            {
                GameObject.Destroy(meshCollider);
            }

            // If there isn't a MeshRenderer component already, add one
            if (meshRenderer == null)
            {
                meshRenderer = child.gameObject.AddComponent<MeshRenderer>();
            }

            // Before setting the material, check if testMaterial is assigned to avoid NullReferenceException
            if (testMaterial != null)
            {
                meshRenderer.material = testMaterial;
            }
            else
            {
                Debug.Log("There is no material assigned.");
            }
        }
    }

    private void CreateMeshObject(MRUKAnchor anchorInfo, Material material, bool addCollider)
    {
        GameObject newGameObject = new GameObject(anchorInfo.name + "_EffectMesh_ak");
        newGameObject.transform.SetParent(anchorInfo.transform, false);
        if (material != null)
        {
            MeshRenderer meshRenderer = newGameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
        }



        MeshFilter meshFilter = newGameObject.AddComponent<MeshFilter>();
        Mesh newMesh = new Mesh();
        meshFilter.mesh = newMesh;

        // Assuming anchorInfo.PlaneBoundary2D is a List<Vector2> representing the plane boundary
        List<Vector3> vertices = new List<Vector3>();
        foreach (var point in anchorInfo.PlaneBoundary2D)
        {
            // Convert 2D points to 3D vertices
            vertices.Add(new Vector3(point.x, point.y, 0)); // Assuming Z = 0 for the plane
        }

        // Generate triangles for the mesh
        List<int> triangles = new List<int>();
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            // Creating a fan based on the first vertex
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = triangles.ToArray();
        newMesh.RecalculateNormals(); // To ensure the mesh interacts with lighting correctly

        if (addCollider)
        {
            MeshCollider meshCollider = newGameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = newMesh;
        }
    }

}
