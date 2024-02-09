using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb2D;
    
    public float movimientoFuerza = 5f;

    public static float positionX;

    private void Start()
    {
      rb2D = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    private void Update()
    {
        float movementx = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        
        Vector2 posicionJug = transform.position;

        posicionJug = posicionJug + new Vector2(movementx , 0f) * movimientoFuerza * Time.deltaTime;

        transform.position = posicionJug;

        positionX = posicionJug.x;
    }

}
