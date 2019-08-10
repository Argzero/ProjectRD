using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReticlePositioner : MonoBehaviour
{
    public bool Started = false;
    public float MoveSpeed = 5f;
    public float MThreshold = 0.2f;
    public GameObject BG;
    Vector2 curPos = Vector2.zero;
    public float Width = 3f; public float Height = 3f;
    Rigidbody2D rb;
    public enum ControlMode
    {
        Mouse,
        KB
    }
    public ControlMode Mode;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        HoleSpawner hs = FindObjectOfType<HoleSpawner>();
        if (Mode == ControlMode.KB)
            hs.MaxSpawnSpeed = 15;
        else
            hs.MaxSpawnSpeed = 10;
    }

    public void StartGame()
    {
        Started = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Started)
        {
            Cursor.visible = true;
            transform.localPosition = new Vector3(0, 0, 10);
            return;
        }
        if (Mode == ControlMode.KB)
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal") * Width, Input.GetAxis("Vertical") * Height);
            if (input != curPos)
            {
                curPos = input;
            }
            Vector2 dist = (curPos - (Vector2)transform.localPosition);
            dist = Vector2.ClampMagnitude(dist.normalized * MoveSpeed*100f*Time.fixedDeltaTime, dist.magnitude);
            transform.localPosition = transform.localPosition + (Vector3)dist;
        }
        if (Mode == ControlMode.Mouse) {
            Cursor.visible = false;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 newPos = Vector2.Lerp(transform.position, mousePosition, 1);
            Vector3 dVec = newPos - transform.position;
            Vector3 cVec = (dVec.magnitude < MThreshold) ? dVec.normalized * MThreshold : dVec;
            Vector3 mVec = mousePosition - transform.position;
            if (dVec.magnitude < 0.05f)
                return;
            if (mVec.magnitude < MThreshold)
                transform.position = mousePosition;
            else
                transform.position = transform.position+cVec;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Started)
            return;
        if(!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
