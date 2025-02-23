﻿using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Keys;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public PlayerData Data;

        #endregion

        #region Serialized Variables

        [Space] [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerPhysicsController playerPhysicsController;
        [SerializeField] private TextMeshPro playerScoreTMP;

        #endregion

        #region Private Variables

        private int _playerScore;

        #endregion

        #endregion

        private void Awake()
        {
            Data = GetPlayerData();
            SendPlayerDataToControllers();
        }
        

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>("Data/CD_Player").Data;

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(Data.MovementData);
            playerPhysicsController.SetPhysicsData(Data.PullBackForceData);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputTaken += OnActivateMovement;
            InputSignals.Instance.onInputReleased += OnDeactivateMovement;
            InputSignals.Instance.onInputDragged += OnGetInputValues;
            ScoreSignals.Instance.onIncreasePlayerScore += OnIncreasePlayerScore;
            ScoreSignals.Instance.onDecreasePlayerScore += OnDecreasePlayerScore;
            ScoreSignals.Instance.onSetTotalLevelScore -= OnSetTotalLevelScore;


            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.onFinishLineReached += OnFinishLineReached;
       
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= OnActivateMovement;
            InputSignals.Instance.onInputReleased -= OnDeactivateMovement;
            InputSignals.Instance.onInputDragged -= OnGetInputValues;

            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.onFinishLineReached -= OnFinishLineReached;
            ScoreSignals.Instance.onIncreasePlayerScore -= OnIncreasePlayerScore;
            ScoreSignals.Instance.onDecreasePlayerScore -= OnDecreasePlayerScore;
            ScoreSignals.Instance.onSetTotalLevelScore -= OnSetTotalLevelScore;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Movement Controller

        private void OnActivateMovement()
        {
            movementController.EnableMovement();
            
        }

        private void OnDeactivateMovement()
        {
            movementController.DisableMovement();
        }

        private void OnGetInputValues(HorizontalInputParams inputParams)
        {
            movementController.UpdateInputValue(inputParams);
        }

        #endregion

        private void OnPlay()
        {
            movementController.IsReadyToPlay(state:true);
            animationController.ActivatePlayerAnim(true);
        }

        private void OnReset()
        {
            movementController.OnReset();
        }

        private void OnLevelSuccessful()
        {
            movementController.IsReadyToPlay(false);
        }

        private void OnFinishLineReached()
        {
            movementController.IsReadyToPlay(false);
            animationController.ActivatePlayerAnim(false);
            animationController.MiniGamePlayerAnim(true);
            ScoreSignals.Instance.onSetTotalLevelScore?.Invoke(_playerScore);
            MiniGameSignals.Instance.onSetLevelScoreToMiniGame?.Invoke(_playerScore);
        }

        private void OnIncreasePlayerScore(int score)
        {
            _playerScore += score;
            playerScoreTMP.text = _playerScore.ToString();
        }

        private void OnDecreasePlayerScore(int score)
        {
            _playerScore -= score;
            playerScoreTMP.text = _playerScore.ToString();
        }

        private void OnSetTotalLevelScore(int levelScore)
        {
            _playerScore = levelScore;
        }
    }
}