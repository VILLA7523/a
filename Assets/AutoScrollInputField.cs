using UnityEngine;
using UnityEngine.UI;

public class AutoScrollInputField : MonoBehaviour
{
    private InputField inputField;
    private ScrollRect scrollRect;

    void Start()
    {
        inputField = GetComponent<InputField>();
        scrollRect = GetComponentInParent<ScrollRect>();

        // Añadir un listener al evento onValueChanged para manejar el desplazamiento
        inputField.onValueChanged.AddListener(delegate { ScrollToBottom(); });
    }

    private void ScrollToBottom()
    {
        // Si el contenido es más alto que el área de visualización, desplázate hacia abajo
        Canvas.ForceUpdateCanvases(); // Actualiza todos los Canvas inmediatamente
        scrollRect.verticalNormalizedPosition = 0f; // Esto desplaza el ScrollRect hacia abajo
    }
}
