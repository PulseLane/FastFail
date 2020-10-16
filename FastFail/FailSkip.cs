using FastFail.Settings;
using IPA.Utilities;
using System.Linq;
using UnityEngine;

namespace FastFail
{
    class FailSkip : MonoBehaviour
    {
        // Standard and mission use different failure controllers
        protected StandardLevelGameplayManager _standardLevelGameplayManager;
        protected MissionLevelGameplayManager _missionLevelGameplayManager;
        protected StandardLevelFailedController _standardLevelFailedController;
        protected MissionLevelFailedController _missionLevelFailedController;

        protected StandardLevelScenesTransitionSetupDataSO _standardLevelSceneSetupData;
        protected StandardLevelFailedController.InitData _standardInitData;
        protected MissionLevelScenesTransitionSetupDataSO _missionLevelSceneSetupData;
        protected MissionLevelFailedController.InitData _missionInitData;
        protected MissionObjectiveCheckersManager _missionObjectiveCheckersManager;
        protected PrepareLevelCompletionResults _prepareLevelCompletionResults;

        protected VRControllersInputManager _vrControllersInputManager;

        protected bool _standardLevel;
        protected bool _hasFailed = false;
        protected bool _skipped = false;

        public void Awake()
        {
            Logger.log.Debug("awaken");
            _standardLevelGameplayManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().FirstOrDefault();
            // Use the appropriate level failed event
            if (_standardLevelGameplayManager)
            {
                Logger.log.Debug("Standard");
                _standardLevelGameplayManager.levelFailedEvent += this.OnLevelFail;
                _standardLevel = true;
            }
            else
            {
                Logger.log.Debug("Mission");
                _missionLevelGameplayManager = Resources.FindObjectsOfTypeAll<MissionLevelGameplayManager>().FirstOrDefault();
                _missionLevelGameplayManager.levelFailedEvent += this.OnLevelFail;
                _standardLevel = false;
            }

            // Get all the necessary fields
            _standardLevelFailedController = Resources.FindObjectsOfTypeAll<StandardLevelFailedController>().FirstOrDefault();
            if (_standardLevelFailedController)
            {
                _standardLevelSceneSetupData = _standardLevelFailedController.GetField<StandardLevelScenesTransitionSetupDataSO, StandardLevelFailedController>("_standardLevelSceneSetupData");
                _standardInitData = _standardLevelFailedController.GetField<StandardLevelFailedController.InitData, StandardLevelFailedController>("_initData");
                _prepareLevelCompletionResults = _standardLevelFailedController.GetField<PrepareLevelCompletionResults, StandardLevelFailedController>("_prepareLevelCompletionResults");
            }
            else
            {
                _missionLevelFailedController = Resources.FindObjectsOfTypeAll<MissionLevelFailedController>().FirstOrDefault();
                _missionLevelSceneSetupData = _missionLevelFailedController.GetField<MissionLevelScenesTransitionSetupDataSO, MissionLevelFailedController>("_missionLevelSceneSetupData");
                _missionInitData = _missionLevelFailedController.GetField<MissionLevelFailedController.InitData, MissionLevelFailedController>("_initData");
                _missionObjectiveCheckersManager = _missionLevelFailedController.GetField<MissionObjectiveCheckersManager, MissionLevelFailedController>("_missionObjectiveCheckersManager");
                _prepareLevelCompletionResults = _missionLevelFailedController.GetField<PrepareLevelCompletionResults, MissionLevelFailedController>("_prepareLevelCompletionResults");
            }

            _vrControllersInputManager = Resources.FindObjectsOfTypeAll<PauseMenuManager>().FirstOrDefault()
                                            .GetField<VRControllersInputManager, PauseMenuManager>("_vrControllersInputManager");
        }

        public void OnLevelFail()
        {
            _hasFailed = true;
        }

        public void Update()
        {
            if (_vrControllersInputManager.MenuButtonDown())
                Logger.log.Debug("Menu down");
            if (_hasFailed && (Configuration.Instance.autoSkip || _vrControllersInputManager.MenuButtonDown()) && !_skipped)
            {
                // Stop the base coroutine and call the necessary functions to fail the level as quickly as possible
                if (_standardLevel)
                {
                    _standardLevelFailedController.StopAllCoroutines();
                    LevelCompletionResults.LevelEndAction levelEndAction = _standardInitData.autoRestart ? LevelCompletionResults.LevelEndAction.Restart : LevelCompletionResults.LevelEndAction.None;
                    LevelCompletionResults levelCompletionResults = _prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Failed, levelEndAction);
                    _standardLevelSceneSetupData.Finish(levelCompletionResults);
                }
                else
                {
                    _missionLevelFailedController.StopAllCoroutines();
                    LevelCompletionResults.LevelEndAction levelEndAction = _missionInitData.autoRestart ? LevelCompletionResults.LevelEndAction.Restart : LevelCompletionResults.LevelEndAction.None;
                    LevelCompletionResults levelCompletionResults = _prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Failed, levelEndAction);

                    MissionObjectiveResult[] results = _missionObjectiveCheckersManager.GetResults();
                    MissionCompletionResults missionCompletionReuslts = new MissionCompletionResults(levelCompletionResults, results);

                    _missionLevelSceneSetupData.Finish(missionCompletionReuslts);
                }

                _skipped = true;
            }
        }
    }
}
