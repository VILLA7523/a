 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;


namespace PiedraEscuchadora {

public class RecordClip : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    [SerializeField] private Button recordButton = null;
    [SerializeField] private Image viñetaPlayer = null;
    [SerializeField] private Button[] target;
    private string[] negatives;
    private OpenAIApi openai = new OpenAIApi();
    public static List<string> stresses = new List<string>();
    private string textTotal = "";

    // type text
    public float typingSpeed = 0.1f; // Velocidad a la que aparecen las letras
    private TextMeshProUGUI textLabel; // Referencia al componente TextMeshProUGUI
    [SerializeField] private Image viñetaGuia;
    private string story;
    private int idx = 0;

    void Start()
    {
        viñetaPlayer.gameObject.SetActive(false);
        GameObject tmpChild = viñetaGuia.transform.Find("Text (TMP)").gameObject;
        textLabel = tmpChild.GetComponent<TextMeshProUGUI>();
        messageInit();    
    }

    public void messageInit() {
        textLabel.text = ""; // Comienza con el texto vacío
        StartCoroutine(TypeTextSequence());
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

        yield return TypeText("me siento menospreciado");
        yield return new WaitForSeconds(typingSpeed);

         textLabel.text += " Ahora cuentame tú ¿<b>Cuándo</b> y <b>porqué</b> te estresas?";

    }
    IEnumerator TypeText(string textToType)
    {
        foreach (char letter in textToType.ToCharArray())
        {
            textLabel.text += letter; 
            yield return new WaitForSeconds(typingSpeed); 
        }
    }

    public void CreatePostit() {
        viñetaPlayer.gameObject.SetActive(true);
    }
    
    public void DeletePostit()
    {
        viñetaPlayer.gameObject.SetActive(false);
    }
    
    public void SendPostit()
    {
        if(idx < 4) {
            negatives[idx] = tmpInputField.text;
            TextMeshProUGUI tmpText = target[idx].GetComponentInChildren<TextMeshProUGUI>();
            tmpText.text = negatives[idx];
            tmpInputField.text = "";
            viñetaPlayer.gameObject.SetActive(false);
            idx = idx + 1;
        }
    }

    public void Stop()
    {  
        //ans = await SendRecording();
    }

    // public void Save() 
    // {
    //     textTotal += tmpInputField.text;
    //     Debug.Log(textTotal);
    // }

    // private async Task<bool> SendIA() {
    //     // var chatCompletion = await openai.CreateChatCompletion(
    //     //     new CreateChatCompletionRequest {
    //     //     Model = "gpt-3.5-turbo",
    //     //     Messages = new List<ChatMessage>() {
    //     //         new ChatMessage() {
    //     //             Role = "user",
    //     //             Content = "divide el texto " + transcriptionText + " en 2 oraciones de trece palabras",  
    //     //         }
    //     //     },});
                
    //     //   if (chatCompletion.Choices != null && chatCompletion.Choices.Count > 0) {
    //     //     var message = chatCompletion.Choices[0].Message;
    //     //     message.Content = message.Content.Trim();                
    //     //     string stressesRaw = message.Content;
    //     //     stressesRaw = stressesRaw.Replace("@", System.Environment.NewLine);
    //     //     var pattern = @"\d\. (.*)(\n|$)";
    //     //     var matches = Regex.Matches(stressesRaw, pattern);
    //     //     foreach(Match match in matches) {
    //     //       stresses.Add(match.Groups[1].Value);
    //     //     }
    //     //     string stressesLog = string.Join(", ", stresses);
    //     //     Debug.Log("Pensamientos negativos sobre el estres: " + stressesLog);
    //     //     return true;
    //     //   } else {
    //     //     Debug.Log("No text was generated from this prompt.");
    //     //     return false;
    //     //   }
    // }
}



}