using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float speed = 8;
    float visibleHeightThreshold;
    void Start()
    {
        visibleHeightThreshold = Camera.main.orthographicSize + transform.localScale.x;
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
         if (transform.position.x > visibleHeightThreshold)
         {
             Destroy(gameObject);
         }
    }
}
