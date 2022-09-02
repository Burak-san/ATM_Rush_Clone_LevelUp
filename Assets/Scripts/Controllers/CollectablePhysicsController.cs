using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class CollectablePhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CollectableManager collectableManager;
        
        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Portal"))
            {
                CollectableSignals.Instance.onCollideGate?.Invoke(collectableManager.gameObject);
            }

            if (other.CompareTag("Player") && !CompareTag("Collected"))
            {
               CollectableSignals.Instance.onAddStackList?.Invoke(collectableManager.gameObject);
               SetPlayerScoreSignal(collectableManager.gameObject,1);
               tag = "Collected";
            }

            if (other.CompareTag("Obstacle"))
            {
                CollectableSignals.Instance.onCollideObstacle?.Invoke(collectableManager.gameObject,other.transform.position);
                SetPlayerScoreSignal(collectableManager.gameObject,-1);
            }

            if ( CompareTag("Collected"))
            {
                if (other.CompareTag("ATM"))
                {
                    SetAtmScoreSignal(collectableManager.gameObject);
                    CollectableSignals.Instance.onCollideATM?.Invoke(collectableManager.gameObject);
                }

                if (other.CompareTag("WalkingPlatform"))
                {
                    SetAtmScoreSignal(collectableManager.gameObject);
                    CollectableSignals.Instance.onCollideWalkingPlatform?.Invoke(collectableManager.gameObject);
                    tag = "Uncollected";
                }
            }
            if (other.CompareTag("Collected") && CompareTag("Uncollected"))
            {
                CollectableSignals.Instance.onAddStackList?.Invoke(collectableManager.gameObject);
                tag = "Collected";
                SetPlayerScoreSignal(collectableManager.gameObject,1);
            }
        }

        private void SetPlayerScoreSignal(GameObject gO,int changeFactor)
        {
            var state = gO.GetComponent<CollectableManager>().Data;
            if (state == CollectableTypes.Money)
            {
                ScoreSignals.Instance.onIncreasePlayerScore?.Invoke(1*changeFactor);
            }
            else if(state == CollectableTypes.Gold)
            {
                ScoreSignals.Instance.onIncreasePlayerScore?.Invoke((2*changeFactor));
            }
            else
            {
                ScoreSignals.Instance.onIncreasePlayerScore?.Invoke((3*changeFactor));
            }
        }
        
        private void SetAtmScoreSignal(GameObject gO)
        {
            var state = gO.GetComponent<CollectableManager>().Data;
            if (state == CollectableTypes.Money)
            {
                ScoreSignals.Instance.onIncreaseATMScore?.Invoke(1);
            }
            else if(state == CollectableTypes.Gold)
            {
                ScoreSignals.Instance.onIncreaseATMScore?.Invoke(2);
            }
            else
            {
                ScoreSignals.Instance.onIncreaseATMScore?.Invoke(3);
            }
        }
    }
}