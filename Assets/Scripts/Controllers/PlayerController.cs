using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public int id;

        [Header("PlayerInfo")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private GameObject _hatObject;

        [HideInInspector] public float _currentHatTime;

        [Header("Components")]
        private Rigidbody _rigidbody;
        private Player _photonPlayer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space))
                TryJump();
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

    }
}