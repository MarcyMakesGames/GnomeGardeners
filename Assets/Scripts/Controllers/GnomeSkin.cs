using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeSkin : MonoBehaviour
{
    public SpriteRenderer mustache;
    public SpriteRenderer hat;
    public SpriteRenderer toolArm;

    public void ChangeArm(Sprite toolSprite)
    {
        toolArm.sprite = toolSprite;
    }
}
