
using Meta.XR.MRUtilityKit;
using UnityEngine;
using UnityEngine.Events;
using Material = UnityEngine.Material;

public class KeyWall : MonoBehaviour
{
    [SerializeField] private Material materials;
    [SerializeField] private GameObject directionToLookAt;
    [SerializeField] private GameObject deskPrefab;
    [SerializeField] private RotSnap rotSnap;

    private UnityEvent sceneLoadedEvent = new();
    private GameObject directionToLookAtState;
    private MRUKRoom room;
    private MRUKAnchor keywall;

    /*  public void Update()
      {
          if(directionToLookAtState.transform == directionToLookAt.transform)
            {
                return;
            }

          FaceEnemy(room.gameObject, keywall);
      }*/
    public void OpenKeyWall()
    {
        room = FindAnyObjectByType<MRUKRoom>();
        Vector2 wallSize = Vector2.zero;
        keywall = room.GetKeyWall(out wallSize);

        if (room != null)
        {

            GameObject childObject = new GameObject("keywall"); // Create a new GameObject named "ChildObjectName"
            childObject.transform.parent = keywall.transform;

            Debug.Log("key wall found");

            GameObject childGameObject = keywall.gameObject.transform.GetChild(0).gameObject;
            Debug.Log($"child object {childGameObject.name}");
            MeshRenderer meshRenderer = childGameObject.GetComponent<MeshRenderer>();
            MeshCollider meshCollider = childGameObject.GetComponent<MeshCollider>();
            Destroy(meshCollider);
            meshRenderer.material = materials;
            //FaceEnemy(room.gameObject, keywall);
            keywall.GetAnchorCenter();

            GameObject desk = Instantiate(deskPrefab);
            Vector3 keywallCenter = keywall.GetAnchorCenter();
            desk.transform.position = new Vector3(keywallCenter.x, 0.5f, keywallCenter.z);
            rotSnap.ActivateUpdateMode(childGameObject.transform, desk.transform);
        }
    }
    void FaceEnemy(GameObject room, MRUKAnchor wall)
    {

        Debug.Log($"Enemy Position: {directionToLookAt.transform.position}, Rotation: {directionToLookAt.transform.rotation}, Scale: {directionToLookAt.transform.localScale}");
        Debug.Log($"Wall Position: {wall.transform.position}, Rotation: {wall.transform.rotation}, Scale: {wall.transform.localScale}");
        Debug.Log($"Room Position: {room.transform.position}, Rotation: {room.transform.rotation}, Scale: {room.transform.localScale}");



        Vector3 directionToEnemy = (directionToLookAt.transform.position - wall.transform.position).normalized;
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


        sceneLoadedEvent?.Invoke();
    }
}
