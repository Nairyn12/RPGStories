using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIRaycastDebugger : MonoBehaviour
{
    void Update()
    {
        // Проверяем клик левой кнопкой мыши через новую систему ввода
        if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            // Переводим координаты мыши в экранные
            pointerData.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count == 0)
            {
                Debug.Log("[UI Debug] Мышь НИКУДА не попала! EventSystem сломан или Canvas не видит мышь.");
            }
            else
            {
                foreach (var result in results)
                {
                    Debug.Log($"[UI Debug] Мышь попала в: {result.gameObject.name} (Слой: {LayerMask.LayerToName(result.gameObject.layer)})");
                }
            }
        }
    }
}