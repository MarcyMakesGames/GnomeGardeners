using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Need", menuName = "Plants/Need")]
public class Need : ScriptableObject
{
    public NeedType type;
    public float threshold;
    public Sprite popUp;
}
