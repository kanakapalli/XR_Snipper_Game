using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }
    public GameObject PlayerPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon Lobby");
        CreateOrJoinRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Created room: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(PlayerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    internal void CreateOrJoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2, IsVisible = true, IsOpen = true };
            PhotonNetwork.JoinOrCreateRoom("Flappy Bird", roomOptions, null);
        }
    }
}