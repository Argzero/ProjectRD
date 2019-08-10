using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotator : MonoBehaviour
{
    Camera c;
    public bool Started = false;
    public float RotSpeed = 30f;
    public float DistortionSpeed = 1f;
    public float Angle = 0f;
    public float SinT = 0f;
    public float SinMag = 1f;
    public float OscSpeed = 0.3f;

    // Start is called before the first frame update
    void Awake()
    {
        c = GetComponent<Camera>();
        UpdateAngle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateAngle();
        Color col = c.backgroundColor;
        float h, s, v;
        Color.RGBToHSV(col, out h, out s, out v);
        h = Mathf.Clamp(h + RotSpeed * Time.fixedDeltaTime * 60f / 360f, 0f, 1f);
        c.backgroundColor = Color.HSVToRGB(h, s, v);

        float size;
        SinT = OscSpeed * Time.time;
        size = 5 + SinMag * Mathf.Sin(SinT);
        c.orthographicSize = size;

        if (!Started)
            return;
        transform.RotateAround(Vector3.zero, Vector3.forward, RotSpeed * Time.deltaTime * 60f);
        transform.Rotate(Vector3.forward, DistortionSpeed * Time.fixedDeltaTime * 60f);
    }

    public void StartGame()
    {
        Started = true;
    }

    private void UpdateAngle()
    {
        Angle = Quaternion.FromToRotation(Vector3.up, transform.position).eulerAngles.z;
    }
}
