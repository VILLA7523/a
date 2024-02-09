using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace PiedraEscuchadora {
    public class Record : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Sprite recordButtonOn;
        [SerializeField] private Sprite recordButtonOff;
        [SerializeField] private Sprite recordButtonProcessing;
        [SerializeField] private TMPro.TextMeshProUGUI transcription;
        [SerializeField] private Image comicPanel;
        private readonly string microphoneName = "Microphone (Realtek(R) Audio)";
        private readonly float recordingMinutes = 2.5f;
        private AudioClip clip;
        private OpenAIApi openai = new OpenAIApi();
        private bool recording = false;
        public static List<string> stresses = new List<string>();

        void Start()
        {
            foreach (var device in Microphone.devices) {
                Debug.Log(device);
            }
            recordButton.onClick.AddListener(StartRecording);
        }

        private void StartRecording() {
            if (!recording) {
                clip = Microphone.Start(microphoneName, false, (int)(recordingMinutes*60), 44100);
                
                recordButton.GetComponent<Image>().sprite = recordButtonOn;
                recordButton.onClick.AddListener(StopRecording);
                recording = true;
            }
        }

        private async void StopRecording() {
            if (recording) {
                recordButton.GetComponent<Image>().sprite = recordButtonProcessing;
                Microphone.End(microphoneName);

                byte[] data = SaveWav.Save("output.wav", clip);
                var transcriptionText = (await openai.CreateAudioTranscription(
                    new CreateAudioTranscriptionsRequest {
                        FileData = new FileData() {Data = data, Name = "audio.wav"},
                        Model = "whisper-1",
                        Language = "es",
                    }
                )).Text;

                transcription.text = "<mark=#000000DE>" + transcriptionText + "</mark>";

                var chatCompletion = await openai.CreateChatCompletion(
                    new CreateChatCompletionRequest {
                        Model = "gpt-3.5-turbo",
                        Messages = new List<ChatMessage>() {
                            new ChatMessage() {
                                Role = "user",
                                Content = "divide el texto " + transcriptionText + " en diez oraciones de trece palabras",  
                            }
                        },
                    }
                );
                if (chatCompletion.Choices != null && chatCompletion.Choices.Count > 0) {
                    var message = chatCompletion.Choices[0].Message;
                    message.Content = message.Content.Trim();                

                    string stressesRaw = message.Content;
                    stressesRaw = stressesRaw.Replace("@", System.Environment.NewLine);
                    var pattern = @"\d\. (.*)(\n|$)";
                    var matches = Regex.Matches(stressesRaw, pattern);
                    foreach(Match match in matches) {
                        stresses.Add(match.Groups[1].Value);
                    }
                    string stressesLog = string.Join(", ", stresses);
                    Debug.Log("Pensamientos negativos sobre el estres: " + stressesLog);
                } else {
                    Debug.Log("No text was generated from this prompt.");
                }

                recordButton.GetComponent<Image>().sprite = recordButtonOff;
                recordButton.onClick.AddListener(StartRecording);
                recording = false;
            } 
        }
    }
}

