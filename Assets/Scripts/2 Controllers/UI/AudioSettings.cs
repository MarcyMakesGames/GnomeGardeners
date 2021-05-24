using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GnomeGardeners
{
	public class AudioSettings : MonoBehaviour
	{
		public Slider sliderVolumeMaster;
		public Slider sliderVolumeSound;
		public Slider sliderVolumeMusic;
		public Slider sliderVolumeAmbience;


		private FloatEventChannelSO OnMasterVolumeChanged;
		private FloatEventChannelSO OnSoundVolumeChanged;
		private FloatEventChannelSO OnMusicVolumeChanged;
		private FloatEventChannelSO OnAmbienceVolumeChanged;

        #region Unity Methods

        private void Awake()
        {
			OnMasterVolumeChanged = Resources.Load<FloatEventChannelSO>("Channels/MasterVolumeChangedEC");
			OnSoundVolumeChanged = Resources.Load<FloatEventChannelSO>("Channels/SoundVolumeChangedEC");
			OnMusicVolumeChanged = Resources.Load<FloatEventChannelSO>("Channels/MusicVolumeChangedEC");
			OnAmbienceVolumeChanged = Resources.Load<FloatEventChannelSO>("Channels/AmbienceVolumeChangedEC");
		}

        private void Start()
        {
			sliderVolumeMaster.value = GameManager.Instance.AudioManager.MasterVolume;
			sliderVolumeSound.value = GameManager.Instance.AudioManager.SoundVolume;
			sliderVolumeMusic.value = GameManager.Instance.AudioManager.MusicVolume;
			sliderVolumeAmbience.value = GameManager.Instance.AudioManager.AmbienceVolume;
        }

        #endregion

        #region Public Methods

        public void SetMasterVolume(float volume)
		{
			OnMasterVolumeChanged.RaiseEvent(volume);
		}

		public void SetSoundVolume(float volume)
		{
			OnSoundVolumeChanged.RaiseEvent(volume);
		}

		public void SetMusicVolume(float volume)
		{
			OnMusicVolumeChanged.RaiseEvent(volume);
		}

		public void SetAmbienceVolume(float volume)
		{
			OnAmbienceVolumeChanged.RaiseEvent(volume);
		}

		#endregion

		#region Private Methods



		#endregion
	}
}
