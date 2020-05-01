using IPA.Utilities;
using System.Linq;
using UnityEngine;

namespace FastFail
{
    class FailSkip : MonoBehaviour
    {
        protected StandardLevelGameplayManager _gameplayManager;
        protected StandardLevelFailedController _standardLevelFailedController;

        protected StandardLevelScenesTransitionSetupDataSO _standardLevelSceneSetupData;
        protected StandardLevelFailedController.InitData _initData;
        protected PrepareLevelCompletionResults _prepareLevelCompletionResults;

        protected bool _hasFailed = false;

        public void Awake()
        {
            Logger.Log("Awakening");
            _gameplayManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().FirstOrDefault();
            _gameplayManager.levelFailedEvent += this.OnLevelFail;

            _standardLevelFailedController = Resources.FindObjectsOfTypeAll<StandardLevelFailedController>().FirstOrDefault();
            _standardLevelSceneSetupData = _standardLevelFailedController.GetField<StandardLevelScenesTransitionSetupDataSO, StandardLevelFailedController>("_standardLevelSceneSetupData");
            _initData = _standardLevelFailedController.GetField<StandardLevelFailedController.InitData, StandardLevelFailedController>("_initData");
            _prepareLevelCompletionResults = _standardLevelFailedController.GetField<PrepareLevelCompletionResults, StandardLevelFailedController>("_prepareLevelCompletionResults");

            if (_gameplayManager)
            {
                Logger.Log("Found gameplay manager!");
            }
            if (_standardLevelFailedController)
            {
                Logger.Log("Found standard level failed controller!");
            }
        }

        public void OnLevelFail()
        {
            _hasFailed = true;
        }

        public void Update()
        {
            if (_hasFailed &&
               (Input.GetButtonDown("MenuButtonLeftHand") | Input.GetButtonDown("MenuButtonRightHand")))
            {
                Logger.Log("pause button pressed during fail!");
                _standardLevelFailedController.StopAllCoroutines();

                LevelCompletionResults.LevelEndAction levelEndAction = _initData.autoRestart ? LevelCompletionResults.LevelEndAction.Restart : LevelCompletionResults.LevelEndAction.None;
                LevelCompletionResults levelCompletionResults = _prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Failed, levelEndAction);
                _standardLevelSceneSetupData.Finish(levelCompletionResults);
            }
        }

        
    }
}
