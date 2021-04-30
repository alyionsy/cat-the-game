using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 nextPos;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private Transform transform2;

    void Start()
    {
        pos1 = childTransform.localPosition;
        pos2 = transform2.localPosition;
        nextPos = pos2;
    }

    void Update()
    {
        Move();
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }

    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nextPos, speed * Time.deltaTime);

        if (Vector3.Distance(childTransform.localPosition, nextPos) <= 0.1)
        {
            ChangeDestination();
        }
    }

    private void ChangeDestination()
    {
        nextPos = nextPos != pos1 ? pos1 : pos2;
    }
}
