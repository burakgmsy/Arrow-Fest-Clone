using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] public Transform target;
    [SerializeField] public Vector3 offset;
    ArrowStack arrowStack;
    

    private void Start()
    {
        arrowStack = GameObject.Find("ArrowStack").GetComponent<ArrowStack>();
        offset = transform.position - target.position;
    }
    void FixedUpdate()
    {
        if (arrowStack.inFinishLine)
            FinishFollow();
        else
            Follow();
    }
    private void Follow()
    {
        transform.position = new Vector3(0, offset.y + target.transform.position.y, offset.z + target.transform.position.z);
    }
    private void FinishFollow()
    {
        transform.position = new Vector3(0, Mathf.Lerp(transform.position.y, offset.y + target.transform.position.y + 1f, 2f * Time.deltaTime), offset.z + target.transform.position.z);
    }
}

