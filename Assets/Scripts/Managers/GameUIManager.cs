using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace Managers
{
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager Instance { get; private set; }

        public PlayerUIContainer[] playerUIContainers;
        public TextMeshProUGUI winText;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializePlayerUI();
        }

        private void Update()
        {
            UpdatePlayerUI();
        }

        private void UpdatePlayerUI()
        {
            for (int i = 0; i < GameManager.Instance.players.Length; i++)
            {
                if (GameManager.Instance.players[i] != null)
                {
                    playerUIContainers[i].hatTimeSlider.value = GameManager.Instance.players[i].currentHatTime;
                }
            }
        }

        private void InitializePlayerUI()
        {
            for (int i = 0; i < playerUIContainers.Length; i++)
            {
                PlayerUIContainer container = playerUIContainers[i];

                if (i < PhotonNetwork.PlayerList.Length)
                {
                    container.containerObject.SetActive(true);
                    container.playerNameText.text = PhotonNetwork.PlayerList[i].NickName;
                    container.hatTimeSlider.maxValue = GameManager.Instance.timeToWin;
                }
                else
                {
                    container.containerObject.SetActive(false);
                }
            }
        }

        public void SetWinText(string winnerName)
        {
            winText.gameObject.SetActive(true);
            winText.text = winnerName;
        }

    }

    [System.Serializable]
    public class PlayerUIContainer
    {
        public GameObject containerObject;
        public TextMeshProUGUI playerNameText;
        public Slider hatTimeSlider;
    }
}