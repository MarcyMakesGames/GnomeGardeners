using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using Random = System.Random;

namespace GnomeGardeners
{
    public class LevelManager : MonoBehaviour
    {
        [HideInInspector] public int lastTotalScore;
        [HideInInspector] public int lastRequiredScore;

        public GameObject levelTutorial;
        public List<GameObject> levels;

        public bool isLastLevelCompleted;

        private LevelController currentLevel;
        private int levelIndex;
        
        private VoidEventChannelSO OnLevelLoseEvent;
        private VoidEventChannelSO OnLevelWinEvent;

        #region Unity Methods
        private void Awake()
        {
            if (GameManager.Instance.LevelManager == null)
                GameManager.Instance.LevelManager = this;

            levelIndex = -1;
            OnLevelLoseEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelLoseEC");
            OnLevelWinEvent = Resources.Load<VoidEventChannelSO>("Channels/LevelWinEC");
            OnLevelLoseEvent.OnEventRaised += UpdateIsLastLevelCompleted;
            OnLevelWinEvent.OnEventRaised += UpdateIsLastLevelCompleted;
        }

        private void OnDestroy()
        {
            GameManager.Instance.PoolController.SetPoolObjectsInactive();

            OnLevelLoseEvent.OnEventRaised -= UpdateIsLastLevelCompleted;
            OnLevelWinEvent.OnEventRaised -= UpdateIsLastLevelCompleted;
        }

        #endregion
        
        #region Public Methods

        public IEnumerator LoadTutorial()
        {
            yield return LoadLevel(levelTutorial);
        }

        public IEnumerator NextLevel()
        {
            levelIndex++;

            yield return StartCoroutine(LoadLevel(levelIndex));
        }

        public IEnumerator RestartLevel()
        {
            GameObject currentLevelPrefab;
            if (levelIndex < 0)
                currentLevelPrefab = levelTutorial;
            else
                currentLevelPrefab = levels[levelIndex];
            yield return StartCoroutine(LoadLevel(currentLevelPrefab));
        }

        public GameObject GetTutorialMenu()
        {
            return currentLevel.tutorialMenu;
        }

        public bool HasCurrentLevelBeenCompleted()
        {
            return currentLevel.HasBeenCompleted;
        }
        
        #endregion
        
        #region Private Methods

        private IEnumerator LoadLevel(GameObject level)
        {
            if(currentLevel)
                Destroy(currentLevel.gameObject);

            yield return new WaitForSeconds(1f);
            
            var newLevel = Instantiate(level);

            yield return new WaitForSeconds(1f);
            
            currentLevel = newLevel.GetComponent<LevelController>();
            currentLevel.LevelStart();
            StartCoroutine(currentLevel.UpdateLevel());
        }

        private IEnumerator LoadLevel(int index)
        {
            if(currentLevel)
                Destroy(currentLevel.gameObject);

            yield return new WaitForSeconds(1f);
            
            var newLevel = Instantiate(levels[index]);
            
            yield return new WaitForSeconds(1f);

            currentLevel = newLevel.GetComponent<LevelController>();
            currentLevel.LevelStart();
            StartCoroutine(currentLevel.UpdateLevel());
        }

        private void UpdateIsLastLevelCompleted()
        {
            if (levelIndex == levels.Count - 1)
                isLastLevelCompleted = true;
        }
        
        #endregion
    }
}
