using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private static GameManager gameManager;
    private GameTime gameTime;
    private LevelManager levelManager;
    private ObjectManager objectManager;
    private IInteractionController interactionController;

    public static GameManager Instance { get => gameManager; }
    public GameTime Time { get => gameTime; set => gameTime = value; }
    public LevelManager LevelManager { get => levelManager; set => levelManager = value; }
    public ObjectManager ObjectManager { get => objectManager; set => objectManager = value; }
    public IInteractionController InteractionController { get => interactionController; set => interactionController = value; }

    private void Awake()
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

    [ContextMenu("Start Level")]
    private void StartLevel()
    {
        Instance.LevelManager.SetLevelActive(0);
    }

    [ContextMenu("Announce Times")]
    private void AnnounceTimes()
    {
        Debug.Log("Game elapsed time: " + gameTime.ElapsedTime);
    }
}
