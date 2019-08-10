using UnityEngine;
using UnityEngine.UI;
public class ColorTimer : MonoBehaviour
{
    Text c;
    public bool Started = false;
    public float time = 0f;
    public float RotSpeed = 30f;
    public float Angle = 0f;
    HoleSpawner hs;
    // Start is called before the first frame update
    void Awake()
    {
        c = GetComponent<Text>();
        UpdateAngle();
        hs = FindObjectOfType<HoleSpawner>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Started)
            time += Time.fixedDeltaTime;
        Color col = c.color;
        float h, s, v;
        Color.RGBToHSV(col, out h, out s, out v);
        h = Mathf.Clamp(h + RotSpeed * Time.fixedDeltaTime * 60f / 360f, 0f, 1f);
        c.color = Color.HSVToRGB(h, s, v);
        c.text = ""+(int)time;

        if (time < 60f)
        {
            Time.timeScale = 0.8f+0.6f*Mathf.Clamp01(Time.time / 60f);
            hs.SpawnSpeed = hs.MaxSpawnSpeed + (20f - hs.MaxSpawnSpeed) * Mathf.Clamp01((60f - Time.time) / 60f);
        }
    }

    public void StartGame()
    {
        Started = true;
    }

    private void UpdateAngle()
    {
        Angle = Quaternion.Angle(Quaternion.identity, Quaternion.FromToRotation(Vector3.up, transform.position));
    }
}
