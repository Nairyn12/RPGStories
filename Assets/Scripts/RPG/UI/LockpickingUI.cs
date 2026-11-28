using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.RPG
{
    public class LockpickingUI : MonoBehaviour
    {
        public static LockpickingUI Instance { get; private set; }

        [Header("Text UI")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private TextMeshProUGUI pinText;
        [SerializeField] private Image vibrationWarning; // Красный квадрат опасности

        [Header("Visual Maze UI")]
        [Tooltip("Панель, в которой происходит взлом (серый фон замочной скважины)")]
        [SerializeField] private RectTransform lockArea;
        [Tooltip("Иконка отмычки (кружок или картинка)")]
        [SerializeField] private RectTransform pickIcon;
        [Tooltip("Левая часть стены (до паза)")]
        [SerializeField] private RectTransform wallLeft;
        [Tooltip("Правая часть стены (после паза)")]
        [SerializeField] private RectTransform wallRight;
        [Tooltip("Толщина стены в пикселях")]
        [SerializeField] private float wallThickness = 10f;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            panel.SetActive(false);
            if (LockpickingManager.Instance != null)
            {
                LockpickingManager.Instance.OnStateChanged += UpdateUI;
                LockpickingManager.Instance.OnPinChanged += UpdatePins;
            }
        }

        private void OnDestroy()
        {
            if (LockpickingManager.Instance != null)
            {
                LockpickingManager.Instance.OnStateChanged -= UpdateUI;
                LockpickingManager.Instance.OnPinChanged -= UpdatePins;
            }
        }

        private void Update()
        {
            if (LockpickingManager.Instance == null || !LockpickingManager.Instance.IsActive) return;

            UpdateMazeVisuals();
            UpdateDangerWarning();
        }

        private void UpdateMazeVisuals()
        {
            if (lockArea == null || pickIcon == null) return;

            float areaWidth = lockArea.rect.width;
            float areaHeight = lockArea.rect.height;

            var man = LockpickingManager.Instance;

            // 1. Двигаем отмычку
            // Превращаем угол (-90..90) в координату X (-width/2..width/2)
            float pickX = (man.CurrentAngle / man.MaxAngle) * (areaWidth / 2);
            // Превращаем глубину (0..maxDepth) в координату Y (0..height)
            float pickY = (man.CurrentDepth / man.MaxDepth) * areaHeight;

            // Ставим отмычку по центру (Anchor: middle-center)
            pickIcon.anchoredPosition = new Vector2(pickX, pickY);

            // 2. Двигаем стены (только если мы уже дошли до штифта или вот-вот дойдем)
            if (man.CurrentState == LockpickingManager.LockState.Pushing || man.CurrentState == LockpickingManager.LockState.Vibrating)
            {
                float wallY = (man.TargetDepth / man.MaxDepth) * areaHeight;

                // Левая стена
                float leftEdge = -areaWidth / 2;
                float gapLeft = (man.TargetAngle - man.AngleTolerance) / man.MaxAngle * (areaWidth / 2);
                float leftWidth = gapLeft - leftEdge;

                wallLeft.anchoredPosition = new Vector2(leftEdge + leftWidth / 2, wallY);
                wallLeft.sizeDelta = new Vector2(leftWidth, wallThickness);

                // Правая стена
                float gapRight = (man.TargetAngle + man.AngleTolerance) / man.MaxAngle * (areaWidth / 2);
                float rightEdge = areaWidth / 2;
                float rightWidth = rightEdge - gapRight;

                wallRight.anchoredPosition = new Vector2(gapRight + rightWidth / 2, wallY);
                wallRight.sizeDelta = new Vector2(rightWidth, wallThickness);

                wallLeft.gameObject.SetActive(true);
                wallRight.gameObject.SetActive(true);
            }
            else
            {
                wallLeft.gameObject.SetActive(false);
                wallRight.gameObject.SetActive(false);
            }
        }

        private void UpdateDangerWarning()
        {
            if (vibrationWarning == null) return;

            if (LockpickingManager.Instance.CurrentState == LockpickingManager.LockState.Vibrating)
            {
                vibrationWarning.gameObject.SetActive(true);
                float danger = LockpickingManager.Instance.DangerLevel;
                float blinkSpeed = Mathf.Lerp(3f, 25f, danger);
                float alpha = Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1f);

                if (danger < 0.05f) alpha = 0f;

                Color c = vibrationWarning.color;
                c.a = alpha;
                vibrationWarning.color = c;
            }
            else if (LockpickingManager.Instance.CurrentState == LockpickingManager.LockState.Broken)
            {
                vibrationWarning.gameObject.SetActive(true);
                Color c = vibrationWarning.color;
                c.a = 1f;
                vibrationWarning.color = c;
            }
            else
            {
                vibrationWarning.gameObject.SetActive(false);
            }
        }

        public void Show() => panel.SetActive(true);
        public void Hide() => panel.SetActive(false);

        private void UpdatePins(int current, int total)
        {
            if (pinText != null) pinText.text = $"Штифт: {current + 1} / {total}";
        }

        private void UpdateUI()
        {
            if (LockpickingManager.Instance == null || promptText == null) return;

            switch (LockpickingManager.Instance.CurrentState)
            {
                case LockpickingManager.LockState.Pushing:
                    promptText.text = "Ведите мышь ВВЕРХ";
                    promptText.color = Color.white;
                    break;
                case LockpickingManager.LockState.Vibrating:
                    promptText.text = "Ищите паз, двигая мышь ВЛЕВО/ВПРАВО!";
                    promptText.color = Color.yellow;
                    break;
                case LockpickingManager.LockState.Unlocked:
                    promptText.text = "ЗАМОК ОТКРЫТ!";
                    promptText.color = Color.green;
                    break;
                case LockpickingManager.LockState.Broken:
                    promptText.text = "ОТМЫЧКА СЛОМАЛАСЬ!";
                    promptText.color = Color.red;
                    break;
            }
        }
    }
}