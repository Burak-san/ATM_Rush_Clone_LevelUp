using System;
using Data.ValueObject;
using Keys;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private new Rigidbody rigidbody;

        #endregion

        #region Private Variables

        [Header("Data")] [ShowInInspector] private PlayerMovementData _movementData;
        [ShowInInspector] private bool _isReadyToMove,_isReadyToPlay;
        [ShowInInspector] private float _inputValue;
        [ShowInInspector] private Vector2 _clampValues;

        #endregion

        #endregion

        public void SetMovementData(PlayerMovementData dataMovement)
        {
            _movementData = dataMovement;
        }

        public void UpdateInputValue(HorizontalInputParams inputParams)
        {
            _inputValue = inputParams.XValues;
            _clampValues = inputParams.ClampValues;
        }

        public void EnableMovement()
        {
            _isReadyToMove = true;
        }

        public void DisableMovement()
        {
            _isReadyToMove = false;
        }

        public void IsReadyToPlay(bool state)
        {
            _isReadyToPlay = state;
        }

        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                if (_isReadyToMove)
                {
                    Move();
                }
                else
                {
                    StopSides();
                }
            }
            else
            {
                Stop();
            }
        }

        private void Move()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValue * _movementData.SidewaysSpeed, velocity.y, _movementData.ForwardSpeed);
            rigidbody.velocity = velocity;

            Vector3 position;
            position = new Vector3(Mathf.Clamp(rigidbody.position.x, _clampValues.x, _clampValues.y),
                (position = rigidbody.position).y, position.z);
            rigidbody.position = position;
        }

        private void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        private void StopSides()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _movementData.ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void OnReset()
        {
            Stop();
            _isReadyToMove = false;
            _isReadyToPlay = false;
        }
    }
}