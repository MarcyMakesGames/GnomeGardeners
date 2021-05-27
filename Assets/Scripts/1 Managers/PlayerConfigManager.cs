using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GnomeGardeners
{
    public class PlayerConfigManager : MonoBehaviour
    {
        public bool canJoinPlayers;

        private PlayerInputManager inputManager;
        private List<PlayerConfig> playerConfigs;
        private List<GameObject> playerConfigObjects;
        private int maxPlayers = 4;
        private int playerCount;

        public List<PlayerConfig> PlayerConfigs { get => playerConfigs; }

        public int PlayerCount => playerCount;

        //public void SetGnomeSkin(int index, ISkin gnomeSkin)
        //{
        //This is where we'd actually create a new playerConfig with the skin using the index.
        //}

        private void Awake()
        {
            inputManager = GetComponent<PlayerInputManager>();

            if (GameManager.Instance.PlayerConfigManager == null)
            {
                GameManager.Instance.PlayerConfigManager = this;
                playerConfigs = new List<PlayerConfig>();
                playerConfigObjects = new List<GameObject>();
            }
        }

        private void Update()
        {
            ListenJoinPlayer();
            ListenMainMenu();
        }

        public void ReadyPlayer(int index, GnomeSkinObject gnomeSkin, Sprite playerIcon)
        {
            playerConfigs[index].IsReady = true;
            playerConfigs[index].GnomeSkin = gnomeSkin;
            playerConfigs[index].PlayerIcon = playerIcon;

            StartGameCheck();
        }

        public void StartGameCheck()
        {
            if (playerConfigs.Count >= 1 && playerConfigs.All(x => x.IsReady))
            {
                if (!GameManager.Instance.DebugMenu)
                {
                    canJoinPlayers = false;
                    GameManager.Instance.SceneController.LoadSceneGameplay();
                    GameManager.Instance.playersReady = true;
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

                playerConfigObjects.Add(playerInput.gameObject);
            }
        }

        public void HandlePlayerLeft(PlayerInput playerInput)
        {
            if(playerInput.playerIndex == 1)
            {
                playerInput.user.UnpairDevicesAndRemoveUser();
                playerCount--;

                List<PlayerConfig> objToDestroy = new List<PlayerConfig>();

                foreach (PlayerConfig config in playerConfigs)
                    if (config.Input == playerInput)
                        objToDestroy.Add(config);

                foreach (PlayerConfig config in objToDestroy)
                    playerConfigs.Remove(config);
            }
        }


        private void ListenJoinPlayer()
        {
            if (canJoinPlayers && Keyboard.current.spaceKey.wasPressedThisFrame && playerCount <= maxPlayers - 1)
            {
                switch (playerCount)
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
                        if (Gamepad.all.Count < 1)
                            return;
                        inputManager.JoinPlayer(playerCount, playerCount, "Gamepad", Gamepad.all[1]);
                        playerCount++;
                        break;
                    case 3:
                        if (Gamepad.all.Count < 2)
                            return;
                        inputManager.JoinPlayer(playerCount, playerCount, "Gamepad", Gamepad.all[2]);
                        playerCount++;
                        break;
                }
            }
        }

        private void ListenMainMenu()
        {
            if (canJoinPlayers && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                List<GameObject> objectsToDestroy = new List<GameObject>();

                foreach (GameObject playerConfigObject in playerConfigObjects)
                    if(playerConfigObject != playerConfigObjects[0])
                    {
                        HandlePlayerLeft(playerConfigObject.GetComponent<PlayerInput>());
                        objectsToDestroy.Add(playerConfigObject);
                    }

                foreach (GameObject obj in objectsToDestroy)
                {
                    playerConfigObjects.Remove(obj);
                    Destroy(obj);
                }

                playerConfigs[0].IsReady = false;

                canJoinPlayers = false;
                FindObjectOfType<MainMenuController>().SetPanelActive(1);
            }
        }
    }
}
