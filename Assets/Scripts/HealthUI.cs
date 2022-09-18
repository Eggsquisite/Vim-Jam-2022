using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private List<Animator> healthAnims;
    private int maxHealth;
    private int currentHealth;
    private string currentState;

    private void Start()
    {
        maxHealth = healthAnims.Count;
        currentHealth = maxHealth;
    }

    public int TakeDamage() {
        currentHealth -= 1;

        if (currentHealth > -1) {
            Debug.Log("Current Health: " + currentHealth);
            healthAnims[currentHealth].Play("hurt_anim");
            //AnimHelper.ChangeAnimationState(healthAnims[currentHealth], ref currentState, "hurt_anim");
            //healthAnims.RemoveAt(currentHealth);
        }

        return currentHealth;
    }

}
