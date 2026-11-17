using UnityEngine;
using System.Linq; // <-- ВАЖНО: Добавляем поддержку поиска по списку

namespace Game.RPG
{
    public class DevInventoryTester : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private PlayerStats stats;

        [Header("Test Items")]
        [SerializeField] private ItemDataSO testArmor;
        [SerializeField] private ItemDataSO testWeapon;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 400, 300, 400), GUI.skin.box);
            GUILayout.Label("— Inventory Test —");

            GUILayout.Label($"Stealth: {stats.GetStat(StatType.Stealth):F1}");
            GUILayout.Label($"Melee: {stats.GetStat(StatType.Melee):F1}");

            if (GUILayout.Button("Добавить Броню") && testArmor != null)
                inventory.AddItem(testArmor, 1);
            if (GUILayout.Button("Добавить Меч") && testWeapon != null)
                inventory.AddItem(testWeapon, 1);

            GUILayout.Space(10);
            GUILayout.Label("— Equip/Unequip —");

            if (GUILayout.Button("Надеть Броню") && testArmor != null)
            {
                // Используем FirstOrDefault вместо Find
                var slot = inventory.Slots.FirstOrDefault(s => s.Item == testArmor);
                if (slot != null) inventory.EquipItem(slot);
                else Debug.Log("Броня не найдена в инвентаре!");
            }

            if (GUILayout.Button("Надеть Меч") && testWeapon != null)
            {
                var slot = inventory.Slots.FirstOrDefault(s => s.Item == testWeapon);
                if (slot != null) inventory.EquipItem(slot);
                else Debug.Log("Меч не найден в инвентаре!");
            }

            if (GUILayout.Button("Снять Броню"))
                inventory.UnequipItem(EquipmentType.Armor);
            if (GUILayout.Button("Снять Меч"))
                inventory.UnequipItem(EquipmentType.Weapon);

            GUILayout.Space(10);
            GUILayout.Label("— Drop —");

            if (GUILayout.Button("Выбросить Броню") && testArmor != null)
            {
                var slot = inventory.Slots.FirstOrDefault(s => s.Item == testArmor);
                if (slot != null) inventory.DropItem(slot, 1);
                else Debug.Log("Броня не найдена в инвентаре!");
            }

            GUILayout.EndArea();
        }
    }
}