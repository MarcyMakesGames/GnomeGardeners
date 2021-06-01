using System;
using System.Collections;
using System.Collections.Generic;
using GnomeGardeners;
using UnityEngine;
using TMPro;

	public class GameOverMenuController : MonoBehaviour
	{
		[SerializeField] private GameObject messageWin;
		[SerializeField] private GameObject messageLose;
		
		[SerializeField] private TextMeshProUGUI text;

		private VoidEventChannelSO OnLevelWin;
		private VoidEventChannelSO OnLevelLose;
		private VoidEventChannelSO OnLevelEnd;
		private IntEventChannelSO OnCurrentLevelCurrentScore;

		private int totalScore;

		#region Unity Methods

		private void Awake()
		{
			OnLevelWin = Resources.Load<VoidEventChannelSO>("Channels/LevelWinEC");
			OnLevelLose = Resources.Load<VoidEventChannelSO>("Channels/LevelLoseEC");
			OnLevelEnd = Resources.Load<VoidEventChannelSO>("Channels/LevelEndEC");
			OnCurrentLevelCurrentScore = Resources.Load<IntEventChannelSO>("Channels/CurrentLevelCurrentScoreEC");

		}

		private void Start()
		{
			OnLevelWin.OnEventRaised += ActivateWinMessage;
			OnLevelLose.OnEventRaised += ActiveLoseMessage;
			OnLevelEnd.OnEventRaised += UpdateScoreText;
			OnCurrentLevelCurrentScore.OnEventRaised += SetTotalScore;
		}

		private void OnDestroy()
		{
			OnLevelWin.OnEventRaised -= ActivateWinMessage;
			OnLevelLose.OnEventRaised -= ActiveLoseMessage;
			OnLevelEnd.OnEventRaised -= UpdateScoreText;
			OnCurrentLevelCurrentScore.OnEventRaised -= SetTotalScore;
		}

		private void UpdateScoreText()
		{
			text.text = totalScore.ToString();
		}

		#endregion

		#region Private Methods

		private void ActivateWinMessage()
		{

			messageWin.SetActive(false);
			messageLose.SetActive(false);
			messageWin.SetActive(true);
		}

		#endregion

		private void ActiveLoseMessage()
		{

			messageWin.SetActive(false);
			messageLose.SetActive(false);
			messageLose.SetActive(true);		
		}

		private void SetTotalScore(int value)
		{
			totalScore = value;
		}
	}