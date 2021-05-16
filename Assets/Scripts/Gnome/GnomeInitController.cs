using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace GnomeGardeners
{
    public class GnomeInitController : MonoBehaviour
    {
        private bool debug = false;

        [SerializeField]
        private GameObject gnomePrefab;
        [SerializeField]
        private List<Transform> playerSpawnLocations;

        private bool hasSpawned = false;

        private void InitPlayerGnomes()
        {
            foreach(PlayerConfig player in GameManager.Instance.PlayerConfigManager.PlayerConfigs)
            {
                GameObject newGnome = Instantiate(gnomePrefab, playerSpawnLocations[player.PlayerIndex].position, gnomePrefab.transform.rotation, transform);
                //We would build the gnomeSkin here.

                newGnome.GetComponent<Gnome>().InitializePlayer(player);
                DebugLogger.Log(this, "Player " + player.Input.playerIndex + " device: " + player.Input.devices);

                player.Input.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
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
}