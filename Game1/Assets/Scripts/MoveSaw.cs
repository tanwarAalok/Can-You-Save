using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSaw : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float turnSpeed = 5f;
    BoxCollider2D boxCollider2D;
    [SerializeField] GameObject saw;
    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        saw.transform.Rotate(new Vector3(0,0,1), turnSpeed *Time.deltaTime);
    }
    void Flip()
    {
        moveSpeed  *= -1;
        turnSpeed *= -1;
        transform.localScale = new Vector3((transform.localScale.x * -1), transform.localScale.y ,transform.localScale.z);

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Flip();
    }
}
