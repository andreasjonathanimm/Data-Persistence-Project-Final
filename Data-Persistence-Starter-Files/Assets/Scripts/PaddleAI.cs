using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    public float Speed = 2.0f;
    public float MaxMovement = 2.0f;
    public float offset = 0.25f;
    public Transform ball;

    private void Start()
    {
        if (ball.position.x < transform.position.x) offset = -offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.gameObject.activeInHierarchy)
        {
            if (ball.position.x == transform.position.x)
            {
                offset = -offset;
            }
            Vector3 pos = transform.position;
            pos.x += (ball.position.x - offset - transform.position.x) * Speed * Time.deltaTime;

            if (pos.x > MaxMovement)
                pos.x = MaxMovement;
            else if (pos.x < -MaxMovement)
                pos.x = -MaxMovement;

            transform.position = pos;
        }
    }
}
