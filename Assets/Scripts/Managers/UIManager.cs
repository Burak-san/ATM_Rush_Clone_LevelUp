using System;
using Controllers;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private UIPanelController uiPanelController;
        [SerializeField] private LevelPanelController levelPanelController;
        [SerializeField] private SteadyPanelController steadyPanelController;

        #endregion

        #endregion
        

        #region Event Subscriptions

        private void Awake()
        {
            LoadDefaultMoney();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onChangeLevelText += OnChangeLevelText;
            UISignals.Instance.onChangeMoneyText += OnChangeMoneyText;
            UISignals.Instance.onOpenPanel += OnOpenPanel;
            UISignals.Instance.onClosePanel += OnClosePanel;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
            
        }

        private void UnsubscribeEvents()
        {
            UISignals.Instance.onChangeLevelText -= OnChangeLevelText;
            UISignals.Instance.onChangeMoneyText -= OnChangeMoneyText;
            UISignals.Instance.onOpenPanel -= OnOpenPanel;
            UISignals.Instance.onClosePanel -= OnClosePanel;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
        }
        

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnOpenPanel(UIPanels panelParam)
        {
            uiPanelController.OpenPanel(panelParam);
        }

        private void OnClosePanel(UIPanels panelParam)
        {
            uiPanelController.ClosePanel(panelParam);
        }
        

        private void OnPlay()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.StartPanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.LevelPanel);
        }
        
        public void Play()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
            CoreGameSignals.Instance.onSetCameraTarget.Invoke();
            CoreGameSignals.Instance.onSetCameraState?.Invoke(CameraTypes.InitializeCam);
            OnChangeLevelText();
        }
        
        private void OnRestartLevel()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.LevelPanel);
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.WinPanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.StartPanel);
        }

        public void Restart()
        {
            CoreGameSignals.Instance.onRestartLevel?.Invoke();
        }
        
        private void OnNextLevel()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.WinPanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.StartPanel);
            UISignals.Instance.onChangeLevelText?.Invoke();
        }

        public void Next()
        {
            CoreGameSignals.Instance.onNextLevel?.Invoke();
        }

        private void OnLevelSuccessful()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.LevelPanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.WinPanel);
        }

        private void OnChangeLevelText()
        {
            var levelID = CoreGameSignals.Instance.onGetLevelID.Invoke();
            levelPanelController.SetLevelText(levelID+1);
        }

        private void OnChangeMoneyText(int lastScore)
        {
            steadyPanelController.SetMoneyText(lastScore);
        }

        private void LoadDefaultMoney()
        {
            var defaultMoney = !ES3.KeyExists("Money") ? 0 : ES3.Load<int>("Money");
            steadyPanelController.SetMoneyText(defaultMoney);
        }
    }
}