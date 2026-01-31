using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager Singleton;

    [SerializeField]
    public int MaxHealth;
    private int currentHealth;

    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public Vector2 MajorSpawnPoint;

    [SerializeField]
    public Vector2 MinorSpawnPoint;

    public void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        currentHealth = MaxHealth;
    }

    public void OutboundDamage()
    {
        var hasDied = Damage();
        if (hasDied) return;

        SoftSpawn();
    }

    // TODO: Invincibility time
    public bool Damage(int damageAmount = 1)
    {
        if (currentHealth > damageAmount)
            currentHealth -= damageAmount;
        else
            currentHealth = 0;

        if (currentHealth == 0)
        {
            Die();
        }

        return currentHealth == 0;
    }

    // public void Heal(int amount = 1)
    // {
    //     if (currentHealth + amount <= MaxHealth)
    //     {
    //         currentHealth -= amount;
    //     }
    //     else
    //     {
    //         currentHealth = MaxHealth;
    //     }
    // }

    public void Die()
    {
        // ShowDeathDialog()
        Spawn();
    }

    public void SoftSpawn()
    {
        Player.transform.SetPositionAndRotation(MinorSpawnPoint, Player.transform.rotation);
    }

    public void Spawn()
    {
        currentHealth = MaxHealth;
        Player.transform.SetPositionAndRotation(MajorSpawnPoint, Player.transform.rotation);
    }

    public void UpdateMajorSpawnPoint(Vector2 position) { MajorSpawnPoint = position; }
    public void UpdateMinorSpawnPoint(Vector2 position) { MinorSpawnPoint = position; }
}
