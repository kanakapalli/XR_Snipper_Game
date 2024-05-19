using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class traningspwaning : MonoBehaviour
{

    [SerializeField] Material stencil;

    [SerializeField] GameObject taget;
    [SerializeField] GameObject gun;
    [SerializeField] OVRCameraRig camera ;

    [SerializeField] UnityEvent sceneLoadedEvent = new();



    public void spwanTaning()
    {
        Debug.Log("spwan Taning cALLED");

        MRUK mruk = MRUK.Instance;
        /*mruk.gameObject.transform.position = new Vector3(-160.4f, 0f, 195.9f);*/

        Vector2 vector2 = Vector2.zero;
        MRUKAnchor keyWall = mruk.GetCurrentRoom().GetKeyWall(out vector2);

        // add stencill material to key wall
        ApplyMaterialToGameObject(keyWall.gameObject, stencil);

        // keyWall.gameObject.transform;

    }

    void ApplyMaterialToGameObject(GameObject obj, Material mat)
    {
        Debug.Log("ApplyMaterialToGameObject  cALLED");
        // Apply the material to the parent GameObject
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }

        // Recursively apply the material to all child GameObjects
        foreach (Transform child in obj.transform)
        {
            ApplyMaterialToGameObject(child.gameObject, mat);
        }
    }

    public void invockasd()
    {
        StartCoroutine(ExecuteAfterDelay());
    }
    IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Wait for 5 seconds
        openKeyWall(); // Function to call after the delay
    }

    public void openKeyWall()
    {
        Debug.Log("openKeyWall called");
        
        MRUK mruk = MRUK.Instance;
        MRUKRoom room = mruk.GetCurrentRoom();
        Vector2 wallSize = Vector2.zero;
        MRUKAnchor keywall = room.GetKeyWall(out wallSize);



        if (room != null)
        {

            GameObject childObject = new GameObject("keywall"); // Create a new GameObject named "ChildObjectName"
            childObject.transform.parent = keywall.transform;

            Debug.Log("key wall found");

            GameObject childGameObject = keywall.gameObject.transform.GetChild(0).gameObject;
            Debug.Log($"child object {childGameObject.name}");
            MeshRenderer meshRenderer = childGameObject.GetComponent<MeshRenderer>();
            MeshCollider meshCollider = childGameObject.GetComponent<MeshCollider>();
            if(meshCollider != null) Destroy(meshCollider);
            meshRenderer.material = stencil;


            FaceEnemy(room.gameObject, keywall);
            keywall.GetAnchorCenter();

            //GameObject desk = Instantiate(gun);
            Vector3 keywallCenter = keywall.GetAnchorCenter();
            Vector3 floorAnchor = room.GetFloorAnchor().transform.position;

            //desk.transform.position = new Vector3(keywallCenter.x, floorAnchor.y, keywallCenter.z);
            /*    camera.gameObject.SetActive(true);*/

        }
        else
        {
            Debug.Log($"no room found");
        }
    }

        void FaceEnemy(GameObject room, MRUKAnchor wall)
        {

            Debug.Log($"Enemy Position: {taget.transform.position}, Rotation: {taget.transform.rotation}, Scale: {taget.transform.localScale}");
            Debug.Log($"Wall Position: {wall.transform.position}, Rotation: {wall.transform.rotation}, Scale: {wall.transform.localScale}");
            Debug.Log($"Room Position: {room.transform.position}, Rotation: {room.transform.rotation}, Scale: {room.transform.localScale}");


/*        Vector3 moveValue = new Vector3(camera.gameObject.transform.position.x, room.transform.position.y, camera.gameObject.transform.position.z);
        room.transform.position = moveValue;*/

        Vector3 directionToEnemy = (taget.transform.position - wall.transform.position).normalized;
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


        /*        Vector2 vectorzero = Vector2.zero;
                gun.transform.position = MRUK.Instance.GetCurrentRoom().GetKeyWall(out vectorzero).GetAnchorCenter();*/



         sceneLoadedEvent?.Invoke();




    }

    }
