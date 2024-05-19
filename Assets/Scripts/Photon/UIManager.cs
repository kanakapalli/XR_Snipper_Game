using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text connectionStatusText;
    [SerializeField] private TMP_Text roomInfoText;
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMP_Text gameOverText;

    private void Start()
    {
        UpdateConnectionStatus();
        UpdateRoomInfo();
        ToggleLobbyUI(true);
        ToggleGameUI(false);
        ToggleGameOverUI(false);

        EventSystem.Instance.RegisterEvent("GameOver", OnGameOver);
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            UpdateConnectionStatus();
            UpdateRoomInfo();
        }
    }

    private void UpdateConnectionStatus()
    {
        connectionStatusText.text = $"Connection Status: {PhotonNetwork.NetworkClientState}";
    }

    private void UpdateRoomInfo()
    {
        if (PhotonNetwork.InRoom)
        {
            roomInfoText.text = $"Room: {PhotonNetwork.CurrentRoom.Name} | Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        else
        {
            roomInfoText.text = "Not in a room";
        }
    }

    private void ToggleLobbyUI(bool value)
    {
        lobbyUI.SetActive(value);
    }

    private void ToggleGameUI(bool value)
    {
        gameUI.SetActive(value);
    }

    private void ToggleGameOverUI(bool value)
    {
        gameOverUI.SetActive(value);
    }

    public void OnJoinRoomButtonClick()
    {
        NetworkManager.Instance.CreateOrJoinRoom();
        ToggleLobbyUI(false);
        ToggleGameUI(true);
    }

    private void OnGameOver(object data)
    {
        bool iWon = (bool)data;
        gameOverText.text = iWon ? "You Won!" : "You Lost!";
        ToggleGameUI(false);
        ToggleGameOverUI(true);
    }
}