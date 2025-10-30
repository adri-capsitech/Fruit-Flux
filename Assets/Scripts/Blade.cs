
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Blade : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 prevPos, startPos, currentPos;
    public BoxCollider bladeColl;
    bool trailStart;
    public GameObject BladeLinePrefab;
    private Camera cam;
    GameObject Current;
    public float minVelocity = 0.01f, minDragDistance = 0.2f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        bladeColl = GetComponent<BoxCollider>();
        bladeColl.enabled = false;
    }
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        MouseInput();
#elif UNITY_ANDROID 
        TouchInput();
#endif
    }
    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = GetMousePosition();
            trailStart = false;
        }
        if (Input.GetMouseButton(0))
        {
            currentPos = GetMousePosition();
            rb.position = currentPos;
            if (!trailStart && Vector3.Distance(currentPos, startPos) > minDragDistance)
            {
                StartCutting();
            }
            else if (trailStart)
            {
                UpdateCut(currentPos);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }
    }
    void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = GetTouchPosition(touch.position);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touchPosition;
                    trailStart = false;
                    break;

                case TouchPhase.Moved:
                    currentPos = touchPosition;
                    rb.position = currentPos;

                    if (!trailStart && Vector3.Distance(currentPos, startPos) > minDragDistance)
                    {
                        StartCutting();
                    }
                    else if (trailStart)
                    {
                        UpdateCut(currentPos);

                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    StopCutting();
                    break;

            }

        }

    }
    Vector3 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9));
    }
    Vector3 GetTouchPosition(Vector3 touchPos)
    {
        return cam.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 9));
    }
    void UpdateCut(Vector3 newPos)
    {
        rb.position = newPos;
        float velocity = (newPos - prevPos).magnitude / Time.deltaTime;

        if (velocity > minVelocity)
        {
            bladeColl.enabled = true;
        }
        else
        {
            bladeColl.enabled = false;
        }
        prevPos = newPos;
    }
    void StartCutting()
    {
        trailStart = true;
        Current = Instantiate(BladeLinePrefab, transform);
        bladeColl.enabled = false;
    }
    void StopCutting()
    {

        trailStart = false;
        if (Current != null)
        {
            Current.transform.SetParent(null);
            Destroy(Current, 2f);

        }

        bladeColl.enabled = false;
    }
}


