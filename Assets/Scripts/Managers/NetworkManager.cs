using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _loadingCanvas;
        private TaskCompletionSource<bool> _connectTask;

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
            if (_loadingCanvas != null)
                _loadingCanvas.SetActive(true);
            Connect();
        }

        private async void Connect()
        {
            _connectTask = new TaskCompletionSource<bool>();

            PhotonNetwork.ConnectUsingSettings();

            await _connectTask.Task;

            if (_loadingCanvas != null)
                _loadingCanvas.SetActive(false);
            _connectTask = null;
        }

        public void CreateRoom(string roomName)
        {
            PhotonNetwork.CreateRoom(roomName);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        [PunRPC]
        public void ChangeScene(int sceneIndex)
        {
            PhotonNetwork.LoadLevel(sceneIndex);
        }

        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master Server!");
            if (_connectTask != null) _connectTask.SetResult(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected!");
            if (_connectTask != null) _connectTask.SetResult(false);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Room Created!\nRoom Name = " + PhotonNetwork.CurrentRoom.Name);
        }
        #endregion
    }
}