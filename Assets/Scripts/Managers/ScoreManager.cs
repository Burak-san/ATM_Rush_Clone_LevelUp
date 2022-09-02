using System;
using System.Collections.Generic;
using Controllers;
using Keys;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private List<TextMeshPro> textList = new List<TextMeshPro>();

        #endregion

        #region Private Variables

        private int _atmScore;
        private int _score;
        private float _factor;
        private int _totalScore;

        #endregion

        #endregion

        #region Subscription Events

        private void Awake()
        {
            _totalScore = GetCurrentMoney();
        }

        private void OnEnable()
        { 
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ScoreSignals.Instance.onIncreaseATMScore += OnIncreaseATMScore;
            ScoreSignals.Instance.onSetTotalLevelScore += OnSetTotalLevelScore;
            MiniGameSignals.Instance.onSetMoneyFactor += OnSetMoneyFactor;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
        }

        private void UnsubscribeEvents()
        {
            ScoreSignals.Instance.onIncreaseATMScore -= OnIncreaseATMScore;
            ScoreSignals.Instance.onSetTotalLevelScore -= OnSetTotalLevelScore;
            MiniGameSignals.Instance.onSetMoneyFactor -= OnSetMoneyFactor;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private int GetCurrentMoney()
        {
            if (!ES3.FileExists()) return 0;
            return ES3.KeyExists("Money") ? ES3.Load<int>("Money") : 0;
        }

        private void OnIncreaseATMScore(int score)
        {
            _atmScore += score;
            foreach (var textMeshPro in textList)
            {
                textMeshPro.text = _atmScore.ToString();
            }
        }
        

        private void OnSetMoneyFactor(float factor)
        {
            _factor = factor;
        }

        private void OnSetTotalLevelScore(int score)
        {
            _score = score;
        }
        

        private void OnNextLevel()
        {
            CoreGameSignals.Instance.onSaveGame?.Invoke(new SaveGameDataParams()
            {
                Money = SetLastScore()
            });
            UISignals.Instance.onChangeMoneyText?.Invoke(SetLastScore());
        }

        private int SetLastScore()
        {
            
            return (int)(_score * _factor) + _totalScore;
        }
    }
}