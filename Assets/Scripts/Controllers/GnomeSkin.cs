using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeSkin : MonoBehaviour
{
    public SpriteRenderer mustache;
    public SpriteRenderer hat;
    public SpriteRenderer toolArm;

    public void ChangeArm(SpriteRenderer renderer)
    {
        toolArm.sprite = renderer.sprite;
        toolArm.color = renderer.color;
    }

    public void ResetArm()
    {
        toolArm.color = Color.white;
    }
}
