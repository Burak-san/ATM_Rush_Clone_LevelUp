using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Signals;
using UnityEngine;

namespace Commands
{
    public class StackCollideObstacleCommand
    {
        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _collectables;

        #endregion

        public StackCollideObstacleCommand(ref List<GameObject> stackMembers,ref GameObject collectables)
        {
            _stackList = stackMembers;
            _collectables = collectables;
        }
        public void Execute(GameObject gO, Vector3 obsPos)
        {
            var siblingIndex = gO.transform.GetSiblingIndex();
            
            _stackList.Remove(gO);

            for (int i = siblingIndex; i <= _stackList.Count - 1; i++)
            {
                int value = (int)_stackList[i].GetComponent<CollectableManager>().Data;
                ScoreSignals.Instance.onDecreasePlayerScore(value);
                _stackList[i].transform.GetChild(1).tag = "Uncollected";
                var newPos = new Vector3(Random.Range(-5f, 5f), 0.5f, obsPos.z + Random.Range(5f, 20f));
                _stackList[i].transform.GetChild(1).GetComponent<Rigidbody>().isKinematic = true;
                _stackList[i].transform.SetParent(_collectables.transform);
                _stackList[i].transform.DOJump(newPos, 2f, 2, .5f, false);
                _stackList.RemoveAt(i);
                _stackList.TrimExcess();
            }
            
            for (int i = 1; i <= _stackList.Count - 1; i++)
            {
                _stackList[i].transform.localPosition =
                    _stackList[i - 1].transform.localPosition + Vector3.forward;
            }
            
            Object.Destroy(gO);
            _stackList.TrimExcess();
            
        }
    }
}