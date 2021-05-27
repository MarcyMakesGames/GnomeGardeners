using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace GnomeGardeners
{
    public class GnomeInitController : MonoBehaviour
    {
        private bool debug = false;

        [SerializeField] private GameObject gnomePrefab;
        [SerializeField] private GameObject gnomePrefab2;
        [SerializeField] private GameObject gnomePrefab3;
        [SerializeField] private GameObject gnomePrefab4;
        [SerializeField] private List<Transform> playerSpawnLocations;

        private bool hasSpawned = false;

        private IntEventChannelSO OnNumberOfPlayersEvent;
        private IntIntEventChannelSO OnPlayerColorAssignedEvent;

        private void Start()
        {
            OnNumberOfPlayersEvent = Resources.Load<IntEventChannelSO>("Channels/NumberOfPlayersEC");
            OnPlayerColorAssignedEvent = Resources.Load<IntIntEventChannelSO>("Channels/PlayerColorAssignedEC");
        }

        private void InitPlayerGnomes()
        {
            foreach(PlayerConfig player in GameManager.Instance.PlayerConfigManager.PlayerConfigs)
            {
                GameObject newGnome;
                switch (player.GnomeSkin.GnomeSkinNumber)
                {
                    case 0:
                         newGnome = Instantiate(gnomePrefab, playerSpawnLocations[player.PlayerIndex].position, gnomePrefab.transform.rotation, transform);
                        break;
                    case 1:
                        newGnome = Instantiate(gnomePrefab2, playerSpawnLocations[player.PlayerIndex].position, gnomePrefab2.transform.rotation, transform);
                        break;
                    case 2:
                        newGnome = Instantiate(gnomePrefab3, playerSpawnLocations[player.PlayerIndex].position, gnomePrefab3.transform.rotation, transform);
                        break;
                    case 3:
                        newGnome = Instantiate(gnomePrefab4, playerSpawnLocations[player.PlayerIndex].position, gnomePrefab4.transform.rotation, transform);
                        break;
                    default:
                        newGnome = Instantiate(gnomePrefab, playerSpawnLocations[player.PlayerIndex].position, gnomePrefab.transform.rotation, transform);
                        break;

                }
                newGnome.GetComponent<Gnome>().InitializePlayer(player);
                DebugLogger.Log(this, "Player " + player.Input.playerIndex + " device: " + player.Input.devices);
                OnPlayerColorAssignedEvent.RaiseEvent( player.PlayerIndex, player.GnomeSkin.GnomeSkinNumber);

            }
            GameManager.Instance.PlayerConfigManager.PlayerConfigs[0].Input.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
            hasSpawned = true;
            OnNumberOfPlayersEvent.RaiseEvent(GameManager.Instance.PlayerConfigManager.PlayerCount);
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
