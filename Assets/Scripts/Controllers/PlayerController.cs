using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Managers;

namespace Controllers
{
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        [HideInInspector] public int id;

        [Header("PlayerInfo")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private GameObject _hatObject;

        public float _currentHatTime;

        [Header("Components")]
        private Rigidbody _rigidbody;
        private Player _photonPlayer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (_currentHatTime >= GameManager.Instance.timeToWin && !GameManager.Instance.gameEnd)
                {
                    GameManager.Instance.gameEnd = true;
                    GameManager.Instance.photonView.RPC("WinGame", RpcTarget.All, id);
                }
            }

            if (!photonView.IsMine)
                return;

            Move();

            if (Input.GetKeyDown(KeyCode.Space))
                TryJump();

            if (_hatObject.activeInHierarchy)
                _currentHatTime += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine)
                return;

            if (collision.gameObject.CompareTag("Player"))
            {
                if (GameManager.Instance.GetPlayer(collision.gameObject).id == GameManager.Instance.playerIdWithHat)
                {
                    if (GameManager.Instance.CanGetHat())
                    {
                        GameManager.Instance.photonView.RPC("GiveHat", RpcTarget.All, id, false);
                    }
                }
            }
        }

        private void Move()
        {
            float x = Input.GetAxis("Horizontal") * _moveSpeed;
            float z = Input.GetAxis("Vertical") * _moveSpeed;

            _rigidbody.velocity = new Vector3(x, _rigidbody.velocity.y, z);
        }

        private void TryJump()
        {
            Ray ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, 0.7f))
            {
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
        }

        public void SetHat(bool hasHat)
        {
            _hatObject.SetActive(hasHat);
        }

        [PunRPC]
        public void Initialize(Player player)
        {
            _photonPlayer = player;
            id = player.ActorNumber;
            GameManager.Instance.players[id - 1] = this;

            if (id == 1)
            {
                GameManager.Instance.GiveHat(id, true);
            }


            if (!photonView.IsMine)
            {
                _rigidbody.isKinematic = true;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_currentHatTime);
            }
            else if (stream.IsReading)
            {
                _currentHatTime = (float)stream.ReceiveNext();
            }
        }
    }
}