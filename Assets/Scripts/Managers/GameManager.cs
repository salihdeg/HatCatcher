using Controllers;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviourPun
    {
        public static GameManager Instance;

        [Header("Stats")]
        public bool gameEnd = false;
        public float timeToWin = 60;
        public float invictibleDuration = 1.5f;

        private float _hatPickupTime;

        [Header("Players")]
        public string playerPrefabLocation;

        public Transform[] spawnPoints;
        public PlayerController[] players;
        public int playerIdWithHat;

        private int _playersInGame;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            players = new PlayerController[PhotonNetwork.PlayerList.Length];

            photonView.RPC("IAmInGame", RpcTarget.All);
        }

        #region PunRPC Functions

        [PunRPC]
        private void IAmInGame()
        {
            _playersInGame++;

            //If All Players Connected Spawn Players
            if (_playersInGame == PhotonNetwork.PlayerList.Length)
            {
                SpawnPlayer();
            }
        }

        [PunRPC]
        public void GiveHat(int playerId, bool initialGive)
        {
            if (!initialGive)
                GetPlayer(playerIdWithHat).SetHat(false);

            playerIdWithHat = playerId;
            GetPlayer(playerId).SetHat(true);
            _hatPickupTime = Time.time;
        }

        [PunRPC]
        private void WinGame(int playerId)
        {
            gameEnd = true;

            PlayerController player = GetPlayer(playerId);

            Invoke("BackToMenu", 3f);
        }

        #endregion

        public void BackToMenu()
        {
            PhotonNetwork.LeaveRoom();
            NetworkManager.Instance.ChangeScene(0);
        }

        public bool CanGetHat()
        {
            if (Time.time > _hatPickupTime + invictibleDuration)
                return true;

            return false;
        }

        private void SpawnPlayer()
        {
            GameObject playerObj =
                PhotonNetwork.Instantiate(
                    playerPrefabLocation,
                    spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                    Quaternion.identity);

            PlayerController controller = playerObj.GetComponent<PlayerController>();

            controller.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }



        public PlayerController GetPlayer(int playerId)
        {
            return players.First(p => p.id == playerId);
        }

        public PlayerController GetPlayer(GameObject playerObj)
        {
            return players.First(p => p.gameObject == playerObj);
        }
    }
}