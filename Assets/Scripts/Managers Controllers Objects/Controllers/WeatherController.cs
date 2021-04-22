using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private List<WeatherObject> weatherTypes;
    [SerializeField] private float weatherDuration;

    private WeatherObject currentWeather;
    private float currentWeatherTimer = 0f;

    public WeatherObject CurrentWeather { get => currentWeather; }

    public delegate void OnWeatherChange();
    public event OnWeatherChange WeatherChanged;

    #region Unity Methods
    private void Start()
    {
        foreach(WeatherObject weather in weatherTypes)
        {
            if (weather.weatherType == WeatherType.Sunny)
            {
                currentWeather = weather;
                AnnounceWeather();
                break;
            }
        }

        // WeatherChanged += AnnounceWeather;
        currentWeatherTimer = GameManager.Instance.Time.ElapsedTime;
    }

    private void Update()
    {
        WeatherCountdown();
    }
    #endregion

    #region Private Methods
    private void AnnounceWeather()
    {
        Debug.Log("Current weather is: " + currentWeather.weatherType.ToString());
    }

    private void WeatherCountdown()
    {
        if(GameManager.Instance.Time.GetTimeSince(currentWeatherTimer) >= weatherDuration)
        {
            int selectedWeather = Random.Range(0, weatherTypes.Count);
            currentWeather = weatherTypes[selectedWeather];

            WeatherChanged();
            currentWeatherTimer = GameManager.Instance.Time.ElapsedTime;
        }
    }
    #endregion
}
