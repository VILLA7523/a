 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;
namespace PiedraEscuchadora {

public class RecordClip : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    [SerializeField] private Button recordButton = null;
    [SerializeField] private Image viñetaPlayer = null;
    [SerializeField] private Image[] target = new Image[4];
    [SerializeField] private Image contador = null;
    
    private string[] negatives = new string[4];
    private OpenAIApi openai = new OpenAIApi();
    public static List<string> stresses = new List<string>();
    private string textTotal = "";
    public float typingSpeed = 0.1f; // Velocidad a la que aparecen las letras
    private TextMeshProUGUI textLabel; // Referencia al componente TextMeshProUGUI
    [SerializeField] private Image viñetaGuia;
    private string story;
    private int idx = 0;
    private int limit = 4;
    private int current = -1;

    [SerializeField] private Button nextButton;


    void Start()
    {
        viñetaPlayer.gameObject.SetActive(false);
        GameObject tmpChild = viñetaGuia.transform.Find("Text (TMP)").gameObject;
        textLabel = tmpChild.GetComponent<TextMeshProUGUI>();
        messageInit();    
        for(int i = 0 ; i < limit ; i++) {
            target[i].gameObject.SetActive(false);
        }
        nextButton.gameObject.SetActive(false);
        // recordButton.interactable = false;
        // recordButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f); 
    }

    public void messageInit() {
        textLabel.text = ""; 
        StartCoroutine(TypeTextSequence());
        Debug.Log("termino");
        // recordButton.interactable = true;
        // recordButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f); 
    }

    IEnumerator TypeTextSequence()
    {
        yield return TypeText("Me estreso");
        yield return new WaitForSeconds(typingSpeed); 
    
        textLabel.text += " <b>cuando</b> ";
        yield return new WaitForSeconds(typingSpeed); 
        
        yield return TypeText("me pisan constantemente,");
        yield return new WaitForSeconds(typingSpeed); 
        
        textLabel.text += " <b>porque</b> ";
        yield return new WaitForSeconds(typingSpeed);

        yield return TypeText("me siento menospreciado. Ahora cuentame tú ¿");
        yield return new WaitForSeconds(typingSpeed);

        textLabel.text += "<b>Cuándo y porqué</b> ";
        yield return new WaitForSeconds(typingSpeed);
        
        yield return TypeText("te estresas?");
        viñetaPlayer.gameObject.SetActive(true);
        contador.gameObject.SetActive(true);

    }

    IEnumerator TypeText(string textToType)
    {
        foreach (char letter in textToType.ToCharArray())
        {
            textLabel.text += letter; 
            yield return new WaitForSeconds(typingSpeed); 
        }
    }

    public void ChangeText(int currentidx) {
        viñetaPlayer.gameObject.SetActive(true);
        tmpInputField.text = target[currentidx].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        current = currentidx;
    }

    public void CreatePostit() {
        if(idx < limit) {
            viñetaPlayer.gameObject.SetActive(true);
        }
    }
    public void DeletePostit()
    {
        viñetaPlayer.gameObject.SetActive(false);
    }

    public void SendPostit()
    {
        bool ok = true;
        if(idx < limit) {
            if(current == -1)  {
                current = idx;
                idx = idx + 1;
                ok = false;
            }
            negatives[current] = tmpInputField.text;
            target[current].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = negatives[current];
            target[current].gameObject.SetActive(true);
            tmpInputField.text = "";
            if(idx == limit) viñetaPlayer.gameObject.SetActive(false);
            if(!ok)  {
                contador.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = idx.ToString();
                current = -1;
            }
        } 

        if(idx >= 2) {
            nextButton.gameObject.SetActive(true);
        }
    }


    public async void Save() 
    {
        for(int i = 0 ; i < limit ; i++) {
            stresses.Add(negatives[i]);
        }
        SceneManager.LoadScene("Loading");
    }

   
}
}