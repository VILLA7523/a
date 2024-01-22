using OpenAI;
using UnityEngine;
using UnityEngine.UI;
using System; 
using System.Collections.Generic; 

namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private Text message;
        [SerializeField] private Text result; //--
        [SerializeField] private Dropdown dropdown;
        
        private readonly string fileName = "output.wav";
        private readonly int duration = 5;
        
        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi();

        private void Start()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
            #else
            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }
            recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);
            
            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);
            #endif
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        private void StartRecording()
        {
            isRecording = true;
            recordButton.enabled = false;

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            
            #if !UNITY_WEBGL
            clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
            #endif
        }

        private async void EndRecording()
        {
            message.text = "Transcripting...";
            
            #if !UNITY_WEBGL
            Microphone.End(null);
            #endif
            
            byte[] data = SaveWav.Save(fileName, clip);
            
            // var req = new CreateAudioTranscriptionsRequest
            // {
            //     FileData = new FileData() {Data = data, Name = "audio.wav"},
            //     // File = Application.persistentDataPath + "/" + fileName,
            //     Model = "whisper-1",
            //     Language = "es"
            // };
            // var res = await openai.CreateAudioTranscription(req);

            progressBar.fillAmount = 0;
            // message.text = res.Text;

            //--
            string problem = "me estresa tener que dar el examen tan rápido porque no estudié";
            // string solution = "me levantaré temprano para estudiar unas horitas más";//message.text;
            string solution = "organizare el poco tiempo que me queda para estudiar y dare mi examen con lo que aprenda sabiendo que mas adelante me organizare mejor y dare un mejor examen";//message.text;
            string pool = "me estresa tener que dar el examen tan rápido porque no estudié. "+
                            "En la vida universitaria, el estrés es una compañía constante que se manifiesta de diversas maneras, a menudo desafiando la salud mental y emocional de los estudiantes. La carga académica, con su secuencia interminable de tareas, exámenes y proyectos, es uno de los principales generadores de ansiedad. La presión por rendir bien, mantener un promedio alto y enfrentar evaluaciones constantes puede ser abrumadora. La dimensión financiera también juega un papel crucial. Las matrículas elevadas, los gastos en libros y materiales, y la necesidad de cubrir los costos de vida, a menudo llevan a la inseguridad económica. La búsqueda de trabajos a tiempo parcial para sufragar gastos adicionales contribuye a la carga, ya que los estudiantes luchan por equilibrar las demandas laborales y académicas. La adaptación social en un entorno universitario puede convertirse en un desafío significativo. La presión por hacer conexiones, integrarse en grupos sociales y gestionar relaciones interpersonales se suma al estrés. La sensación de competencia y la necesidad de encajar pueden generar ansiedad social, especialmente en un contexto donde la comparación con los demás es común. Las expectativas familiares, muchas veces ligadas a la inversión financiera realizada en la educación universitaria, pueden generar una presión adicional. La necesidad de cumplir con las expectativas familiares y justificar el costo de la educación se convierte en un peso en los hombros de los estudiantes. La gestión del tiempo se presenta como un reto constante. La necesidad de equilibrar clases, estudios, trabajo y tiempo libre puede llevar a una sensación de agotamiento. La incapacidad para administrar eficientemente el tiempo puede contribuir al estrés y afectar negativamente la calidad de vida. El futuro incierto después de la universidad también añade preocupación. La toma de decisiones relacionadas con la carrera, la búsqueda de empleo y la transición a la vida laboral pueden generar inseguridad y ansiedad sobre el camino por venir. Las experiencias personales, como problemas familiares, desafíos de salud o dificultades en relaciones personales, pueden intensificar el estrés. Las preocupaciones personales se entrelazan con las demandas académicas y sociales, creando una carga emocional significativa. En resumen, la vida universitaria, aunque llena de oportunidades y crecimiento, no está exenta de tensiones. Los estudiantes enfrentan diariamente una amalgama de desafíos que incluyen la presión académica, las preocupaciones financieras, la adaptación social y la incertidumbre sobre el futuro. La gestión eficaz del estrés y el acceso a recursos de apoyo son cruciales para ayudar a los estudiantes a sobrellevar estas tensiones y prosperar durante su tiempo en la universidad.";
            var req = new CreateChatCompletionRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Role = "user",
                        Content = "divide el texto " + pool + " en diez oraciones de trece palabras"
                        // Content = "dime si la frase " + solution + " es negativa solo diciendome una de las siguietnes respuestas unicas \"sí\", \"no\" o \"tal vez\"",
                        // Content = "dime con una respuesta unica de \"sí\" o \"no\" si la frase siguiente \"" + solution +
                        //     "\" " + "alivia el estrés de esta otra frase \"" + problem + "\"",
                        
                        // qué tan positiva para aliviar el estrés de la frase " + problem +
                        //             " es la frase " + solution,                
                        }
                }
            };
            var completionResponse = await openai.CreateChatCompletion(req);
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                result.text = message.Content;
            } else {
                result.text = "No text was generated from this prompt.";
            }
            //--

            recordButton.enabled = true;
        }

        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progressBar.fillAmount = time / duration;
                
                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    EndRecording();
                }
            }
        }
    }
}
