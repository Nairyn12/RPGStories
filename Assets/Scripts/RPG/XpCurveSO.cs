using UnityEngine;

namespace Game.RPG
{
    /// <summary>
    /// Кривая опыта. X = уровень, Y = сколько XP нужно, чтобы с этого уровня
    /// перейти на следующий. Multiplier — глобальный множитель поверх кривой.
    /// Кривая редактируется прямо в инспекторе (AnimationCurve).
    /// </summary>
    [CreateAssetMenu(fileName = "XpCurve_", menuName = "RPG/XP Curve")]
    public class XpCurveSO : ScriptableObject
    {
        [Tooltip("X = уровень, Y = XP для перехода на следующий уровень. " +
                 "Дефолт аппроксимирует 100 × level^1.5.")]
        public AnimationCurve Curve = AnimationCurve.Constant(1, 20, 100);

        [Tooltip("Глобальный множитель поверх кривой. Для тюнинга «громкости» XP.")]
        [Range(0.1f, 10f)]
        public float Multiplier = 1f;

        [Range(2, 50)]
        public int MaxLevel = 20;

        public int GetXpForLevel(int level)
        {
            if (Curve == null || Curve.length == 0) return 100;
            float raw = Curve.Evaluate(level) * Multiplier;
            return Mathf.Max(1, Mathf.RoundToInt(raw));
        }

        private void OnEnable()
        {
            // На случай, если ассет создан без Reset (например, скопирован)
            if (Curve == null || Curve.length == 0)
                ResetToDefault();
        }

        [ContextMenu("Reset to default curve")]
        private void ResetToDefault()
        {
            Curve = new AnimationCurve();
            for (int i = 1; i <= 20; i++)
                Curve.AddKey(i, Mathf.Pow(i, 1.5f) * 100f);   // 10 = 100 / 10^1.5 * multiplier

            for (int i = 0; i < Curve.length; i++)
                Curve.SmoothTangents(i, 0f);

            Multiplier = 1f;
            MaxLevel = 20;
        }

        private void Reset()
        {
            ResetToDefault();
        }
    }
}