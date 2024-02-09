using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWrite : MonoBehaviour
{
    [SerializeField] GameObject stone;
    [SerializeField] GameObject playerFigure;
    [SerializeField] GameObject playerWrite;
    [SerializeField] GameObject star;
    
    Collision collision;
    private bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        playerWrite.SetActive(false);
        star.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFigure.GetComponent<BoxCollider2D>().IsTouching(stone.GetComponent<BoxCollider2D>())) {
            hasCollided = true;
        }

        if (hasCollided && stone.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f && !star.gameObject.activeSelf) {
            playerWrite.SetActive(true);
        } else {
            playerWrite.SetActive(false);
        }
    }

}
