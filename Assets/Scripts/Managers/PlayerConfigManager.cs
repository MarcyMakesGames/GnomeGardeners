using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigManager : MonoBehaviour
{
    [SerializeField] private GameObject gnomePrefab;
    private List<PlayerConfig> playerConfigs;
    private int maxPlayers = 4;

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
        if(playerConfigs.Count >= 2 && playerConfigs.All(x => x.IsReady))
        {
            Debug.Log("All players read, loading next scene.");
            //Load next scene
        }
    }

    public void HandlePlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player" + playerInput.playerIndex + " joined.");

        PlayerConfig newConfig = new PlayerConfig(playerInput);
        playerInput.transform.SetParent(transform);
        playerConfigs.Add(newConfig);

        GameObject newGnome = Instantiate(gnomePrefab, transform);
        newGnome.GetComponent<GnomeController>().InitializePlayer(newConfig);

        if (playerConfigs.Any(x => x.PlayerIndex == playerInput.playerIndex))
        {
            //Do we need a safety check to verify index here?
        }
    }

    private void Start()
    {
        playerConfigs = new List<PlayerConfig>();
    }
}

public class PlayerConfig
{
    private PlayerInput input;
    private int playerIndex;
    private bool isReady;

    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }

    //We need to add some kind of ISkin or object to represent the gnome appearance here.

    public PlayerConfig(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
        isReady = false;
    }
}
