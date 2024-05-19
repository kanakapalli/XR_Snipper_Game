using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float jumpForce = 10f;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private PhotonView photonView;
    private bool isGameOver = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (photonView.IsMine && !isGameOver)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            gameManager.PlayerLost(photonView.IsMine);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(isGameOver);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            isGameOver = (bool)stream.ReceiveNext();
        }
    }
}