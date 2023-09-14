using Photon.Pun;
using UnityEngine;

namespace Managers
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom(string roomName)
        {
            PhotonNetwork.CreateRoom(roomName);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void ChangeScene(int sceneIndex)
        {
            PhotonNetwork.LoadLevel(sceneIndex);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master Server!");
        }
    }
}