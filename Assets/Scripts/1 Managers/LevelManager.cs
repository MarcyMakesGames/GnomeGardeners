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

        private GameObject current;
        private int levelIndex;

        #region Unity Methods
        private void Awake()
        {
            if (GameManager.Instance.LevelManager == null)
                GameManager.Instance.LevelManager = this;

            levelIndex = -1;
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
            if (levelIndex >= levels.Count)
            {
                isLastLevelCompleted = true;
                yield break;
            }
            yield return StartCoroutine(LoadLevel(levelIndex));
        }

        public IEnumerator RestartLevel()
        {
            yield return StartCoroutine(LoadLevel(current));
        }
        
        #endregion
        
        #region Private Methods

        private IEnumerator LoadLevel(GameObject level)
        {
            if(current)
                Destroy(current);

            yield return new WaitForSeconds(1f);
            
            var newLevel = Instantiate(level);
            current = newLevel;
            var currentLevel = current.GetComponent<LevelController>();
            currentLevel.LevelStart();
            StartCoroutine(currentLevel.UpdateLevel());
        }

        private IEnumerator LoadLevel(int index)
        {
            if(current)
                Destroy(current);

            yield return new WaitForSeconds(1f);
            
            var newLevel = Instantiate(levels[index]);
            current = newLevel;
            var currentLevel = current.GetComponent<LevelController>();
            currentLevel.LevelStart();
            StartCoroutine(currentLevel.UpdateLevel());
        }
        
        #endregion
    }
}
