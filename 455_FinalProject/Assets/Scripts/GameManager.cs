using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;
using System;

namespace GameManager
{
    public class GameManager : PunBehaviour
    {

        private string charPrefName;
        [SerializeField] Transform spawnPoint;

        [SerializeField] Button loginButton;
        [SerializeField] Button registerButton;
        [SerializeField] Button createroomButton;
        [SerializeField] Button joinroomButton;
        [SerializeField] Button readyButton;
        [SerializeField] Button startButton;
        [SerializeField] Button refreshRoomButton;
        [SerializeField] GameObject loginPanel;
        [SerializeField] GameObject mainMenuPanel;
        [SerializeField] GameObject errPanel;
        [SerializeField] GameObject startPanel;
        [SerializeField] Text statusText;
        [SerializeField] Text roomlist;
        [SerializeField] Text playerInEachRoom;
        [SerializeField] Text playerInRoom;

        // public GameObject[] gameStage;

        [SerializeField] InputField roomName;

        private GameObject ownerChar;

        private string playername;
        private string heroname;
        private bool isReady = false;
        // private bool isStageClear = false;

        public static GameManager instance;

        private void Awake()
        {
            instance = this;

            if (PhotonNetwork.connected == false)
            {
                PhotonNetwork.ConnectUsingSettings("1.0");
            }

            // DontDestroyOnLoad(this);
        }

        void Start()
        {
            loginButton.onClick.AddListener(OnLoginClick);
            createroomButton.onClick.AddListener(OnJoinOrCreate);
            joinroomButton.onClick.AddListener(OnJoinOrCreate);
            startButton.onClick.AddListener(OnStart);
            refreshRoomButton.onClick.AddListener(OnRefreshRoom);

            PhotonNetwork.automaticallySyncScene = true;
        }

        private void Update()
        {
            // Debug.Log(PhotonNetwork.room);
            statusText.text = PhotonNetwork.connectionState.ToString();
            // OnGameplay();
            // OnStageClear();
        }

        void OnLoginClick()
        {
            playername = DatabaseConnection.instance.playerName;
            heroname = DatabaseConnection.instance.heroName;

            // PhotonNetwork.player.NickName = playername;
            // PhotonNetwork.playerName = playername;
            // photonView.owner.NickName = playername;
        }

        void OnRefreshRoom()
        {
            roomlist.text = "";
            playerInEachRoom.text = "";
            for (int i = 0; i < PhotonNetwork.GetRoomList().Length; i++)
            {
                RoomInfo[] rooms = PhotonNetwork.GetRoomList();
                // roomlist.text += $"{rooms[i].Name}\n";
                // playerInRoom.text += $"{rooms[i].MaxPlayers}\n";
                if (rooms[i].PlayerCount < 2)
                {
                    roomlist.text += $"{rooms[i].Name}\n";
                    playerInEachRoom.text += $"{rooms[i].PlayerCount}/{rooms[i].MaxPlayers}\n";
                }
            }
        }

        void OnJoinOrCreate()
        {
            string roomname = roomName.text;
            // if (PhotonNetwork.room == null)
            if (PhotonNetwork.insideLobby)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 2;
                roomOptions.IsOpen = true;
                roomOptions.IsVisible = true;
                // if (PhotonNetwork.room.Name == roomname && PhotonNetwork.room.MaxPlayers < 2)
                // {
                PhotonNetwork.JoinOrCreateRoom(roomname, roomOptions, TypedLobby.Default);
                // }
            }

            mainMenuPanel.SetActive(false);
            startPanel.SetActive(true);
            // gameStage[0].SetActive(true);
        }

        // void OnReady()
        // {
        //     isReady = true;
        //     readyButton.gameObject.SetActive(true);
        // }

        void OnStart()
        {
            isReady = true;
            if (isReady)
            {
                // if (PhotonNetwork.isMasterClient)
                // {
                //     startButton.gameObject.SetActive(true);
                // }
                OnGameplay();
            }
        }

        void OnGameplay()
        {
            Debug.Log(PhotonNetwork.room);

            if (PhotonNetwork.isMasterClient)
            {
                // startButton.gameObject.SetActive(true);
                PhotonNetwork.LoadLevel(1);
            }


            // if (PhotonNetwork.inRoom)
            // {
            //     PhotonNetwork.playerName = DatabaseConnection.instance.playerName;

            //     playerInRoom.text = "";
            //     foreach (PhotonPlayer player in PhotonNetwork.playerList)
            //     {
            //         // Debug.Log(player.NickName);
            //         playerInRoom.text += $"Player : {player.NickName}\n";
            //     }

            //     if (PhotonNetwork.room.PlayerCount == 2)
            //     {
            //         startPanel.SetActive(false);
            //         gameStage[0].SetActive(true);
            //         if (ownerChar == null)
            //         {
            //             // PhotonNetwork.LoadLevel(1);
            //             charPrefName = DatabaseConnection.instance.heroName;
            //             ownerChar = PhotonNetwork.Instantiate(charPrefName, spawnPoint.position, spawnPoint.rotation, 0);
            //         }
            //     }
            // }
        }
    }
}

