using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRotator : MonoBehaviour
{
    SpriteRenderer c;
    public float RotSpeed = 30f;
    public float Angle = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        c = GetComponent<SpriteRenderer>();
        UpdateAngle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color col = c.color;
        float h, s, v;
        Color.RGBToHSV(col, out h, out s, out v);
        h = Mathf.Clamp(h + RotSpeed * Time.fixedDeltaTime * 60f / 360f, 0f, 1f);
        c.color = Color.HSVToRGB(h, s, v);
    }

    private void UpdateAngle()
    {
        Angle = Quaternion.Angle(Quaternion.identity, Quaternion.FromToRotation(Vector3.up, transform.position));
    }
}
