using UnityEngine;

[System.Serializable]
public class Stage
{
    public PlantStage specifier;
    [SerializeField] private int index;
    [SerializeField] public Sprite sprite;
    [SerializeField] private float timeToNextStage = 5f;
    [SerializeField] private Need need;
    [HideInInspector] public string name;


    public float TimeToNextStage { get => timeToNextStage; }
    public int Index { get => index; }

    public Stage()
    {
        name = specifier.ToString();
    }

    public bool IsReady() 
    {
        return need.IsFulfilled;
    }

    public void SatisfyNeed(float value)
    {
        need.Satisfy(value);
    }
}
