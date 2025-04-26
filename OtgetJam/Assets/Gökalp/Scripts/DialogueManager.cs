using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialogueData dialogueData;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Image dialogueImage;
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

        if (currentLine.optionalImage != null)
        {
            dialogueImage.gameObject.SetActive(true);
            dialogueImage.sprite = currentLine.optionalImage;
        }
        else
        {
            dialogueImage.gameObject.SetActive(false);
        }

        previousButton.gameObject.SetActive(currentLineIndex > 0);
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
