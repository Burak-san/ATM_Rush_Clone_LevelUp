using System.Collections.Generic;
using Commands;
using UnityEngine;
using Controllers;
using Signals;
using Sirenix.OdinInspector;

namespace Managers
{
    public class StackManager : MonoBehaviour
    {

        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private StackMovementController stackMovementController;
        [SerializeField] private StackCollideWPlatformCommand stackCollideWPlatformCommand;

        #endregion

        #region Private Variables

        [ShowInInspector] private List<GameObject> _stackMembers = new List<GameObject>();
        [ShowInInspector] private GameObject _collectables;
        [ShowInInspector] private GameObject _player;

        private AddToStackCommand _addToStackCommand;
        private StackAnimationCommand _stackAnimationCommand;
        private StackCollideAtmCommand _stackCollideAtmCommand;
        private StackCollideObstacleCommand _stackCollideObstacleCommand;
        private StackCollideWPlatformCommand _stackCollideWPlatformCommand;

        #endregion

        #endregion

        private void Awake()
        {
            _collectables = GameObject.Find("Collectables");
            Init();
        }

        #region Subscribe Events

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CollectableSignals.Instance.onAddStackList += _addToStackCommand.Execute;
            CollectableSignals.Instance.onCollideObstacle += _stackCollideObstacleCommand.Execute;
            CollectableSignals.Instance.onCollideATM += _stackCollideAtmCommand.Execute;
            CollectableSignals.Instance.onCollideWalkingPlatform += _stackCollideWPlatformCommand.Execute;
        }

        private void UnsubscribeEvents()
        {
            CollectableSignals.Instance.onAddStackList -= _addToStackCommand.Execute;
            CollectableSignals.Instance.onCollideObstacle -= _stackCollideObstacleCommand.Execute;
            CollectableSignals.Instance.onCollideATM -= _stackCollideAtmCommand.Execute;
            CollectableSignals.Instance.onCollideWalkingPlatform -= _stackCollideWPlatformCommand.Execute;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion
        
        private void Update()
        {
            stackMovementController.Lerp(_stackMembers);
        }

        private void Init()
        {
            _stackAnimationCommand = new StackAnimationCommand(ref _stackMembers);
            _addToStackCommand = new AddToStackCommand(ref _stackMembers,ref _stackAnimationCommand,this.transform,this);
            _stackCollideAtmCommand = new StackCollideAtmCommand(ref _stackMembers);
            _stackCollideObstacleCommand = new StackCollideObstacleCommand(ref _stackMembers, ref _collectables);
            _stackCollideWPlatformCommand = new StackCollideWPlatformCommand(ref _stackMembers, ref _collectables);
        }
    }
}