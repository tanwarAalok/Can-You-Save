using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSaw : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    bool deadEnd = false;
    [SerializeField] float turnSpeed = 5f;
    BoxCollider2D boxCollider2D;
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
        //transform.Rotate(new Vector3(0,0,1), turnSpeed *Time.deltaTime);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("here");
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy")) return;
        moveSpeed  *= -1;
        transform.localScale = new Vector3((transform.localScale.x * -1), transform.localScale.y ,transform.localScale.z);
    }
}
