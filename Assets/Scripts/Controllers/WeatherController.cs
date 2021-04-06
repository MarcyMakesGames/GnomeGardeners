using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WeatherController : MonoBehaviour
{
    [SerializeField] protected List<WeatherObject> weatherTypes;
    [SerializeField] protected float weatherDuration;

    protected WeatherObject currentWeather;
    protected float currentWeatherTimer = 0f;

    public WeatherObject CurrentWeather { get => currentWeather; }

    public delegate void OnWeatherChange();
    public event OnWeatherChange WeatherChanged;

    protected void Start()
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

        WeatherChanged += AnnounceWeather;
        currentWeatherTimer = GameManager.Instance.Time.ElapsedTime;
    }

    protected void Update()
    {
        WeatherCountdown();
    }

    protected void AnnounceWeather()
    {
        Debug.Log("Current weather is: " + currentWeather.weatherType.ToString());
    }

    protected void WeatherCountdown()
    {
        if(GameManager.Instance.Time.GetTimeSince(currentWeatherTimer) >= weatherDuration)
        {
            int selectedWeather = Random.Range(0, weatherTypes.Count);
            currentWeather = weatherTypes[selectedWeather];

            WeatherChanged();
            currentWeatherTimer = GameManager.Instance.Time.ElapsedTime;
        }
    }
}

public enum WeatherType
{
    Sunny = 1,
    Rainy,
    Windy
}