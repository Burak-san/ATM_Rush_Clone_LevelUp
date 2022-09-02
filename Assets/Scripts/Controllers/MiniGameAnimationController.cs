using Data.ValueObject;
using DG.Tweening;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class MiniGameAnimationController : MonoBehaviour
    {
        public void StartAnim(Transform targetTransform,MiniGameData data,GameObject fakePlayer,int levelScore )
        {
            var position = targetTransform.position;
            Vector3 newPos = new Vector3(position.x, position.y , position.z-data.cubeScale.z*1.5f);
            fakePlayer.SetActive(transform);
            
            MiniGameSignals.Instance.onSetMoneyFactor?.Invoke(((float)levelScore/(data.maxMoneyValue)+ 1));
            MiniGameSignals.Instance.onSetCameraTargetFakePlayer?.Invoke(fakePlayer);
            
            var fakePlayerPos = (levelScore / data.maxMoneyValue) * data.cubeScale.y;
            fakePlayer.transform.DOMoveY(newPos.y + fakePlayerPos, 10, false)
                .OnComplete(() => CoreGameSignals.Instance.onLevelSuccessful?.Invoke());
            
            
        }
    }
}