using Managers;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class SteadyPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private UIManager uiManager;

        #endregion

        #endregion
        public void SetMoneyText(int money)
        {
            moneyText.text = money.ToString();
        }
    }
}