using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0f";
    private string userId = "KAIST";
    // Start is called before the first frame update
    void Awake()
    {
        // Scene sync
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        Debug.Log(PhotonNetwork.SendRate); //(30fps) communication count with photon server

        //CONNECT TO MASTER
        PhotonNetwork.ConnectUsingSettings();
    }

    // MASTER --> LOBBY --> ROOM
    //callback methods
    //1. Master callback
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); //false
        //JOIN LOBBY
        PhotonNetwork.JoinLobby();

    }
    //2. Lobby callback
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwor.InLobby = {PhotonNetwork.InLobby}"); //true
        //JOIN RAMDOM ROOM
        PhotonNetwork.JoinRandomRoom(); //or join room
    }
    //3. Random room callback
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandomFailed {returnCode}:{message}"); //true

        //SET ROOM OPTION
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;
        ro.IsOpen = true;
        //CREATE ROOM
        PhotonNetwork.CreateRoom("My Room", ro);

    }

    //4. Create room callback
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room !");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //5. Join Room callback
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player count = {PhotonNetwork.CurrentRoom.PlayerCount}");
        //verify joined users' nickname
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            //if same nickname, add number at the end (This is called ActorNumber)
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }
        //SPAWN PLAYER
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }
}