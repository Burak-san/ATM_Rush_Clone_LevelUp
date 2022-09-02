using Cinemachine;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] CinemachineVirtualCamera runnerVirtualCam,miniGameCam;

        #endregion

        #region Private Variables

        [ShowInInspector] private Vector3 _initialPosition;
        private Animator _camAnimator;

        #endregion

        #endregion

        private void Awake()
        {
            _camAnimator = GetComponent<Animator>();
            GetInitialPosition();
            SetCameraTarget();
        }

        #region Event Subscription
        
        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onSetCameraState += OnSetNextCameraState;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onSetCameraTarget += OnSetCameraTarget;
            CoreGameSignals.Instance.onFinishLineReached += OnStartMiniGameCam;
            MiniGameSignals.Instance.onSetCameraTargetFakePlayer += OnSetCameraTargetFakePlayer;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onSetCameraState -= OnSetNextCameraState;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onSetCameraTarget -= OnSetCameraTarget;
            CoreGameSignals.Instance.onFinishLineReached -= OnStartMiniGameCam;
            MiniGameSignals.Instance.onSetCameraTargetFakePlayer -= OnSetCameraTargetFakePlayer;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion
        
        private void GetInitialPosition()
        {
            _initialPosition = transform.localPosition;
        }
        
        private void OnMoveToInitialPosition()
        {
            transform.localPosition = _initialPosition;
        }
        
        
        private void OnSetCameraTarget()
        {       
            var playerManager = FindObjectOfType<PlayerManager>().transform;
            runnerVirtualCam.Follow = playerManager;
        }
        
        private void OnReset()
        {
            runnerVirtualCam.LookAt = null;
            runnerVirtualCam.Follow = null;
            runnerVirtualCam.transform.position = new Vector3(0, 24, 3);
            
            runnerVirtualCam.LookAt = null;
            runnerVirtualCam.Follow = null;
            miniGameCam.transform.position = new Vector3(0, 9.60000038f, 210.100006f);
            OnMoveToInitialPosition();
        }
        
        
        private void OnSetNextCameraState(CameraTypes cameraTypes)
        {
            if (cameraTypes == CameraTypes.InitializeCam)
            {
                _camAnimator.Play("RunnerCam");
                cameraTypes = CameraTypes.RunnerCam;
            }
            else if(cameraTypes == CameraTypes.RunnerCam)
            {
                _camAnimator.Play("MiniGameCam");
                cameraTypes = CameraTypes.MiniGameCam;
            }
            else if(cameraTypes == CameraTypes.MiniGameCam)
            {
                _camAnimator.Play("InitializeCam");
                cameraTypes = CameraTypes.InitializeCam;
            }
        }
        
        private void SetCameraTarget()
        {
            CoreGameSignals.Instance.onSetCameraTarget?.Invoke();
        }

        private void OnStartMiniGameCam()
        {
            OnSetNextCameraState(CameraTypes.RunnerCam);
        }

        private void OnSetCameraTargetFakePlayer(GameObject fakePlayer)
        {
            miniGameCam.Follow =fakePlayer.transform;
        }

        private void OnNextLevel()
        {
            OnSetNextCameraState(CameraTypes.MiniGameCam);
            OnSetCameraTarget();
        }

        private void OnRestartLevel()
        {
            OnSetNextCameraState(CameraTypes.MiniGameCam);
            OnSetCameraTarget();
        }
    }
}