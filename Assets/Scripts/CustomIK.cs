using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomIK : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject joint1;
    [SerializeField] private GameObject wrist;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject handTarget;

    private float rootAngle;
    private float joint1Angle;
    private Vector2 rootPos;
    private Vector2 targetPos;
    float scale;
    private bool hasControl = true;

    // Start is called before the first frame update
    void Start()
    {
        rootPos = root.transform.position;
        scale = transform.lossyScale.x;
    }

    public void SetUsable(bool usable)
    {
        hasControl = usable;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindAnyObjectByType<MenuManager>().GetPaused())
        {
            return;
        }
        if (hasControl)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            Vector2 d = (Vector2)handTarget.transform.position - (Vector2)wrist.transform.position;
            targetPos = wrist.transform.position + (Vector3)d.normalized * Time.deltaTime * Mathf.Min(1.5f, d.magnitude * 10f);
            if (d.magnitude < .075)
            {
                FindAnyObjectByType<RowerController>().OnHandReturn();
            }
        }
        rootPos = root.transform.position;
        Vector2 delta = targetPos - rootPos;
        float len = Mathf.Clamp((delta).magnitude / 2 / scale, .2f, 1.95f);
        joint1Angle = Mathf.Acos((len * len - 2) / 2) * Mathf.Rad2Deg;
        joint1.transform.localRotation = Quaternion.Euler(0, 0, joint1Angle);

        rootAngle = -joint1Angle / 2;
        rootAngle += Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        root.transform.localRotation = Quaternion.Euler(0, 0, rootAngle);

        if (hasControl)
        {
            wrist.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
