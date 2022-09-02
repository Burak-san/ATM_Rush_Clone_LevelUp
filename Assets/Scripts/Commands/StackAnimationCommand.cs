using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Commands
{
    public class StackAnimationCommand
    {
        #region Private Variables

        private List<GameObject> _stackList;

        #endregion
        public StackAnimationCommand(ref List<GameObject> stackList)
        {
            _stackList = stackList;
        }

        public IEnumerator Execute() 
        {
            var increasedScale = Vector3.one*1.7f ;
            for (int i = 0 ; i <= _stackList.Count-1 ; i++)
            {
                var index = (_stackList.Count -1) - i;

                _stackList[index].transform.DOScale(increasedScale, 0.1f).SetEase(Ease.Flash); 
                _stackList[index].transform.DOScale(Vector3.one*1.2f, .1f ).SetEase(Ease.Flash).SetDelay(.1f); 
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}