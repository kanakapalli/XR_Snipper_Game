using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject obstaclesPrefab;
    [SerializeField] private float obstaclesSpawnDelay = 2f;
    private bool gameStarted = false;
    private int playerLostCount = 0;

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
        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (gameStarted && PhotonNetwork.IsMasterClient)
        {
            SpawnObstacles();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        InvokeRepeating(nameof(SpawnObstacles), 0f, obstaclesSpawnDelay);
    }

    private void SpawnObstacles()
    {
        Vector3 position = new Vector3(Camera.main.transform.position.x + 10f, Random.Range(-3f, 3f), 0f);
        PhotonNetwork.Instantiate(obstaclesPrefab.name, position, Quaternion.identity);
    }

    public void PlayerLost(bool isMine)
    {
        playerLostCount++;
        if (playerLostCount == 2)
        {
            gameStarted = false;
            CancelInvoke(nameof(SpawnObstacles));
            PhotonNetwork.LeaveRoom();
            EventSystem.Instance.TriggerEvent("GameOver", isMine);
        }
    }
}