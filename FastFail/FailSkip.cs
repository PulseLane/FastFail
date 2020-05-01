using IPA.Utilities;
using System.Linq;
using UnityEngine;

namespace FastFail
{
    class FailSkip : MonoBehaviour
    {
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

        protected bool _standardLevel;
        protected bool _hasFailed = false;

        public void Awake()
        {
            Logger.Log("Awakening");
            _standardLevelGameplayManager = Resources.FindObjectsOfTypeAll<StandardLevelGameplayManager>().FirstOrDefault();
            if (_standardLevelGameplayManager)
            {
                _standardLevelGameplayManager.levelFailedEvent += this.OnLevelFail;
                _standardLevel = true;
                Logger.Log("Standard");
            }
            else
            {
                Logger.Log("Mission");
                _missionLevelGameplayManager = Resources.FindObjectsOfTypeAll<MissionLevelGameplayManager>().FirstOrDefault();
                _missionLevelGameplayManager.levelFailedEvent += this.OnLevelFail;
                _standardLevel = false;
            }

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
            }
        }

        
    }
}
