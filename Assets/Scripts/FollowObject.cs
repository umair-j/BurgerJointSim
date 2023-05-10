using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool isFollowStart;
    public Transform target;
    public float followSpeed;
    public void UpdatePosition(Transform followedObject, float speed, bool followStart)
    {
        isFollowStart = followStart;
        target = followedObject;
        followSpeed = speed;
    }
    private void Update()
    {
        if (isFollowStart)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime),
                transform.position.y,
                Mathf.Lerp(transform.position.z, target.position.z, followSpeed * Time.deltaTime));
        }
    }
}
