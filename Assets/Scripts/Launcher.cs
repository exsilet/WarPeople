using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomJoinUI;
    [SerializeField] private GameObject buttonLoadArena;
    [SerializeField] private GameObject buttonJoinRoom;
    [SerializeField] private GameObject controlPanel;

    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private string playerName;
    [SerializeField] private string roomName;
    [SerializeField] private string gameVersion;
    [SerializeField] private TMP_Text connectionStatus;
    [SerializeField] private TMP_Text playerStatus;
    [SerializeField] private bool isConnecting;

    void Start() 
    {
        //1
        PlayerPrefs.DeleteAll(); 

        Debug.Log("Connecting to Photon Network");

        //2
        roomJoinUI.SetActive(false);
        buttonLoadArena.SetActive(false);

        //3
        ConnectToPhoton();
    }

    private void ConnectToPhoton()
    {
        connectionStatus.text = "Connecting...";
        PhotonNetwork.GameVersion = gameVersion; //1
        PhotonNetwork.ConnectUsingSettings(); //2
    }

    void Awake()
    {
        //4 
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    // Helper Methods
    public void SetPlayerName()
    {
        playerName = _playerName.text;
    }

    public void SetRoomName()
    {
        roomName = _roomName.text;
    }
    
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = playerName; //1
            Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room " + _roomName.text);
            RoomOptions roomOptions = new RoomOptions(); //2
            TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default); //3
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby); //4
        }
    }

    public void LoadArena()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            playerStatus.text = "Minimum 2 Players required to Load Arena!";
        }
    }
    
    public override void OnConnected()
    {
        // 1
        base.OnConnected();
        // 2
        connectionStatus.text = "Connected to Photon!";
        connectionStatus.color = Color.green;
        roomJoinUI.SetActive(true);
        buttonLoadArena.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 3
        isConnecting = false;
        controlPanel.SetActive(true);
        Debug.LogError("Disconnected. Please check your Internet connection.");
    }

    public override void OnJoinedRoom()
    {
        // 4
        if (PhotonNetwork.IsMasterClient)
        {
            buttonLoadArena.SetActive(true);
            buttonJoinRoom.SetActive(false);
            playerStatus.text = "You are Lobby Leader";
        }
        else
        {
            playerStatus.text = "Connected to Lobby";
        }
    }
}