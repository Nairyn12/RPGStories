namespace Game.RPG
{
    public interface IDamageable
    {
        /// <summary>
        /// Наносит урон объекту.
        /// </summary>
        /// <param name="amount">Количество урона.</param>
        /// <param name="source">Кто нанес урон (например, игрок).</param>
        void TakeDamage(float amount, UnityEngine.GameObject source = null);
    }
}