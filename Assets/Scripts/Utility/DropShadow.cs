using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{
    public Vector2 shadowOffset;
    public Material shadowMaterial;

    private SpriteRenderer spriteRenderer;
    private GameObject shadowGameObject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shadowGameObject = new GameObject();
        Instantiate(shadowGameObject, transform);

        SpriteRenderer shadowSpriteRenderer = shadowGameObject.AddComponent<SpriteRenderer>();
        shadowSpriteRenderer.sprite = spriteRenderer.sprite;
        shadowSpriteRenderer.material = shadowMaterial;
        shadowSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        shadowGameObject.transform.localPosition = transform.localPosition + (Vector3)shadowOffset;
        shadowGameObject.transform.localRotation = transform.localRotation;
    }
}