using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Self Variables

        #endregion

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents(); 
        }
                    
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onSaveGame += OnSaveGame;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onSaveGame -= OnSaveGame;
        }

        #endregion
        private void  OnSaveGame(SaveGameDataParams dataParams)
        {
            ES3.Save("Level",dataParams.Level);

            ES3.Save("Money" ,dataParams.Money);
        }
    }
}