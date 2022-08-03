using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class StackMovementController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables
        
        [SerializeField] private StackManager manager;

        #endregion

        #region Private Variables

        private List<GameObject> _newList = new List<GameObject>();


        #endregion

        #endregion

        public void Lerp(List<GameObject> stackList)
        {
            _newList = stackList;
            StartCoroutine(LerpMoney());
        }


        IEnumerator LerpMoney()//shaderin getvaluesine erisip onu degistircez
        {
            for (int i = 0; i <= _newList.Count -1 ; i++)
            {
                _newList[i].transform
                    .DOMoveX(i == 0 ? transform.position.x : _newList[i - 1].transform.position.x, .1f);
                yield return new WaitForSeconds(.1f);
            }
            
        }
    }
}