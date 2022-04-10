using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackGroundPanel : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    [SerializeField] float StartingPosition;
    [SerializeField] float OutOfViewpoint;

    // Update is called once per frame
    void Update()
    {
       transform.position= new Vector2(transform.position.x,transform.position.y - MovementSpeed);
        if( transform.position.y < OutOfViewpoint)
        {
            transform.position = new Vector2(transform.position.x , StartingPosition);
        }
    }
    


}
