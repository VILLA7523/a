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
        feedback.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(CloseFeedback);
    }

    async void Send() {
        if (StressProblems.currentProblemIdx >= StressProblems.problems.Length) return;
        Debug.Log("StressProblems.currentProblemIdx: " + StressProblems.currentProblemIdx);

        string problem = StressProblems.problems[StressProblems.currentProblemIdx];
        string solution = playerWriteInputField.text;
        
        string[] responses = await Task.WhenAll(
            Ask(problem, solution),
            Ask(problem, solution),
            Ask(problem, solution),
            Ask(problem, solution),
            Ask(problem, solution),
            Ask(problem, solution),
            Ask(problem, solution),
            Ask(problem, solution, true)
        );
    
        var decision = responses.Aggregate(0, (result, response) => {
            if (
                (response[0] == 'N' ||
                response[0] == 'n') &&
                response[1] == 'o' 
            ) {
                return result - 1;
            } else if (
                (response[0] == 'S' ||
                response[0] == 's') &&
                (response[1] == 'i' ||
                response[1] == 'í') 
            ) {
                return result + 1;
            }
			return 0;
		});

        Debug.Log("problem: " + problem);
        Debug.Log("solution: " + solution);
        Debug.Log("decision: " + decision);

        if (decision >= 5) {
            // star.gameObject.SetActive(true);
            playerWriteInputField.text = "";
            problem = StressProblems.problems[StressProblems.currentProblemIdx];
            Vector3 newPosition = stone.transform.position;
            stars[idx].transform.position = newPosition;
            stoneText.text = problem;
            newPosition.x += 20.0f; 
            newPosition.y = 8.9f;
            stone.transform.position = newPosition; 
            stone.gameObject.SetActive(false);
            stars[idx].gameObject.SetActive(true);
            idx++;
            
            newPosition = meta.transform.position;
            newPosition.x += 6.0f;
            meta.transform.position = newPosition;
            writer.gameObject.SetActive(false);
            StressProblems.currentProblemIdx++;
            //star.GetComponentInChildren<TextMeshPro>().text = motivaciones[idx++];
        } else {
            playerWriteInputField.text = "Podrías mejorar tu respuesta...";

            badAnswers++;
            if (badAnswers == maxBadAnswers) {
                badAnswers = 0;
                feedback.SetActive(true);
                feedback.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = responses[7];
            }
        }
    }

    async Task<string> Ask (string problem, string solution, bool feedback = false) {
        var prompt = "Hola, resume en un \"sí\" o un \"no\" si la siguiente frase \"" + solution +
                        "\" " + "alivia el estrés de esta frase \"" + problem + "\".";
        if (feedback) {
            prompt = "Hola, dime en 30 palabras ¿por qué la frase \"" + solution + "\" no alivia el estrés de la frase \"" + problem + "\"?";
        }

        string result;
        var req = new CreateChatCompletionRequest {
            Model = "gpt-3.5-turbo",
            Messages = new List<ChatMessage>()
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = prompt,              
                }
            }
        };
        var completionResponse = await openai.CreateChatCompletion(req);
        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0) {
            var message = completionResponse.Choices[0].Message;
            message.Content = message.Content.Trim();
            
            result = message.Content;
        } else {
            result = "Text wasn't generated from this prompt";
        }

        return result;
    }

    void CloseFeedback() {
        feedback.SetActive(false);
    }
}
