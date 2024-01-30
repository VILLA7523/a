using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiedraEscuchadora {
    public class StressesReceiver : MonoBehaviour
    {
        void Update()
        {
            foreach(string stress in Record.stresses) {
                Debug.Log(stress);
            }
        }
    }
}
