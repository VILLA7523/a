using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;
using System.Linq;
using System.Threading.Tasks;
using static StressProblems;
using TMPro;
using static StressProblems;

public class PlayerWriteSendOpenAI : MonoBehaviour
{
    [SerializeField] Button sendButton;
    [SerializeField] TMPro.TMP_InputField playerWriteInputField;
    [SerializeField] TMPro.TextMeshPro stoneText;
    [SerializeField] GameObject[] stars;
    [SerializeField] GameObject stone = null , meta = null , writer;
    [SerializeField] GameObject feedback;
    
    int idx = 0;
    OpenAIApi openai = new OpenAIApi();
    int badAnswers;
    int maxBadAnswers = 1;
    string forFeedback;
    
    void Start()
    {
        badAnswers = 0;
        feedback.SetActive(false);

        sendButton.GetComponent<Button>().onClick.AddListener(Send);
    }

    async void Send() {
        if (StressProblems.currentProblemIdx >= StressProblems.problems.Length) return;
        Debug.Log("StressProblems.currentProblemIdx: " + StressProblems.currentProblemIdx);

        string problem = StressProblems.problems[StressProblems.currentProblemIdx];
        string solution = playerWriteInputField.text;
        string response = await Ask(problem, solution);

        bool responseIsAccepted;
        var splittedResponse = response.Split("RESUMEN:");
        if (splittedResponse.Length == 2) {
            forFeedback = splittedResponse[1];
            responseIsAccepted = false;
        } else {
            responseIsAccepted = true;
        }

        Debug.Log("problem: " + problem);
        Debug.Log("solution: " + solution);
        Debug.Log("chatGPT response\n: " + response);
        Debug.Log("responseIsAccepted: " + responseIsAccepted);

        if (responseIsAccepted) {
            // star.gameObject.SetActive(true);
            playerWriteInputField.text = "";
            stoneText.text = "";

            StressProblems.currentProblemIdx++;

            stone.gameObject.SetActive(false);
            
            if (StressProblems.currentProblemIdx >= StressProblems.problems.Length) return;
            problem = StressProblems.problems[StressProblems.currentProblemIdx];
            stoneText.text = problem;
            Vector3 newPosition = stone.transform.position;
            newPosition.x += 20.0f; 
            newPosition.y = 8.9f;
            stone.transform.position = newPosition; 
            stars[idx].transform.position = stone.transform.position;
            stars[idx].gameObject.SetActive(true);
            idx++;
            
            newPosition = meta.transform.position;
            newPosition.x += 10.0f;
            writer.gameObject.SetActive(false);
            meta.transform.position = newPosition;
            //star.GetComponentInChildren<TextMeshPro>().text = motivaciones[idx++];
        } else {
            playerWriteInputField.text = "Podrías mejorar tu respuesta...";

            badAnswers++;
            if (badAnswers == maxBadAnswers) {
                feedback.SetActive(true);
                feedback.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = forFeedback;
            }
        }
    }

    async Task<string> Ask (string problem, string solution) {
        string prompt = 
            "Hola, porfa, te voy a hacer tres preguntas, repóndeme con un \"sí\" o un \"no\" la respuesta de cada una. " +
            "La primera pregunta es: ¿la frase \"" + solution + "\" alivia el estrés de la frase \"" + problem + "? " +
            "La segunda pregunta es ¿la frase \"" + solution + "\" representa lo que realmente se quiere decir?. " +
            "La tercera pregunta es ¿la frase \"" + solution + "\" representa algo que sinceramente creemos que se dará en el futuro?. " +
            "Ahora, cambia los \"sí\" por el número 1 y los \"no\" por el número 0. Dime cuánto suman tus respuestas." +
            "Si la suma es menor a 3 porfa dame un resumen de 30 palabras de ¿por qué la frase \"" + solution + "\" no alivia el estrés?," +
                "inicia el resumen con la palabra \"RESUMEN\". " + 
            "Si la suma es 3 no me hagas el resumen porfa.";
        Debug.Log("prompt: " + prompt);

        string response;
        var req = new CreateChatCompletionRequest {
            Model = "gpt-3.5-turbo",
            Messages = new List<ChatMessage>()
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = prompt
                }
            }
        };
        var completionResponse = await openai.CreateChatCompletion(req);
        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0) {
            var message = completionResponse.Choices[0].Message;
            message.Content = message.Content.Trim();
            
         response = message.Content;
        } else {
         response = "Text wasn't generated from this prompt";
        }

        return response;
    }
}
