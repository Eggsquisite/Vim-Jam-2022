using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;

    private int index = 0;
    private bool canType = true;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
    }

    public void StartDialogue() {
        if (textComponent.text == lines[index]) {
            NextLine();
        }
        else if (textComponent.text != lines[index]) {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
        else if (index == 0) {
            index++;
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine() { 
        // Type each charater 1 by 1
        foreach(char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() { 
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            index = 0;
            canType = true;
            gameObject.SetActive(false);
        }
    }

    public void StopDialogue() {
        index = 0;
        canType = true;
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }
}
