using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class AddToStackCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stacklist;
        private StackAnimationCommand _stackAnimationCommand;
        private Transform _transform;
        private MonoBehaviour _monoBehaviour;

        #endregion

        #endregion

        public AddToStackCommand(ref List<GameObject> stacklist, ref StackAnimationCommand stackAnimationCommand,Transform transform,MonoBehaviour monoBehaviour)
        {
            _stacklist = stacklist;
            _stackAnimationCommand = stackAnimationCommand;
            _transform = transform;
            _monoBehaviour = monoBehaviour;

        }
        public void Execute(GameObject _gO)
        {
            _stacklist.Add(_gO);
            _gO.transform.SetParent(_transform);
            for (int i = 0; i < _stacklist.Count; i++)
            {
                if (i == 0)
                {
                    _stacklist[0].transform.localPosition = new Vector3(0,.8f,0);
                }
                else
                {
                    _stacklist[i].transform.localPosition = 
                        _stacklist[i - 1].transform.localPosition + Vector3.forward;
                }
            }
            _monoBehaviour.StartCoroutine(_stackAnimationCommand.Execute());
            
        }
    }
}