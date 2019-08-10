using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    GameObject c;
    public float OrigSize;
    public float RotSpeed = 1f;
    public float ColorSpeed = 1f;
    public float DistortionSpeed = 1f;
    public float Angle = 0f;
    public float SinT = 0f;
    public float SinMag = 1f; // Percent of Original Size to Oscillate
    public float OscSpeed = 0.3f;

    // Start is called before the first frame update
    void Awake()
    {
        c = gameObject;
        UpdateAngle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color col = c.GetComponent<SpriteRenderer>().color;
        transform.Rotate(Vector3.forward, DistortionSpeed * Time.deltaTime * 60f);
        float h, s, v;
        Color.RGBToHSV(col, out h, out s, out v);
        h = Mathf.Clamp(h + ColorSpeed * Time.fixedDeltaTime * 60f / 360f, 0f, 1f);
        c.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(h, s, v);
        UpdateAngle();

        float size = OrigSize;
        SinT = OscSpeed * Time.time;
        size += SinMag*OrigSize * Mathf.Sin(SinT);
        Vector3 scale = Vector3.one * size;
        scale.z = 1f;
        transform.localScale = scale;
    }

    private void UpdateAngle()
    {
        Angle = Quaternion.Angle(Quaternion.identity, Quaternion.FromToRotation(Vector3.up, transform.position));
    }
}
