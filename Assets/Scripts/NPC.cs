using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Dialogue wonDialogue;

    private bool gameWon;
    private SpriteRenderer sp;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        interactPrompt.SetActive(false);
        sp = GetComponent<SpriteRenderer>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();
        CheckForPlayer();
    }

    private void FacePlayer() { 
        if (player.transform.position.x > transform.position.x) {
            sp.flipX = false;   
        }
        else if (player.transform.position.x < transform.position.x) {
            sp.flipX = true;
        }
    }

    private void CheckForPlayer() {
        RaycastHit2D rightCheck = Physics2D.Raycast(transform.position, Vector2.right, 1f, playerLayer);
        RaycastHit2D leftCheck = Physics2D.Raycast(transform.position, Vector2.left, 1f, playerLayer);

        if (rightCheck.collider != null && (rightCheck.collider.tag == "Player" || rightCheck.collider.tag == "Fire Head")) {
            interactPrompt.SetActive(true);
            if (!gameWon)
                playerController.GetDialogue(dialogue);
            else
                playerController.GetDialogue(wonDialogue);
        }
        else if (leftCheck.collider != null && (leftCheck.collider.tag == "Player" || leftCheck.collider.tag == "Fire Head")) {
            interactPrompt.SetActive(true);
            if (!gameWon)
                playerController.GetDialogue(dialogue);
            else
                playerController.GetDialogue(wonDialogue);
        }
        else {
            interactPrompt.SetActive(false);
            playerController.GetDialogue(null);
            if (!gameWon)
                dialogue.StopDialogue();
            else
                wonDialogue.StopDialogue();
        }
    }

    // EVENT FROM TORCH.CS
    public void GameWon() {
        gameWon = true;
    }
}
