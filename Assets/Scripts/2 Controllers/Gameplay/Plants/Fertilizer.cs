using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : MonoBehaviour, IHoldable
{
    [SerializeField] private float strength = 50f;
    [SerializeField] private Sprite spriteInHand;
    private bool isBeingCarried;
    private ItemType type = ItemType.Fertilizer;

    public float Strength { get => strength; }
    public Sprite SpriteInHand { get => spriteInHand; set => spriteInHand = value; }
    public bool IsBeingCarried { get => isBeingCarried; set => isBeingCarried = value; }
    public ItemType Type { get => type; set => type = value; }
}
