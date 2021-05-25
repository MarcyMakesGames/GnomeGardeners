using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace GnomeGardeners
{

    public class LevelManager : MonoBehaviour
    {
        /* 
         * The level manager handles scores and other level specific data across scenes.
         */

        [HideInInspector] public int lastTotalScore;
        [HideInInspector] public int lastRequiredScore;

        public GameObject tutorial;
        public List<GameObject> all;

        private GameObject current;
        private int levelIndex;

        private VoidEventChannelSO OnSceneGameplayLoadedEC;



        #region Unity Methods
        private void Awake()
        {
            if (GameManager.Instance.LevelManager == null)
            {
                GameManager.Instance.LevelManager = this;
            }

            levelIndex = 0;

            OnSceneGameplayLoadedEC = Resources.Load<VoidEventChannelSO>("Channels/SceneGameplayLoadedEC");

            OnSceneGameplayLoadedEC.OnEventRaised += ScenePostLoadCheck;
        }

        private void OnDestroy()
        {
            OnSceneGameplayLoadedEC.OnEventRaised += ScenePostLoadCheck;
        }


        #endregion
        
        #region Public Methods

        public void NextLevel()
        {
            SetLevel(levelIndex);
            levelIndex++;
        }
        
        #endregion
        
        #region Private Methods

        private void SetLevel(GameObject level)
        {
            Destroy(current);
            var newLevel = Instantiate(level);
            current = newLevel;
        }

        private void SetLevel(int index)
        {
            Destroy(current);
            var newLevel = Instantiate(all[index]);
            current = newLevel;
        }
        
        private void ScenePostLoadCheck()
        {
            var levelStart = FindObjectOfType<LevelController>();
            var startObject = levelStart.gameObject;
            if (startObject)
                current = startObject;
            else
                SetLevel(tutorial);
        }
        
        #endregion
    }
}
