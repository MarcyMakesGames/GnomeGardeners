using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : MonoBehaviour, IHoldable
{
    [SerializeField] private Sprite spriteInHand;
    private bool isBeingCarried;
    public bool IsBeingCarried { get => isBeingCarried; set => isBeingCarried = value; }
    public Sprite SpriteInHand { get => spriteInHand; set => spriteInHand = value; }
}
