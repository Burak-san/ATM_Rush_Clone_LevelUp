using System.Collections.Generic;
using Signals;
using UnityEngine;

namespace Commands
{
    public class StackCollideWPlatformCommand
    {
        private GameObject _collectables;
        private List<GameObject> _stackList;
        public StackCollideWPlatformCommand(ref List<GameObject> stacList,ref GameObject collectables)
        {
            _stackList = stacList;
            _collectables = collectables;
        }
        public void Execute(GameObject gO)
        {
            if (_stackList.Count==0)
            {
                ScoreSignals.Instance.onSetLevelScore?.Invoke();
            }
            _stackList.Remove(gO);
            _stackList.TrimExcess();
            gO.transform.SetParent(_collectables.transform);
        }
    }
}