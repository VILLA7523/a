using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PiedraEscuchadora {

public class PostItManager : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    public Slider characterCountSlider;
    public int maxCharacterCount = 300;
    public Image fullIcon;
    public Image backIcon;
    public Color full;
    private Color fullFillColor , backFillColor; // Color cuando el Slider está lleno


    void Start()
    {
        tmpInputField.onValueChanged.AddListener(OnTextInput);
        characterCountSlider.maxValue = maxCharacterCount;
        characterCountSlider.value = 0;
        backFillColor = backIcon.color;
        fullFillColor = fullIcon.color;
    }

    private void OnTextInput(string text)
    {
        characterCountSlider.value = Mathf.Min(text.Length , maxCharacterCount);
        
        if (text.Length > maxCharacterCount)
        {
            tmpInputField.text = text.Substring(0, maxCharacterCount);
            fullIcon.color = Color.red;
            backIcon.color = Color.red;
        }else {
            fullIcon.color = fullFillColor;
            backIcon.color = backFillColor;
        }
    }
}

}