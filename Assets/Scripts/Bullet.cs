using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float speed;
    float startingPosition;
    float visibleHeightThreshold;
    [SerializeField] RangedWeapon range;
    void Start()
    {
        
        startingPosition = transform.position.x;
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
         if (Mathf.Abs(transform.position.x - startingPosition) >= range.maxDÝstance)
         {
             Destroy(gameObject);
         }
    }
}
