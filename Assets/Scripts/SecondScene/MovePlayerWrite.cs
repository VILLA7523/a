using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovePlayer;

public class MovePlayerWrite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector2(
            positionX*100+980,
            this.gameObject.transform.position.y
        );
    }
}
