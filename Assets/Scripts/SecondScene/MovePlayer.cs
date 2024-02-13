using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StressProblems;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb2D;

    private SpriteRenderer spriteRenderer; // Asigna esto en el Inspector o encuentra el componente en Start/Awake
    public Sprite idleSprite;
    public float movimientoFuerza = 11f;
    public static float positionX;
    public GameObject stone = null ;
    public static Vector3 posicionJug;
    private Animator animator;
    private float movcurr;
    private bool finish = false;
    public Vector2 targetPosition = new Vector2(10, 5); // Punto objetivo
    public float moveSpeed = 5f; // Velocidad de movimiento
    [SerializeField] GameObject meta;

    public GameObject ObjectMenuPause;


    private void Start()
    {
      rb2D = GetComponent<Rigidbody2D>();   
      animator = GetComponent<Animator>();
      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
      if(finish == false) {
        float movementx = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        if(movementx == 0)  { 
          animator.enabled = false;
          spriteRenderer.sprite = idleSprite;
        }
        else {
            transform.localScale = new Vector3(movementx * Mathf.Abs(transform.localScale.x), transform.localScale.y , transform.localScale.z); // Voltear hacia la izquierda
            animator.enabled  = true;
        }
        
        posicionJug = transform.position + new Vector3(movementx , 0f , 0f) * movimientoFuerza * Time.deltaTime;
        if(posicionJug.x > -10.0f) {
          transform.position = posicionJug;
          positionX = posicionJug.x;
        }
        
        if (StressProblems.currentProblemIdx < StressProblems.problems.Length && (posicionJug.x + 13.7f) > stone.transform.position.x) {
          stone.gameObject.SetActive(true);
        }

        if(posicionJug.x >= meta.transform.position.x) {
          finish = true;
          
        }

      }else {
        animator.enabled  = false;
         ObjectMenuPause.SetActive(true);
      }

    } 



}
