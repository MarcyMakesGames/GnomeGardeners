using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField] protected List<WeatherObject> weatherTypes;
    [SerializeField] protected float weatherDuration;

    protected WeatherObject currentWeather;
    protected float currentWeatherTimer;

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
        currentWeatherTimer = 0f;
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
        currentWeatherTimer += Time.deltaTime;

        if(currentWeatherTimer >= weatherDuration)
        {
            int selectedWeather = Random.Range(0, weatherTypes.Count);
            currentWeather = weatherTypes[selectedWeather];

            WeatherChanged();
            currentWeatherTimer = 0f;
        }
    }
}

public enum WeatherType
{
    Sunny = 1,
    Rainy = 2,
    Windy = 3
}