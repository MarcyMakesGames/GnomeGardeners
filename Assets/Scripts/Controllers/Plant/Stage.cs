using UnityEngine;

[System.Serializable]
public class Stage
{
    public string name;
    [SerializeField] private int index;
    [SerializeField] public Sprite sprite;
    [SerializeField] private float timeToNextStage = 5f;
    [SerializeField] private Need need;

    public float TimeToNextStage { get => timeToNextStage; }
    public int Index { get => index; }

    public bool IsReady() 
    {
        return need.IsFulfilled;
    }

    public void SatisfyNeed(float value)
    {
        need.Satisfy(value);
    }
}
