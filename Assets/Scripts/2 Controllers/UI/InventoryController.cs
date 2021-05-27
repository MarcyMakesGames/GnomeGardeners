using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
	public class InventoryController : MonoBehaviour
	{
		[SerializeField] private GameObject[] inventories;
		[SerializeField] private Sprite[] playerColors = new Sprite[16];

		private Image[] toolImages = new Image[4];
		private Image[] playerImages = new Image[4];

		private bool hasReceivedSpriteThisFrame;
		private Sprite lastEquippedToolSprite;
		private bool hasReceivedPlayerNumberThisFrame;
		private int lastPlayerEquippingTool;

		private Sprite transparent;
		
		
		private IntEventChannelSO OnNumberOfPlayersEvent;
		private IntIntEventChannelSO OnPlayerColorAssignedEvent;
		private SpriteEventChannelSO OnEquippedToolSpriteEvent;
		private IntEventChannelSO OnPlayerEquippingToolEvent;
		private IntEventChannelSO OnToolUnequippedByPlayerEvent;
		
		#region Unity Methods

		private void Start()
		{
			foreach (var inventory in inventories)
				inventory.SetActive(false);
			for (int i = 0; i < 4; ++i)
			{
				toolImages[i] = inventories[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
				playerImages[i] = inventories[i].transform.GetChild(1).GetComponent<Image>();
			}
			
			OnNumberOfPlayersEvent = Resources.Load<IntEventChannelSO>("Channels/NumberOfPlayersEC");
			OnPlayerColorAssignedEvent = Resources.Load<IntIntEventChannelSO>("Channels/PlayerColorAssignedEC");
			OnEquippedToolSpriteEvent = Resources.Load<SpriteEventChannelSO>("Channels/EquippedToolSpriteEC");
			OnPlayerEquippingToolEvent = Resources.Load<IntEventChannelSO>("Channels/PlayerEquippingToolEC");
			OnToolUnequippedByPlayerEvent = Resources.Load<IntEventChannelSO>("Channels/ToolUnequippedByPlayerEC");


			transparent = Resources.Load<Sprite>("transparent");

			OnNumberOfPlayersEvent.OnEventRaised += SetInventoriesActive;
			OnPlayerColorAssignedEvent.OnEventRaised += UpdatePlayerColor;
			OnEquippedToolSpriteEvent.OnEventRaised += SetEquippedToolSprite;
			OnPlayerEquippingToolEvent.OnEventRaised += SetPlayerEquippingTool;
			OnToolUnequippedByPlayerEvent.OnEventRaised += ClearToolIconForPlayer;
			
			hasReceivedSpriteThisFrame = false;
			hasReceivedPlayerNumberThisFrame = false;
		}

		private void Update()
		{
			hasReceivedSpriteThisFrame = false;
			hasReceivedPlayerNumberThisFrame = false;
		}

		private void OnDestroy()
		{
			OnNumberOfPlayersEvent.OnEventRaised -= SetInventoriesActive;
			OnPlayerColorAssignedEvent.OnEventRaised -= UpdatePlayerColor;
			OnEquippedToolSpriteEvent.OnEventRaised -= SetEquippedToolSprite;
			OnPlayerEquippingToolEvent.OnEventRaised -= SetPlayerEquippingTool;
			OnToolUnequippedByPlayerEvent.OnEventRaised -= ClearToolIconForPlayer;
		}
		
		#endregion


		#region Private Methods

		private void SetInventoriesActive(int numberOfPlayers)
		{
			for(int i = 0; i < numberOfPlayers; ++i) 
				inventories[i].SetActive(true);
		}
		
		private void SetEquippedToolSprite(Sprite sprite)
		{
			lastEquippedToolSprite = sprite;
			hasReceivedSpriteThisFrame = true;
			UpdateToolIcon();
		}

		private void SetPlayerEquippingTool(int number)
		{
			lastPlayerEquippingTool = number;
			hasReceivedPlayerNumberThisFrame = true;
			UpdateToolIcon();
		}
		
		private void UpdateToolIcon()
		{
			if (hasReceivedSpriteThisFrame && hasReceivedPlayerNumberThisFrame)
				toolImages[lastPlayerEquippingTool].sprite = lastEquippedToolSprite != null ? lastEquippedToolSprite : transparent;
		}
		
		private void ClearToolIconForPlayer(int playerNumber)
		{
			toolImages[playerNumber].sprite = transparent;
		}

		private void UpdatePlayerColor( int playerNumber, int gnomeSkin)
		{
			var index = playerNumber * 4 + gnomeSkin;
			playerImages[playerNumber].sprite = playerColors[index];
		}

		#endregion
	}
}