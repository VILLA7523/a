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
    [SerializeField] private TMPro.TMP_InputField playerWriteInputField;
    [SerializeField] private TMPro.TextMeshPro stoneText;
    [SerializeField] GameObject[] stars;

    [SerializeField] GameObject stone = null , meta = null , writer;
    private int idx = 0;
    private OpenAIApi openai = new OpenAIApi();

    
    void Start()
    {
        sendButton.GetComponent<Button>().onClick.AddListener(Send);
    }

    async void Send() {
        if (StressProblems.currentProblemIdx >= StressProblems.problems.Length) return;
        string problem = StressProblems.problems[StressProblems.currentProblemIdx];

        Debug.Log("StressProblems.currentProblemIdx: " + StressProblems.currentProblemIdx);

        string solution = playerWriteInputField.text;
    
        // string[] responses = await Task.WhenAll(
        //     Enumerable.Repeat(
        //         Ask(problem, solution),
        //         7
        //     ).ToArray()
        // );
        // var decision = responses.Aggregate(0, (result, response) => {
        //     if (
        //         (response[0] == 'N' ||
        //         response[0] == 'n') &&
        //         response[1] == 'o' 
        //     ) {
        //        return result - 1;
        //     } else if (
        //         (response[0] == 'S' ||
        //         response[0] == 's') &&
        //         (response[1] == 'i' ||
        //         response[1] == 'í') 
        //     ) {
        //        return result + 1;
        //     }
		// 	return 0;
		// });
        var decision = 7;
        if (decision >= 5) {
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
        }
        Debug.Log("Decision (if >0 then yes): " + decision);

    }

    async Task<string> Ask (string problem, string solution) {
        string result;
        var req = new CreateChatCompletionRequest {
            Model = "gpt-3.5-turbo",
            Messages = new List<ChatMessage>()
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = 
                        "Hola, resume en un \"sí\" o un \"no\" si la siguiente frase \"" + solution +
                        "\" " + "alivia el estrés de esta frase \"" + problem + "\".",              
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
}
