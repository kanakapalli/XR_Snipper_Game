using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using Vector3 = UnityEngine.Vector3;
using Unity.VisualScripting;
using System.Diagnostics.Contracts;

public class MainWallPassthrough : MonoBehaviour
{
    public Material stencilMaterial;
    public Material passthroughMaterial;
    public GameObject enemy;
    public GameObject contract;
    public GameObject desk;


    private void Update()
    {

    }

    public void Load()
    {


        MRUKRoom room = FindAnyObjectByType<MRUKRoom>();
        GameObject roomObj = room.gameObject;


        Vector2 wallScale = UnityEngine.Vector2.zero;
        MRUKAnchor keyWall = room?.GetKeyWall(out wallScale);
        Vector3 anchorCenter = keyWall.GetAnchorCenter();

        FaceEnemy(roomObj, keyWall);

        MRUKAnchor floorAnchor = room.GetFloorAnchor();

        CreateMeshObject(keyWall, stencilMaterial, false);


        CreateMeshObject(floorAnchor, passthroughMaterial, true);


        GameObject contracti = Instantiate(contract);
        GameObject deski = Instantiate(desk);


        // Set the new GameObject as a child of the GameObject this script is attached to
        contracti.transform.SetParent(room.transform);
        deski.transform.SetParent(room.transform);


        if (desk != null)
        {


            contracti.transform.localPosition = anchorCenter;
            contracti.transform.localRotation = keyWall.transform.localRotation;
            // contracti.transform.localRotation *= Quaternion.Euler(0, +180, 0);
            deski.transform.localPosition = anchorCenter;
            deski.transform.localRotation = keyWall.transform.localRotation;
            //   deski.transform.localRotation *= Quaternion.Euler(0, +90, 0);
        }
        else
        {
            Debug.Log("<color=blue>happy guy not found </color>");
        }

        //  ApplyLayer(roomObj, "wall");

        StartCoroutine(DelayedForeachLoop(room?.GetRoomAnchors(), keyWall));
    }

    private void ApplyLayer(GameObject roomObj, string layerName)
    {

        roomObj.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in roomObj.transform)
            ApplyLayer(child.gameObject, layerName);
    }

    void FaceEnemy(GameObject room, MRUKAnchor wall)
    {

        Debug.Log($"Enemy Position: {enemy.transform.position}, Rotation: {enemy.transform.rotation}, Scale: {enemy.transform.localScale}");
        Debug.Log($"Wall Position: {wall.transform.position}, Rotation: {wall.transform.rotation}, Scale: {wall.transform.localScale}");
        Debug.Log($"Room Position: {room.transform.position}, Rotation: {room.transform.rotation}, Scale: {room.transform.localScale}");



        Vector3 directionToEnemy = (enemy.transform.position - wall.transform.position).normalized;
        directionToEnemy.y = 0;

        // Wall's global forward vector
        Vector3 wallGlobalForward = wall.transform.forward;

        // Direction we want the room to face to align the wall with the enemy
        // Calculating how much we need to rotate the room's forward direction to match the direction to the enemy, considering the wall's current global orientation
        Vector3 roomTargetForwardDirection = Quaternion.FromToRotation(wallGlobalForward, directionToEnemy) * room.transform.forward;

        // Calculate the rotation needed to align the room's current forward direction with the target forward direction
        Quaternion targetRotation = Quaternion.LookRotation(roomTargetForwardDirection, room.transform.up);

        // Apply the rotation to the room


        room.transform.rotation = targetRotation;

        Quaternion targetRotation2 = Quaternion.Euler(room.transform.rotation.eulerAngles.x,
                                                room.transform.rotation.eulerAngles.y + 180,
                                                room.transform.rotation.eulerAngles.z);
        room.transform.rotation = targetRotation2;



    }

    IEnumerator DelayedForeachLoop(List<MRUKAnchor> AllAnchor, MRUKAnchor keyWall)
    {
        Debug.Log("<color=blue>starting delay 10 f </color>");
        yield return new WaitForSeconds(1f);

        foreach (MRUKAnchor anchor in AllAnchor)
        {

            // Assuming each MRUKAnchor has a 'name' property you want to print.
            // Adjust the property to match what you actually need to print.
            Debug.Log("Anchor Name: " + anchor.name);
            if (anchor != keyWall)
            {

                anchor.gameObject.layer = LayerMask.NameToLayer("wall");
                foreach (Transform child in anchor.gameObject.transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("wall");
                }
            }

        }





    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
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

