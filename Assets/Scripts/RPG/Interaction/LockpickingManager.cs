using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.RPG
{
    public class LockpickingManager : MonoBehaviour
    {
        public static LockpickingManager Instance { get; private set; }

        public enum LockState { Idle, Pushing, Vibrating, Unlocked, Broken }

        public LockState CurrentState { get; private set; } = LockState.Idle;
        public bool IsActive => CurrentState != LockState.Idle;

        // 🛠 ПУБЛИЧНЫЕ СВОЙСТВА ДЛЯ UI
        public float CurrentDepth => _currentDepth;
        public float CurrentAngle => _currentAngle;
        public float TargetDepth => _targetDepth;
        public float TargetAngle => _targetAngle;
        public float MaxDepth => maxDepth;
        public float MaxAngle => maxAngle;
        public float AngleTolerance => _angleTolerance;
        public float DangerLevel { get; private set; } = 0f;

        [Header("Core Settings")]
        [SerializeField] private float depthMultiplier = 1f;
        [SerializeField] private float turnMultiplier = 1.5f;

        [Header("Difficulty & Tolerance")]
        [SerializeField] private float maxAngle = 90f;
        [Tooltip("Увеличенная максимальная глубина (для лабиринта)")]
        [SerializeField] private float maxDepth = 150f;

        [Header("Speed Thresholds (Break if exceeded)")]
        [SerializeField] private float breakForwardSpeed = 25f;
        [SerializeField] private float breakSidewaysSpeed = 20f;

        [Header("Skill Check")]
        [SerializeField] private bool requireSkill = false;

        [Header("Failure Delay")]
        [SerializeField] private float failureMessageDelay = 2f;

        [Header("UI Panels To Hide")]
        [SerializeField] private GameObject playerHUD;
        [SerializeField] private GameObject hotbarPanel;

        private int _totalPins;
        private int _currentPin;

        private float _currentDepth;
        private float _targetDepth;
        private float _currentAngle;
        private float _targetAngle;
        private float _angleTolerance = 5f;
        private float _pinPressure = 0f;
        private bool _isEnding = false;

        private Action<bool> _onComplete;
        private PlayerController _playerController;

        public event Action OnStateChanged;
        public event Action<int, int> OnPinChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void StartLockpick(int lockDifficulty, PlayerStats stats, Action<bool> onComplete)
        {
            _totalPins = Mathf.Clamp(lockDifficulty, 2, 8);
            _currentPin = 0;
            _onComplete = onComplete;
            _isEnding = false;
            DangerLevel = 0f;

            float skill = stats != null ? stats.GetStat(StatType.Stealth) : 0;
            _angleTolerance = Mathf.Lerp(3f, 8f, Mathf.Clamp01(skill / 10f));
            breakSidewaysSpeed = Mathf.Lerp(10f, 25f, Mathf.Clamp01(skill / 10f));

            if (requireSkill && skill < _totalPins)
            {
                onComplete?.Invoke(false);
                ResetToGame();
                return;
            }

            _playerController = FindFirstObjectByType<PlayerController>();
            if (_playerController != null) _playerController.enabled = false;

            StartNextPin();

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (LockpickingUI.Instance != null) LockpickingUI.Instance.Show();
            if (playerHUD != null) playerHUD.SetActive(false);
            if (hotbarPanel != null) hotbarPanel.SetActive(false);

            OnPinChanged?.Invoke(_currentPin, _totalPins);
            OnStateChanged?.Invoke();
        }

        private void StartNextPin()
        {
            _currentDepth = 0f;
            _currentAngle = 0f;
            _pinPressure = 0f;
            DangerLevel = 0f;

            // Генерируем стену и дырку в ней
            _targetDepth = UnityEngine.Random.Range(40f, maxDepth);
            _targetAngle = UnityEngine.Random.Range(-maxAngle + 20f, maxAngle - 20f); // Дырка не у самых краев

            CurrentState = LockState.Pushing;
            OnStateChanged?.Invoke();
        }

        private void Update()
        {
            if (!IsActive || CurrentState == LockState.Unlocked || CurrentState == LockState.Broken) return;
            if (Mouse.current == null) return;

            Vector2 delta = Mouse.current.delta.ReadValue();

            switch (CurrentState)
            {
                case LockState.Pushing: HandlePushing(delta); break;
                case LockState.Vibrating: HandleAtPin(delta); break;
            }
        }

        private void HandlePushing(Vector2 delta)
        {
            if (delta.y > breakForwardSpeed) { BreakPick("Слишком сильный рывок вперёд!"); return; }

            _currentDepth += delta.y * depthMultiplier;
            if (_currentDepth < 0) _currentDepth = 0;

            // Врезались в штифт
            if (_currentDepth >= _targetDepth)
            {
                if (_currentDepth > _targetDepth + 20f)
                {
                    BreakPick("Проскочили штифт!");
                    return;
                }

                _currentDepth = _targetDepth;
                CurrentState = LockState.Vibrating;
                OnStateChanged?.Invoke();
            }
        }

        private void HandleAtPin(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > breakSidewaysSpeed) { BreakPick("Слишком резко крутанули!"); return; }

            // Накопление давления
            if (delta.y > 1f) _pinPressure += delta.y * Time.unscaledDeltaTime * 4f;
            else _pinPressure -= Time.unscaledDeltaTime * 15f;
            _pinPressure = Mathf.Max(0f, _pinPressure);

            _currentAngle += delta.x * turnMultiplier;

            float angleDanger = Mathf.Abs(_currentAngle) / maxAngle;
            float pressureDanger = _pinPressure / 20f;
            DangerLevel = Mathf.Clamp01(Mathf.Max(angleDanger, pressureDanger));

            OnStateChanged?.Invoke();

            if (DangerLevel >= 1f)
            {
                BreakPick("Вы приложили слишком много усилий!");
                return;
            }

            // Проверка на попадание в дырку (паз)
            if (Mathf.Abs(_currentAngle - _targetAngle) <= _angleTolerance)
            {
                _currentPin++;

                if (_currentPin >= _totalPins)
                {
                    CurrentState = LockState.Unlocked;
                    OnStateChanged?.Invoke();
                    EndLockpick(true);
                }
                else
                {
                    OnPinChanged?.Invoke(_currentPin, _totalPins);
                    StartNextPin();
                }
            }
        }

        private void BreakPick(string reason)
        {
            if (_isEnding) return;
            _isEnding = true;

            Debug.Log($"[Lockpick] Отмычка сломалась: {reason}");
            CurrentState = LockState.Broken;
            DangerLevel = 1f;
            OnStateChanged?.Invoke();

            StartCoroutine(EndLockpickDelayed(false, failureMessageDelay));
        }

        private IEnumerator EndLockpickDelayed(bool success, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            EndLockpick(success);
        }

        private void EndLockpick(bool success)
        {
            ResetToGame();
            _onComplete?.Invoke(success);
            CurrentState = LockState.Idle;
            _isEnding = false;
        }

        private void ResetToGame()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (_playerController != null) _playerController.enabled = true;
            if (LockpickingUI.Instance != null) LockpickingUI.Instance.Hide();

            if (playerHUD != null) playerHUD.SetActive(true);
            if (hotbarPanel != null) hotbarPanel.SetActive(true);
        }
    }
}