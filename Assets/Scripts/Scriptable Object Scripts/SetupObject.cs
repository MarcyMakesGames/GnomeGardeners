using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "World/Object")]
public class SetupObject : ScriptableObject
{
    public GameObject occupant;
    public Vector2Int position;
}
