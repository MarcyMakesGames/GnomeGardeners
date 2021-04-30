using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [SerializeField]
    private List<Hazard> hazards;
    [SerializeField]
    private bool randomizeHazards;
    [SerializeField]
    private float hazardDelayTime;

    private float timeBetweenHazards;

    private int currentHazardIndex = 0;

    private float currentHazardTimer = 0f;

    public ScriptableObject CurrentHazard { get => hazards[currentHazardIndex]; }

    public delegate void OnHazardChange();
    public event OnHazardChange HazardChanged;

    #region Unity Methods
    private void Awake()
    {
        GameManager.Instance.HazardController = this;
    }

    private void Start()
    {
        currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
    }

    private void Update()
    {
        HazardCountdown();
    }
    #endregion

    #region Private Methods
    private void HazardCountdown()
    {
        if(hazardDelayTime > 0f)
        {
            hazardDelayTime -= Time.deltaTime;
            return;
        }

        if (!randomizeHazards && currentHazardIndex == hazards.Count - 1)
            return; 

        if (GameManager.Instance.Time.GetTimeSince(currentHazardTimer) >= timeBetweenHazards)
        {
            if(randomizeHazards)
                GetRandomHazard().SpawnHazard();
            else
                GetNextHazard().SpawnHazard();

            HazardChanged();
            currentHazardTimer = GameManager.Instance.Time.ElapsedTime;
            timeBetweenHazards = hazards[currentHazardIndex].HazardDuration;
        }
    }

    private Hazard GetRandomHazard()
    {
        currentHazardIndex = Random.Range(0, hazards.Count);
        return hazards[currentHazardIndex];
    }

    private Hazard GetNextHazard() => hazards[++currentHazardIndex];
    #endregion
}
