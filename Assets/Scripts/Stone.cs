using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PiedraEscuchadora {
    public class Stone : MonoBehaviour
    {
        [SerializeField] GameObject stonePrefab , positivePrefab;
        int iter = 0;
        GameObject gameObject , gameObjectSquare ,player;
        [SerializeField] float minTras;
        [SerializeField] float maxTras; 
        bool newObject = true;

        string[] frases = {"Te sientes estresado por los examenes" , "Tienes miedo a desaprobar" , "Tienes poco tiempo para realizar tus proyectos" , "Aveces te sientes sola y que nadie te quiere"};
        string[] frasesPositivas = {"Cada obstaculo me hace mejor persona" , "Si me esfuerzon no desaprobare" , "Tienes tiempo para realizar tus proyectos" , "No estoy sola y hay gente que me ama"};
    
        void Start()
        {
            player = GameObject.Find("Player");
            StartCoroutine(StoneSpawn());
            Debug.Log("Position " + player.transform.position);
        }

        IEnumerator StoneSpawn() 
        {
            while (iter < frases.Length)
            {
                if (newObject)
                {
                    var wanted = Random.Range(minTras, maxTras);
                    var position = new Vector3(player.transform.position.x + 10.0f, transform.position.y);
                    TextMeshPro textMesh = stonePrefab.GetComponentInChildren<TextMeshPro>();
                    TextMeshPro textMeshPositive = positivePrefab.GetComponentInChildren<TextMeshPro>();
                    textMesh.text = frases[iter];
                    textMeshPositive.text = frasesPositivas[iter];
                    gameObject = Instantiate(stonePrefab , position, Quaternion.identity);
                    gameObjectSquare = Instantiate(positivePrefab , new Vector3(-6.0f , 3f , position.z), Quaternion.identity);
                    newObject = false;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (gameObject != null) Destroy(gameObject);
                    if (gameObjectSquare != null) Destroy(gameObjectSquare);
                    newObject = true;
                    iter++;
                }

                yield return null;
            }
        }


    }
}
