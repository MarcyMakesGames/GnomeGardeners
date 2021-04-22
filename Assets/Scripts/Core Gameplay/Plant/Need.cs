using UnityEngine;

[System.Serializable]
public class Need
{
    public string name;
    public int index;
    [HideInInspector] private bool isFulfilled;
    [SerializeField] private float value = 0f;
    [SerializeField] private float threshold = 30f;
    public bool IsFulfilled
    {
        get
        {
            if (value >= threshold)
                return isFulfilled = true;
            else
                return false;
        }
    }

    public void Satisfy(float value)
    {
        this.value += value;
    }
}
