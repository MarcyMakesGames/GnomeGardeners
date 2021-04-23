using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeInitController : MonoBehaviour
{
    [SerializeField]
    private GameObject gnomePrefab;
    [SerializeField]
    private List<Transform> playerSpawnLocations;

    private GnomeSkin gnomeSkin;
    private bool hasSpawned = false;

    private void InitPlayerGnomes()
    {
        Debug.Log(GameManager.Instance.PlayerConfigManager.PlayerConfigs.Count);
        foreach(PlayerConfig player in GameManager.Instance.PlayerConfigManager.PlayerConfigs)
        {
            Debug.Log("Spawning a gnome");
            GameObject newGnome = Instantiate(gnomePrefab, playerSpawnLocations[player.PlayerIndex].position, Quaternion.identity, transform);
            //We would build the gnomeSkin here.

            newGnome.GetComponent<GnomeController>().InitializePlayer(player);
        }

        hasSpawned = true;
    }

    private void Update()
    {
        if(GameManager.Instance.playersReady && !hasSpawned)
        {
            InitPlayerGnomes();
        }
    }
}
