using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordByWordTextDisplay : MonoBehaviour
{
    // private TextMeshProUGUI textMeshPro;
    // public Image comicPanel;
    // public Image stone;
    // public Image micro;
    // private string[] dialogues = {
    //     "¡Hola! Soy Stone, tu amigo rocoso con un corazón blando. Estoy aquí para escuchar y ayudar. <b>¿Listo para desahogarte?</b>",
    // };
    // private int currentDialogueIndex = 0;

    // private void Start()
    // {
    //     GameObject tmpChild = comicPanel.transform.Find("Text (TMP)").gameObject;
    //     textMeshPro = tmpChild.GetComponent<TextMeshProUGUI>();
    //     comicPanel.gameObject.SetActive(true);
    //     textMeshPro.gameObject.SetActive(true);
    //     micro.gameObject.SetActive(false);
    //     ShowDialogue(); // Mostrar el primer mensaje.
    // }

    // private void Update()
    // {
    //     Vector3 mousePos = Input.mousePosition;
    //     // Debug.Log(mousePos);

    //     if (Input.GetKeyDown(KeyCode.Return))
    //     {
    //         ShowNextDialogue();
    //         Debug.Log(currentDialogueIndex);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Backspace))
    //     {
    //         ShowPreviousDialogue();
    //     }
    // }

    // private void ShowNextDialogue()
    // {
    //     if (currentDialogueIndex < dialogues.Length - 1) // Asegura no sobrepasar el último diálogo.
    //     {
    //         currentDialogueIndex++;
    //         ShowDialogue();
    //     }else {
    //         comicPanel.gameObject.SetActive(false);
    //         stone.transform.position = new Vector3(400f, stone.transform.position.y , stone.transform.position.z);
    //         Debug.Log(stone.transform.position);
    //         micro.gameObject.SetActive(true);
    //     }
    // }

    // private void ShowPreviousDialogue()
    // {
    //     if (currentDialogueIndex > 0) // Asegura no ir más atrás del primer diálogo.
    //     {
    //         currentDialogueIndex--;
    //         ShowDialogue();
    //     }
    // }

    // private void ShowDialogue()
    // {
    //     textMeshPro.text = dialogues[currentDialogueIndex];
    // }
}