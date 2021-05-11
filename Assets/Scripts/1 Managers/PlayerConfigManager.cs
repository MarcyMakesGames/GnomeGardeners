using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigManager : MonoBehaviour
{
    public bool canJoinPlayers;

    private PlayerInputManager inputManager;
    private List<PlayerConfig> playerConfigs;
    private int maxPlayers = 4;
    private int playerCount;

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
            if(!GameManager.Instance.DebugMenu)
            {
                var sceneToLoad = GameManager.Instance.SceneToLoad;
                if ( sceneToLoad == string.Empty)
                {
                    canJoinPlayers = false;
                    GameManager.Instance.SceneController.LoadNextScene();
                    GameManager.Instance.playersReady = true;
                }
                else
                {
                    canJoinPlayers = false;
                    GameManager.Instance.SceneController.LoadSceneByString(sceneToLoad);
                    GameManager.Instance.playersReady = true;
                }
            }
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

    private void Update()
    {
        ListenJoinPlayer();
    }

    private void ListenJoinPlayer()
    {
        if (canJoinPlayers && Keyboard.current.spaceKey.wasPressedThisFrame && playerCount <= maxPlayers - 1)
        {
            switch(playerCount)
            {
                case 0:
                    inputManager.JoinPlayer(playerCount, playerCount, "KeyboardLeft", Keyboard.current);
                    playerCount++;
                    break;
                case 1:
                    inputManager.JoinPlayer(playerCount, playerCount, "KeyboardRight", Keyboard.current);
                    playerCount++;
                    break;
                case 2:
                    inputManager.JoinPlayer(playerCount, playerCount, "Gamepad");
                    playerCount++;
                    break;
                case 3:
                    inputManager.JoinPlayer(playerCount, playerCount, "Gamepad");
                    playerCount++;
                    break;
            }
        }
    }

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();

        if (GameManager.Instance.PlayerConfigManager == null)
        {
            GameManager.Instance.PlayerConfigManager = this;
            playerConfigs = new List<PlayerConfig>();
        }
    }
}

