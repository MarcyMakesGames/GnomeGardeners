using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    protected static GameManager gameManager;
    protected GameTime gameTime;
    protected Level level;

    public static GameManager Instance { get => gameManager; }
    public GameTime Time { get => gameTime; set => gameTime = value; }
    public Level Level { get => level; set => level = value; }

    protected void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }

    protected void Start()
    {
        StartLevel();
    }

    protected void StartLevel()
    {
        level.IsCurrent = true;
    }

    [ContextMenu("Announce Times")]
    protected void AnnounceTimes()
    {
        Debug.Log("Game elapsed time: " + gameTime.ElapsedTime);
    }
}
