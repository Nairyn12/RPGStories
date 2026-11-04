using UnityEngine;
using Game.RPG;

public class TestDummy : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount, GameObject source = null)
    {
        Debug.Log($"Тестовый куб получил урон: {amount} от {source.name}");
    }
}