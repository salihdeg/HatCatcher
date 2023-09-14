using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

namespace Managers
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        [Header("Login")]

        [Header("Screens")]
        [SerializeField] private GameObject _loginScreen;
        [SerializeField] private GameObject _lobbyScreen;

        [Header("Login Screen")]
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _joinRoomButton;
        
        [Header("Lobby Screen")]
        [SerializeField] private TextMeshProUGUI _playerListText;
        [SerializeField] private Button _startGameButton;


        private void Start()
        {
            ChangeLoginScreenButtonsStatus(false);
        }

        private void ChangeLoginScreenButtonsStatus(bool status)
        {
            _createRoomButton.interactable = status;
            _joinRoomButton.interactable = status;
        }

        public override void OnConnectedToMaster()
        {
            ChangeLoginScreenButtonsStatus(true);
        }

        public void SetScreen(GameObject screen)
        {
            _loginScreen.SetActive(false);
            _lobbyScreen.SetActive(false);

            screen.SetActive(true);
        }

        public void OnCreateRoomButton(TMP_InputField roomNameInput)
        {
            NetworkManager.Instance.CreateRoom(roomNameInput.text);
        }

        public void OnJoinRoomButton(TMP_InputField roomNameInput)
        {
            NetworkManager.Instance.JoinRoom(roomNameInput.text);
        }
    }
}

