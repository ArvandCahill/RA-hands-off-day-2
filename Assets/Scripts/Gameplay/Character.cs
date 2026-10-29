
using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;

    private int currentHealth;

    public CharacterData Data => characterData;
    public int CurrentHealth => currentHealth;
    public bool IsDead { get; private set; }

    protected virtual void Awake()
    {
        if (characterData == null)
        {
            Debug.LogError(
                $"{name}: CharacterData is not assigned.",
                this
            );

            enabled = false;
            return;
        }

        currentHealth = Mathf.Max(0, characterData._health);
        IsDead = currentHealth <= 0;
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth == 0)
            Die();
    }

    protected virtual void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
    }

    public virtual void Heal(int amount)
    {
        if (IsDead || amount <= 0)
            return; 

        currentHealth = Mathf.Min(
            characterData._health,
            currentHealth + amount
        );
    }
}
