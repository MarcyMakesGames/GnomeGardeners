using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Ground : MonoBehaviour
{
    public GroundType type = GroundType.Dirt;
}
