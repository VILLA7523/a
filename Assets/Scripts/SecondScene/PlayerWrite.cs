using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWrite : MonoBehaviour
{
    [SerializeField] GameObject stone;
    [SerializeField] GameObject playerFigure;
    [SerializeField] GameObject playerWrite;
    
    Collision collision;
    private bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        playerWrite.SetActive(false);
    }

    // Update lled once per frame
    void Update()
    {
        if (playerFigure.GetComponent<BoxCollider2D>().IsTouching(stone.GetComponent<BoxCollider2D>())) {
            hasCollided = true;
        }else {
            hasCollided = false;
        }
        if (hasCollided) {
            playerWrite.SetActive(true);
        } else {
            playerWrite.SetActive(false);
        }
    }

    public void changescene(string nameMenu) {
        SceneManager.LoadScene(nameMenu);
    }
    

}
