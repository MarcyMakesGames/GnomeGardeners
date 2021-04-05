using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessingController : MonoBehaviour
{
    PostProcessVolume volume;
    WeatherController weather;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        weather = FindObjectOfType<WeatherController>();
        weather.WeatherChanged += ChangeProfile;
    }

    private void ChangeProfile()
    {
        volume.profile = weather.CurrentWeather.weatherProfile;
    }
}
