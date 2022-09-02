using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class StackCollideAtmCommand
    {
        #region Private Variables

        private List<GameObject> _stackList;

        #endregion
        public StackCollideAtmCommand(ref List<GameObject> stackList)
        {
            _stackList = stackList;
        }
        public void Execute(GameObject gO)
        {
            _stackList.Remove(gO);
            _stackList.TrimExcess();
            Object.Destroy(gO);
            for (int i = 1; i <= _stackList.Count - 1; i++)
            {
                _stackList[i].transform.localPosition =
                    _stackList[i - 1].transform.localPosition + Vector3.forward;
            }
        }
    }
}