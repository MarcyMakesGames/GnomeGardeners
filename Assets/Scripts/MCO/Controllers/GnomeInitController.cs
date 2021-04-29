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
        foreach(PlayerConfig player in GameManager.Instance.PlayerConfigManager.PlayerConfigs)
        {
            GameObject newGnome = Instantiate(gnomePrefab, playerSpawnLocations[player.PlayerIndex].position, gnomePrefab.transform.rotation, transform);
            //We would build the gnomeSkin here.

            newGnome.GetComponent<GnomeController>().InitializePlayer(player);
            Debug.Log("Player " + player.Input.playerIndex + " device: " + player.Input.devices);
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
