using System.Collections.Generic;
using UnityEngine;

public class HoleSpawner : MonoBehaviour
{
    public ReticlePositioner Player;
    public float PlayerAngle;
    public GameObject HolePrefab;
    public float MinOffset = 0.3f;
    public float MaxOffset = 0.5f;
    public float NextAngle = 0f;
    public float SpawnSpeed = 20f;
    public float MaxSpawnSpeed = 20f;
    public List<ObjectTimePair> OTPairs;
    public List<GameObject> Pool;
    public int PoolSize = 10;
    public int PoolIndex = 0;
    public Quaternion DefaultQuat;
    public Vector3 DefaultPos;

    private void Awake()
    {
        OTPairs = new List<ObjectTimePair>();
        for (int i = 0; i < PoolSize; i++) { 
            GameObject g = (Instantiate(HolePrefab, transform));
            if (i==0)
            {
                DefaultPos = g.transform.position;
                DefaultQuat = g.transform.rotation;
            }
            g.transform.position = Vector3.one * 1000f;
            Pool.Add(g);
        }
    }

    private void Spawn()
    {
        Debug.Log("Spawn!");
        GameObject g = Pool[PoolIndex++];
        g.transform.position = DefaultPos;
        g.transform.rotation = DefaultQuat;
        if (PoolIndex > Pool.Count - 1)
            PoolIndex = 0;
        g.transform.localPosition = new Vector3(0, MinOffset + (MaxOffset - MinOffset)*Random.Range(0f, 1f), 0);
        float newAng = 90f + PlayerAngle;
        g.transform.RotateAround(transform.position, Vector3.forward, newAng);
        float dAngle = 270f + PlayerAngle;

        while (dAngle < 0f)
            dAngle += 360f;
        while (dAngle >= 360f)
            dAngle -= 360f;
        OTPairs.Add(new ObjectTimePair() { Obj = g, DeathAngle = dAngle });
        NextAngle = PlayerAngle + SpawnSpeed;
    }

    public struct ObjectTimePair
    {
        public float DeathAngle;
        public GameObject Obj;
    }

    private void UpdateAngle()
    {
        PlayerAngle = FindObjectOfType<CamRotator>().Angle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateAngle();

        while (PlayerAngle < 0f)
            PlayerAngle += 360f;
        while (PlayerAngle >= 360f)
            PlayerAngle -= 360f;

        while (NextAngle < 0f)
            NextAngle += 360f;
        while (NextAngle >= 360f)
            NextAngle -= 360f;

        if (PlayerAngle>=0f && PlayerAngle<180f) { 
            if(NextAngle<=360f && NextAngle>270f)
                Spawn();
            else if (NextAngle <= 270f && NextAngle >= 180f) { 

            }
            else if(PlayerAngle > NextAngle) {
                Spawn();
            }
        }
        else if (PlayerAngle >= 180f && PlayerAngle < 360f)
        {
            if (NextAngle >= 90) { 
                if (PlayerAngle > NextAngle)
                {
                    Spawn();
                }
            }
        }

        List<int> KillPairs = new List<int>();
        int i = 0;
        Vector2 posQuad = Vector2.one;
        if (PlayerAngle <= 90 && PlayerAngle >= 0)
            posQuad.x *= -1;
        else if (PlayerAngle <= 180 && PlayerAngle > 90)
            posQuad *= -1;
        else if (PlayerAngle <= 270 && PlayerAngle > 180)
            posQuad.y *= -1;

        foreach (ObjectTimePair p in OTPairs) {
            Vector2 pQuad = Vector2.one;
            if (p.DeathAngle <= 90 && p.DeathAngle >= 0)
                pQuad.x *= -1;
            else if (p.DeathAngle <= 180 && p.DeathAngle > 90)
                pQuad *= -1;
            else if (p.DeathAngle <= 270 && p.DeathAngle > 180)
                pQuad.y *= -1;

            if (pQuad==posQuad) {
                p.Obj.transform.position+=Vector3.one*Mathf.Infinity;
                KillPairs.Add(i);
            }
            i++;
        }
        KillPairs.Reverse();
        for (i=0; i<KillPairs.Count; i++){
            ObjectTimePair otp = OTPairs[KillPairs[i]];
            OTPairs.Remove(otp);
            Debug.Log("Remove!");
        }
    }

}
public static class Vector2Extension
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}