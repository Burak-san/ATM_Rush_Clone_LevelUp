using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class CollectableMovementController : MonoBehaviour
    {
        public void MoveOnWalkingPlatform(GameObject gO,GameObject collectables)
        {
            gO.transform.SetParent(null);
            var goPos = gO.transform.position;
            gO.transform.DOJump(new Vector3(goPos.x, goPos.y + .5f, goPos.z + 2f), 0f, 0,.1f);
            gO.transform.DOMoveX(-10f, 1.1f,false).SetEase(Ease.Linear).SetDelay(.05f);
            gO.transform.SetParent(collectables.transform);
        }
    }
}