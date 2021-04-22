using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfig> playerConfigs;
    private int maxPlayers = 4;

    public List<PlayerConfig> PlayerConfigs { get => playerConfigs; }

    //public void SetGnomeSkin(int index, ISkin gnomeSkin)
    //{
            //This is where we'd actually create a new playerConfig with the skin using the index.
    //}

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
    }

    public void StartGameCheck()
    {
        if(playerConfigs.Count >= 1 && playerConfigs.All(x => x.IsReady))
        {
            Debug.Log("All players ready, loading next scene.");
            GameManager.Instance.SceneController.LoadNextScene();
        }
    }

    public void HandlePlayerJoined(PlayerInput playerInput)
    {
        if (playerConfigs.Count >= maxPlayers)
            return;

        if (!playerConfigs.Any(x => x.PlayerIndex == playerInput.playerIndex))
        {
            PlayerConfig newConfig = new PlayerConfig(playerInput);
            playerConfigs.Add(newConfig);
            playerInput.transform.SetParent(transform);
        }
    }

    private void Awake()
    {
        GameManager.Instance.PlayerConfigManager = this;
        playerConfigs = new List<PlayerConfig>();
    }
}

