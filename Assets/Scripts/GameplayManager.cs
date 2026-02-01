using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Singleton;

    [SerializeField]
    public int MaxHealth;
    private int currentHealth;

    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public GameObject Water;
    [SerializeField]
    public Vector2 WaterRespawnOffset;

    [SerializeField]
    public Vector2 MajorSpawnPoint;

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

    public void Die()
    {
        // ShowDeathDialog()
        Spawn();
    }

    public void Spawn()
    {
        currentHealth = MaxHealth;
        Player.transform.SetPositionAndRotation(MajorSpawnPoint, Player.transform.rotation);
        if (Water != null) setWaterLevel();
    }

    public void UpdateMajorSpawnPoint(Vector2 position)
    {
        MajorSpawnPoint = position;
    }

    private void setWaterLevel()
    {
        Water.transform.SetPositionAndRotation(MajorSpawnPoint + WaterRespawnOffset, Water.transform.rotation);
    }
}
