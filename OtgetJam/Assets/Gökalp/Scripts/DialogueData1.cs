using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public DialogueData dialogueData;
    public Text speakerText;
    public Text dialogueText;
    public Button nextButton;
    public Button previousButton;

    private int currentLineIndex = 0;

    private void Start()
    {
        UpdateDialogue();
        previousButton.onClick.AddListener(PreviousLine);
        nextButton.onClick.AddListener(NextLine);
    }

    private void UpdateDialogue()
    {
        DialogueData.DialogueLine currentLine = dialogueData.lines[currentLineIndex];
        speakerText.text = currentLine.speakerName;
        dialogueText.text = currentLine.text;

        previousButton.gameObject.SetActive(currentLineIndex > 0);
        nextButton.GetComponentInChildren<Text>().text = (currentLineIndex == dialogueData.lines.Length - 1) ? "Devam Et" : "Ýleri";
    }

    private void NextLine()
    {
        if (currentLineIndex < dialogueData.lines.Length - 1)
        {
            currentLineIndex++;
            UpdateDialogue();
        }
        else
        {
            // Son satýra geldi, sahne deðiþtir
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void PreviousLine()
    {
        if (currentLineIndex > 0)
        {
            currentLineIndex--;
            UpdateDialogue();
        }
    }
}
