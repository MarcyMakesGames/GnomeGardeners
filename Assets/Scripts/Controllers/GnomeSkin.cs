using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeSkin
{
    SpriteRenderer mustache;
    SpriteRenderer hat;
    SpriteRenderer toolArm;

    public void ChangeArm(Sprite toolSprite)
    {
        toolArm.sprite = toolSprite;
    }
}
