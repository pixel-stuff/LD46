﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class MaskSlideComponent : MonoBehaviour
{

    public SpriteRenderer TargetSprite;
    public SpriteRenderer TargetMask;
    public float MaskFactor = 0.5f;

    public float VisibleFactor = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetSprite)
        {
            this.transform.localPosition = new Vector3(0, -VisibleFactor * TargetSprite.size.x, 0);
            TargetSprite.transform.localPosition = new Vector3(0, VisibleFactor * TargetSprite.size.x, 0);
        }
        if (TargetMask)
            TargetMask.transform.localPosition = new Vector3(0, VisibleFactor * TargetMask.size.y * MaskFactor, 0);
    }
}