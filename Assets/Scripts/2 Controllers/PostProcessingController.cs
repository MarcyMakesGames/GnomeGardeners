using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace GnomeGardeners
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class PostProcessingController : MonoBehaviour
    {
        private PostProcessVolume volume;
        private WeatherController weather;

        #region Unity Methods
        private void Awake()
        {
            volume = GetComponent<PostProcessVolume>();
            weather = FindObjectOfType<WeatherController>();
            weather.WeatherChanged += ChangeProfile;
        }
        #endregion

        #region Private Methods
        private void ChangeProfile()
        {
            volume.profile = weather.CurrentWeather.weatherProfile;
        }
        #endregion
    }
}
