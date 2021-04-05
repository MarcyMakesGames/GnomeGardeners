using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class Ground : MonoBehaviour
{
    public GroundType type = GroundType.Dirt;
}
