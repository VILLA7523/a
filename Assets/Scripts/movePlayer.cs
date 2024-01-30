using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb2D;
    
    public float movimientoFuerza = 5f;

    private void Start()
    {
      rb2D = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    private void Update()
    {
        float movementx = Input.GetAxisRaw("Horizontal");
        Vector2 posicionJug = transform.position;

        posicionJug = posicionJug + new Vector2(movementx , 0f) * movimientoFuerza * Time.deltaTime;

        transform.position = posicionJug;
    }

    private void FixedUpdate() {

    }


}

