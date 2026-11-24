# Project Digest — RPGStories
Unity 6000.2.6f2 | режим: API

## Scenes
- + Assets/Scenes/SampleScene.unity

## Packages
```json
{
  "dependencies": {
    "com.unity.ai.navigation": "2.0.9",
    "com.unity.cinemachine": "3.1.7",
    "com.unity.collab-proxy": "2.14.0",
    "com.unity.ide.visualstudio": "2.0.23",
    "com.unity.inputsystem": "1.14.2",
    "com.unity.multiplayer.center": "1.0.0",
    "com.unity.render-pipelines.universal": "17.2.0",
    "com.unity.test-framework": "1.6.0",
    "com.unity.timeline": "1.8.9",
    "com.unity.ugui": "2.0.0",
    "com.unity.visualscripting": "1.9.7",
    "com.unity.modules.accessibility": "1.0.0",
    "com.unity.modules.ai": "1.0.0",
    "com.unity.modules.androidjni": "1.0.0",
    "com.unity.modules.animation": "1.0.0",
    "com.unity.modules.assetbundle": "1.0.0",
    "com.unity.modules.audio": "1.0.0",
    "com.unity.modules.cloth": "1.0.0",
    "com.unity.modules.director": "1.0.0",
    "com.unity.modules.imageconversion": "1.0.0",
    "com.unity.modules.imgui": "1.0.0",
    "com.unity.modules.jsonserialize": "1.0.0",
    "com.unity.modules.particlesystem": "1.0.0",
    "com.unity.modules.physics": "1.0.0",
    "com.unity.modules.physics2d": "1.0.0",
    "com.unity.modules.screencapture": "1.0.0",
    "com.unity.modules.terrain": "1.0.0",
    "com.unity.modules.terrainphysics": "1.0.0",
    "com.unity.modules.tilemap": "1.0.0",
    "com.unity.modules.ui": "1.0.0",
    "com.unity.modules.uielements": "1.0.0",
    "com.unity.modules.umbra": "1.0.0",
    "com.unity.modules.unityanalytics": "1.0.0",
    "com.unity.modules.unitywebrequest": "1.0.0",
    "com.unity.modules.unitywebrequestassetbundle": "1.0.0",
    "com.unity.modules.unitywebrequestaudio": "1.0.0",
    "com.unity.modules.unitywebrequesttexture": "1.0.0",
    "com.unity.modules.unitywebrequestwww": "1.0.0",
    "com.unity.modules.vehicles": "1.0.0",
    "com.unity.modules.video": "1.0.0",
    "com.unity.modules.vr": "1.0.0",
    "com.unity.modules.wind": "1.0.0",
    "com.unity.modules.xr": "1.0.0"
  }
}

```

## File tree
- Assets/Scripts\RPG\Combat\CombatState.cs (53 строк)
- Assets/Scripts\RPG\Combat\EnemyAI.cs (152 строк)
- Assets/Scripts\RPG\Combat\EnemyDataSO.cs (32 строк)
- Assets/Scripts\RPG\Combat\EnemyHealth.cs (85 строк)
- Assets/Scripts\RPG\Combat\IDamageable.cs (12 строк)
- Assets/Scripts\RPG\Combat\PlayerCombat.cs (115 строк)
- Assets/Scripts\RPG\Combat\WeaponHitbox.cs (63 строк)
- Assets/Scripts\RPG\Dev\DevInventoryTester.cs (65 строк)
- Assets/Scripts\RPG\Dev\DevStatsTester.cs (135 строк)
- Assets/Scripts\RPG\Dev\TestDummy.cs (10 строк)
- Assets/Scripts\RPG\Dev\UIRaycastDebugger.cs (32 строк)
- Assets/Scripts\RPG\Dialogue\CompleteQuestAction.cs (18 строк)
- Assets/Scripts\RPG\Dialogue\DialogueActions.cs (9 строк)
- Assets/Scripts\RPG\Dialogue\DialogueCondition.cs (9 строк)
- Assets/Scripts\RPG\Dialogue\DialogueRunner.cs (179 строк)
- Assets/Scripts\RPG\Dialogue\DialogueSO.cs (41 строк)
- Assets/Scripts\RPG\Dialogue\FactionDataSO.cs (12 строк)
- Assets/Scripts\RPG\Dialogue\HasQuestCondition.cs (16 строк)
- Assets/Scripts\RPG\Dialogue\ItemCondition.cs (21 строк)
- Assets/Scripts\RPG\Dialogue\QuestReadyCondition.cs (16 строк)
- Assets/Scripts\RPG\Dialogue\QuestStateCondition.cs (38 строк)
- Assets/Scripts\RPG\Dialogue\ReputationCondition.cs (27 строк)
- Assets/Scripts\RPG\Dialogue\StartTradeAction.cs (22 строк)
- Assets/Scripts\RPG\Dialogue\StatCondition.cs (17 строк)
- Assets/Scripts\RPG\GameSaveData.cs (24 строк)
- Assets/Scripts\RPG\Interaction\Chest.cs (126 строк)
- Assets/Scripts\RPG\Interaction\IInteractable.cs (10 строк)
- Assets/Scripts\RPG\Interaction\InteractionUI.cs (38 строк)
- Assets/Scripts\RPG\Interaction\NPCInteractable.cs (21 строк)
- Assets/Scripts\RPG\Interaction\PlayerInteractor.cs (93 строк)
- Assets/Scripts\RPG\Interaction\PlayerInventory.cs (317 строк)
- Assets/Scripts\RPG\Interaction\WorldItem.cs (43 строк)
- Assets/Scripts\RPG\ISaveable.cs (9 строк)
- Assets/Scripts\RPG\Items\ArmorItemSO.cs (13 строк)
- Assets/Scripts\RPG\Items\ConsumableItemSO.cs (38 строк)
- Assets/Scripts\RPG\Items\ItemDataSO.cs (36 строк)
- Assets/Scripts\RPG\Items\MiscItemSO.cs (12 строк)
- Assets/Scripts\RPG\Items\WeaponItemSO.cs (24 строк)
- Assets/Scripts\RPG\Player\PlayerAnimator.cs (65 строк)
- Assets/Scripts\RPG\PlayerController.cs (443 строк)
- Assets/Scripts\RPG\PlayerHealth.cs (144 строк)
- Assets/Scripts\RPG\PlayerLeveling.cs (49 строк)
- Assets/Scripts\RPG\PlayerMana.cs (119 строк)
- Assets/Scripts\RPG\PlayerStamina.cs (150 строк)
- Assets/Scripts\RPG\PlayerStats.cs (167 строк)
- Assets/Scripts\RPG\Quests\GiveItemAction.cs (17 строк)
- Assets/Scripts\RPG\Quests\GiveXpAction.cs (16 строк)
- Assets/Scripts\RPG\Quests\ModifyReputationAction.cs (20 строк)
- Assets/Scripts\RPG\Quests\QuestDataSO.cs (46 строк)
- Assets/Scripts\RPG\Quests\QuestManager.cs (223 строк)
- Assets/Scripts\RPG\Quests\StartQuestAction.cs (30 строк)
- Assets/Scripts\RPG\ReputationManager.cs (90 строк)
- Assets/Scripts\RPG\SaveManager.cs (81 строк)
- Assets/Scripts\RPG\SkillBranchSO.cs (15 строк)
- Assets/Scripts\RPG\SkillCheckService.cs (74 строк)
- Assets/Scripts\RPG\SkillDefinitionSO.cs (35 строк)
- Assets/Scripts\RPG\StatModifier.cs (43 строк)
- Assets/Scripts\RPG\StatType.cs (61 строк)
- Assets/Scripts\RPG\Trade\MerchantInventory.cs (109 строк)
- Assets/Scripts\RPG\Trade\TradeManager.cs (52 строк)
- Assets/Scripts\RPG\UI\ChestSlotUI.cs (99 строк)
- Assets/Scripts\RPG\UI\ChestUI.cs (122 строк)
- Assets/Scripts\RPG\UI\Core\SceneReloader.cs (41 строк)
- Assets/Scripts\RPG\UI\DeathScreen.cs (40 строк)
- Assets/Scripts\RPG\UI\DialogueUI.cs (68 строк)
- Assets/Scripts\RPG\UI\EnemyHealthBar.cs (71 строк)
- Assets/Scripts\RPG\UI\EquipmentSlotUI.cs (85 строк)
- Assets/Scripts\RPG\UI\InventorySlotUI.cs (131 строк)
- Assets/Scripts\RPG\UI\InventoryUI.cs (164 строк)
- Assets/Scripts\RPG\UI\PlayerHUD.cs (86 строк)
- Assets/Scripts\RPG\UI\QuestJournalEntryUI.cs (29 строк)
- Assets/Scripts\RPG\UI\QuestJournalUI.cs (112 строк)
- Assets/Scripts\RPG\UI\QuickSlotUI.cs (85 строк)
- Assets/Scripts\RPG\UI\TooltipManager.cs (50 строк)
- Assets/Scripts\RPG\UI\TradeSlotUI.cs (55 строк)
- Assets/Scripts\RPG\UI\TradeUI.cs (205 строк)
- Assets/Scripts\RPG\XpCurveSO.cs (57 строк)

## Assets/Scripts\RPG\Combat\CombatState.cs
```csharp
namespace Game.RPG
    public class CombatState : MonoBehaviour
        public bool IsInCombat { get; private set; }
        [Tooltip("������� ������ ������ ������ ��� ������ ��������, ����� ����� �� ���")]
        [SerializeField] private float combatCooldown = 5f;

        public void EnterCombat()
        public void RefreshCombat()
        public void ForceExitCombat()
        [ContextMenu("Toggle Combat Mode")]
        public void ToggleCombatMode()
```

## Assets/Scripts\RPG\Combat\EnemyAI.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyAI : MonoBehaviour
        [Header("Data")]
        [SerializeField] private EnemyDataSO data;

        private enum State { Idle, Chasing, Attacking, Dead }
        [SerializeField] private GameObject healthBarPrefab;

```

## Assets/Scripts\RPG\Combat\EnemyDataSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Enemy_", menuName = "RPG/Enemy Data")]
    public class EnemyDataSO : ScriptableObject
        [Header("Identity")]
        public string EnemyId = "wolf"; // <-- ����� ���� (��� �������)
        public string EnemyName = "Wolf";
        public int EnemyLevel = 1;
        [Header("Stats")]
        public float MaxHealth = 50f;
        public float MovementSpeed = 3.5f;
        [Header("Combat")]
        public float Damage = 10f;
        public float AttackRange = 1.5f;
        public float AttackCooldown = 1.5f;
        [Header("Detection")]
        [Tooltip("������ ����������� ������")]
        public float DetectionRadius = 8f;
        [Tooltip("���� ������ (��������, 120 ��������)")]
        public float FieldOfView = 120f;
        [Header("Rewards")]
        [Tooltip("������� ����� ���� ���� ����")]
        public int XpReward = 50;
```

## Assets/Scripts\RPG\Combat\EnemyHealth.cs
```csharp
namespace Game.RPG
    public class EnemyHealth : MonoBehaviour, IDamageable
        [Header("Data")]
        [SerializeField] private EnemyDataSO data;

        public event System.Action<float, float> OnHealthChanged;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => data != null ? data.MaxHealth : 50f;
        public bool IsDead => _currentHealth <= 0;
        public void TakeDamage(float amount, GameObject source = null)
```

## Assets/Scripts\RPG\Combat\IDamageable.cs
```csharp
namespace Game.RPG
    public interface IDamageable
```

## Assets/Scripts\RPG\Combat\PlayerCombat.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerCombat : MonoBehaviour
        [Header("References")]
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerStamina stamina;
        [SerializeField] private CombatState combatState;
        [SerializeField] private PlayerController playerController; // <-- ��������
        [SerializeField] private PlayerAnimator playerAnimator;     // <-- ��������
        [SerializeField] private WeaponHitbox hitbox;
        [SerializeField] private WeaponItemSO currentWeapon;

        public bool IsAttacking { get; private set; }
        public void EquipWeapon(WeaponItemSO newWeapon)
```

## Assets/Scripts\RPG\Combat\WeaponHitbox.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(Collider))]
    public class WeaponHitbox : MonoBehaviour
        [Tooltip("����, �� �������� ����� ���� ������ (��������, Enemy)")]
        public LayerMask targetLayer;
        public void Activate(GameObject owner, float damage, float duration)
```

## Assets/Scripts\RPG\Dev\DevInventoryTester.cs
```csharp
namespace Game.RPG
    public class DevInventoryTester : MonoBehaviour
        [Header("References")]
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private PlayerStats stats;

        [Header("Test Items")]
        [SerializeField] private ItemDataSO testArmor;
        [SerializeField] private ItemDataSO testWeapon;

```

## Assets/Scripts\RPG\Dev\DevStatsTester.cs
```csharp
namespace Game.RPG
    public class DevStatsTester : MonoBehaviour
        [Header("Ссылки на системы (перетащить из Инспектора)")]
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private SkillDefinitionSO testSkillDef;
        [SerializeField] private PlayerLeveling playerLeveling;
        [SerializeField] private PlayerHealth playerHealth;     // ← ДОБАВЛЕНО
        [SerializeField] private SaveManager saveManager;       // ← ДОБАВЛЕНО
        [SerializeField] private PlayerStamina playerStamina;

        [Header("Тестовые настройки модификатора")]
        [SerializeField] private StatType testStat = StatType.Acrobatics;
        [SerializeField] private float testValue = 1f;
        [SerializeField] private ModifierType testType = ModifierType.Flat;

        [Header("Reputation Test")]
        [SerializeField] private ReputationManager reputationManager;
        [SerializeField] private FactionDataSO testFaction; // Перетащи сюда Faction_Village

```

## Assets/Scripts\RPG\Dev\TestDummy.cs
```csharp
public class TestDummy : MonoBehaviour, IDamageable
    public void TakeDamage(float amount, GameObject source = null)
```

## Assets/Scripts\RPG\Dev\UIRaycastDebugger.cs
```csharp
public class UIRaycastDebugger : MonoBehaviour
```

## Assets/Scripts\RPG\Dialogue\CompleteQuestAction.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Action_CompleteQuest_", menuName = "RPG/Dialogue/Action - Complete Quest")]
    public class CompleteQuestAction : DialogueAction
        public QuestDataSO QuestToComplete;
        public override void Execute(GameObject player)
```

## Assets/Scripts\RPG\Dialogue\DialogueActions.cs
```csharp
namespace Game.RPG
    public abstract class DialogueAction : ScriptableObject
        public abstract void Execute(GameObject player);
```

## Assets/Scripts\RPG\Dialogue\DialogueCondition.cs
```csharp
namespace Game.RPG
    public abstract class DialogueCondition : ScriptableObject
        public abstract bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction);
```

## Assets/Scripts\RPG\Dialogue\DialogueRunner.cs
```csharp
namespace Game.RPG
    public class DialogueRunner : MonoBehaviour
        public static DialogueRunner Instance { get; private set; }
        public static bool IsDialogueActive { get; private set; } = false;
        [SerializeField] private DialogueUI ui;
        [SerializeField] private PlayerController playerController;

        public void RefreshReferences()
        public void StartDialogue(DialogueSO dialogue, GameObject player, FactionDataSO npcFaction)
        public void EndDialogue()
```

## Assets/Scripts\RPG\Dialogue\DialogueSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Dialogue_", menuName = "RPG/Dialogue/Dialogue")]
    public class DialogueSO : ScriptableObject
        public string NpcName;
        public List<DialogueNode> Nodes = new();
    [System.Serializable]
    public class DialogueNode
        [TextArea(2, 5)]
        public string NpcText;
        [Tooltip("������� ��� ������ ������� � ����� ����. ���� ����� - �������� ����.")]
        public List<DialogueCondition> Conditions = new();
        public List<DialogueAction> OnEnterActions = new();
        [Tooltip("���� ����� - ������ '����������'")]
        public List<DialogueChoice> Choices = new();
        [Header("Node Settings")]
        [Tooltip("������� �������, ���� ����� ����� ������ ������ ������ ����������� (���� ��� �������)")]
        public bool EndDialogueHere = false; // <-- ����� ����
    [System.Serializable]
    public class DialogueChoice
        public string ChoiceText;
        public int NextNodeIndex;
        public List<DialogueCondition> Conditions = new();
        public List<DialogueAction> Actions = new();
```

## Assets/Scripts\RPG\Dialogue\FactionDataSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Faction_", menuName = "RPG/Faction")]
    public class FactionDataSO : ScriptableObject
        public string FactionId; // <-- �����: ��������, "village"
        public string FactionName;
        public Sprite Icon; // ���������� ��� UI
```

## Assets/Scripts\RPG\Dialogue\HasQuestCondition.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Cond_HasQuest_", menuName = "RPG/Dialogue/Condition - Has Quest Active")]
    public class HasQuestCondition : DialogueCondition
        public QuestDataSO QuestToCheck;
        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
```

## Assets/Scripts\RPG\Dialogue\ItemCondition.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Cond_Item_", menuName = "RPG/Dialogue/Condition - Item")]
    public class ItemCondition : DialogueCondition
        public ItemDataSO RequiredItem;
        public bool MustBeEquipped = false;
        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
```

## Assets/Scripts\RPG\Dialogue\QuestReadyCondition.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Cond_QuestReady_", menuName = "RPG/Dialogue/Condition - Quest Ready")]
    public class QuestReadyCondition : DialogueCondition
        public QuestDataSO QuestToCheck;
        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
```

## Assets/Scripts\RPG\Dialogue\QuestStateCondition.cs
```csharp
namespace Game.RPG
    public enum QuestStateToCheck { IsNotTaken, IsActive, IsReadyToTurnIn, IsCompleted }
    [CreateAssetMenu(fileName = "Cond_QuestState_", menuName = "RPG/Dialogue/Condition - Quest State")]
    public class QuestStateCondition : DialogueCondition
        public QuestDataSO QuestToCheck;
        public QuestStateToCheck StateToCheck;
        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
```

## Assets/Scripts\RPG\Dialogue\ReputationCondition.cs
```csharp
namespace Game.RPG
    public enum ComparisonType { GreaterOrEqual, LessOrEqual }
    [CreateAssetMenu(fileName = "Cond_Reputation_", menuName = "RPG/Dialogue/Condition - Reputation")]
    public class ReputationCondition : DialogueCondition
        public int RequiredValue;
        public ComparisonType Comparison = ComparisonType.GreaterOrEqual;
        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
```

## Assets/Scripts\RPG\Dialogue\StartTradeAction.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Action_StartTrade_", menuName = "RPG/Dialogue/Action - Start Trade")]
    public class StartTradeAction : DialogueAction
        public override void Execute(GameObject player)
```

## Assets/Scripts\RPG\Dialogue\StatCondition.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Cond_Stat_", menuName = "RPG/Dialogue/Condition - Stat")]
    public class StatCondition : DialogueCondition
        public StatType Stat;
        public int RequiredValue;
        public override bool IsMet(PlayerStats stats, PlayerInventory inventory, FactionDataSO npcFaction)
```

## Assets/Scripts\RPG\GameSaveData.cs
```csharp
namespace Game.RPG
    [Serializable]
    public class GameSaveData
        public string SceneName;
        public Vector3 PlayerPosition;
        public Quaternion PlayerRotation;
        public string SaveDate;
        public int PlayerLevel;
        public List<SaveEntry> Entries = new();
    [Serializable]
    public class SaveEntry
        public string Key;
        public string Json;
```

## Assets/Scripts\RPG\Interaction\Chest.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(Collider))]
    public class Chest : MonoBehaviour, IInteractable, ISaveable
        [Header("Loot Settings")]
        [SerializeField] private List<PlayerInventory.InventorySlot> startLoot = new();

        public IReadOnlyList<PlayerInventory.InventorySlot> Slots => _slots;
        public event Action OnChestChanged;
        public string GetInteractionText() => "������� ������";
        public void Interact(GameObject interactor)
        public void AddItem(ItemDataSO item, int amount)
        public bool RemoveItem(PlayerInventory.InventorySlot slot, int amount = 1)
        public string SaveKey => $"Chest_{gameObject.name}_{transform.position.x}_{transform.position.z}";
        [Serializable]
        private class SaveData
            public List<string> ItemIds = new();
            public List<int> Amounts = new();
        public string CaptureState()
        public void RestoreState(string json)
```

## Assets/Scripts\RPG\Interaction\IInteractable.cs
```csharp
namespace Game.RPG
    public interface IInteractable
```

## Assets/Scripts\RPG\Interaction\InteractionUI.cs
```csharp
namespace Game.RPG
    public class InteractionUI : MonoBehaviour
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI promptText;

```

## Assets/Scripts\RPG\Interaction\NPCInteractable.cs
```csharp
namespace Game.RPG
    public class NPCInteractable : MonoBehaviour, IInteractable
        [SerializeField] private DialogueSO dialogue;
        [SerializeField] private FactionDataSO faction; // <-- �����

        public string GetInteractionText()
        public void Interact(GameObject interactor)
```

## Assets/Scripts\RPG\Interaction\PlayerInteractor.cs
```csharp
namespace Game.RPG
    public class PlayerInteractor : MonoBehaviour
        [Header("Settings")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactLayer;

        public event Action<string> OnInteractPromptChanged;
```

## Assets/Scripts\RPG\Interaction\PlayerInventory.cs
```csharp
namespace Game.RPG
    public class PlayerInventory : MonoBehaviour, ISaveable
        [Serializable]
        public class InventorySlot
            public ItemDataSO Item;
            public int Amount;
            public EquipmentType EquippedSlot = EquipmentType.None;
            public int HotbarIndex = -1; // -1 значит не в хотбаре
        [Header("References")]
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private Transform playerTransform;

        [Header("Drop Settings")]
        [SerializeField] private GameObject genericLootBagPrefab;
        [SerializeField] private float dropDistance = 1.5f;

        [SerializeField] private List<InventorySlot> _slots = new();
        public IReadOnlyList<InventorySlot> Slots => _slots;
        public event Action OnInventoryChanged;
        public event Action OnEquipmentChanged;
        public void AddItem(ItemDataSO item, int amount = 1)
        public bool RemoveItem(InventorySlot slot, int amount = 1)
        public void EquipItem(InventorySlot targetSlot)
        public void UnequipItem(EquipmentType type)
        public InventorySlot GetEquippedSlot(EquipmentType type)
        public void SetQuickSlot(int index, InventorySlot targetSlot)
        public void ClearQuickSlot(int index)
        public InventorySlot GetQuickSlot(int index)
        public void UseQuickSlot(int index)
        public void UseItem(InventorySlot slot)
        public void DropItem(InventorySlot slot, int amount = 1)
        public string SaveKey => "PlayerInventory";
        [Serializable]
        private class SaveData
            public List<string> ItemIds = new();
            public List<int> Amounts = new();
            public List<int> EquippedSlots = new();
            public List<int> HotbarIndexes = new();
        public string CaptureState()
        public void RestoreState(string json)
```

## Assets/Scripts\RPG\Interaction\WorldItem.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(Collider))]
    public class WorldItem : MonoBehaviour, IInteractable
        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private int amount = 1;

        public void Setup(ItemDataSO item, int itemAmount)
        public string GetInteractionText()
        public void Interact(GameObject interactor)
```

## Assets/Scripts\RPG\ISaveable.cs
```csharp
namespace Game.RPG
    public interface ISaveable
```

## Assets/Scripts\RPG\Items\ArmorItemSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Armor_", menuName = "RPG/Items/Armor")]
    public class ArmorItemSO : ItemDataSO
        [Header("Stat Modifiers")]
        [Tooltip("����� ����� ������ ��� ����� (��������, +2 � ����������)")]
        public List<StatModifier> Modifiers = new();
```

## Assets/Scripts\RPG\Items\ConsumableItemSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Consumable_", menuName = "RPG/Items/Consumable")]
    public class ConsumableItemSO : ItemDataSO
        [Header("Effects")]
        public float HealthRestore = 0f;
        public float StaminaRestore = 0f;
        public float ManaRestore = 0f;     
        [Tooltip("�� ������� ������ ����������������� �������� (0 = ���������)")]
        public float HealthRestoreDuration = 3f; // <-- ����� ����
        public void Use(GameObject player)
```

## Assets/Scripts\RPG\Items\ItemDataSO.cs
```csharp
namespace Game.RPG
    public enum ItemType { Weapon, Armor, Consumable, Quest, Misc }
    public enum EquipmentType { None, Weapon, Armor, Helmet }
    public abstract class ItemDataSO : ScriptableObject
        [Header("Base Info")]
        public string ItemId;
        public string ItemName;
        [TextArea] public string Description;
        public Sprite Icon;
        [Header("Trade Settings")]
        public int BasePrice = 10;
        public bool IsUnique = false; // ���������� �������� (������� Persuasion)
        [Header("World Representation")] // <-- �����
        [Tooltip("������, ������� ���������, ���� ��������� ������� �� �����")]
        public GameObject WorldPrefab;
        [Header("Inventory Rules")]
        public ItemType Type;
        public int MaxStack = 1;
        [Tooltip("����� �� �������� ���� ������� � ������ �������� ������� (������)?")]
        public bool CanBeInHotbar = true; // <-- ����� ����
        [Header("Equipment")] // <-- ��������
        public EquipmentType EquipSlot = EquipmentType.None;        
```

## Assets/Scripts\RPG\Items\MiscItemSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Misc_", menuName = "RPG/Items/Misc or Quest")]
    public class MiscItemSO : ItemDataSO
```

## Assets/Scripts\RPG\Items\WeaponItemSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Weapon_", menuName = "RPG/Items/Weapon")]
    public class WeaponItemSO : ItemDataSO
        [Header("Combat Stats")]
        public float BaseDamage = 10f;
        public float AttackCooldown = 0.8f;
        public float AttackRange = 1.5f;
        public float StaminaCost = 0f;
        [Header("Scaling")]
        [Tooltip("��������� ��������� ������ ���� �� 1 ������� ������ Melee")]
        public float MeleeScaling = 0.05f;
        public float CalculateDamage(PlayerStats stats)
```

## Assets/Scripts\RPG\Player\PlayerAnimator.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerController controller; // Для проверки земли
        [SerializeField] private PlayerCombat combat;

        public void PlayAttackAnimation()
        public void PlayHitAnimation()
```

## Assets/Scripts\RPG\PlayerController.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerStamina stamina;
        [SerializeField] private PlayerHealth health;
        [SerializeField] private PlayerCombat playerCombat; // <-- ДОБАВИТЬ
        [SerializeField] private CombatState combatState;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private PlayerInventory inventory;

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float crouchSpeed = 1.5f;
        [SerializeField] private float acceleration = 15f;
        [SerializeField] private float rotationSpeed = 12f;

        [Tooltip("Насколько процентов растет скорость за 1 единицу характеристик (Speed, Acrobatics)")]
        [SerializeField] private float speedBonusPerPoint = 0.025f; // 2.5%

        [Header("Jump & Dodge Settings")]
        [SerializeField] private float baseJumpForce = 7f;
        [SerializeField] private float baseDodgeForce = 10f;
        [SerializeField] private float staminaCostPerSecondSprint = 10f;
        [SerializeField] private float staminaCostDodge = 20f;
        [SerializeField] private float jumpAnticipationTime = 0.6f; // <-- ДОБАВИТЬ
        private bool _isJumpingAnticipation = false; // <-- ДОБАВИТЬ
        [Tooltip("Множитель прыжка/уклонения за 1 единицу Акробатики (0.01 = 1%)")]
        [SerializeField] private float acrobaticsBonusPerPoint = 0.01f;

        [Header("Fall Damage Settings")]
        [SerializeField] private float baseSafeFallHeight = 2f;
        [Tooltip("Насколько метров повышается безопасная высота за 1 единицу Акробатики")]
        [SerializeField] private float acrobaticsFallResistance = 0.1f;
        [SerializeField] private float damagePerMeter = 10f;

        [Header("Crouch Settings")]
        [SerializeField] private float standHeight = 1.8f;
        [SerializeField] private float crouchHeight = 1.0f;

        [Header("Landing Settings")]
        [Tooltip("Сколько секунд игрок не может двигаться после приземления")]
        [SerializeField] private float landingLockDuration = 1f;

        public bool IsLanding { get; private set; } // Флаг для других скриптов (если нужно)
        public bool IsGrounded => _isGrounded; // <-- ДОБАВИТЬ ЭТУ СТРОКУ
        [SerializeField] private PlayerAnimator playerAnimator;

```

## Assets/Scripts\RPG\PlayerHealth.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerHealth : MonoBehaviour, ISaveable
        [SerializeField] private float currentHealth;
        private PlayerStats _stats;
        public event Action<float, float> OnHealthChanged; // (current, max)
        public event Action OnDeath;
        public event Action OnDamaged;
        [SerializeField] private float invulnerabilityDuration = 0.5f; // ������������ ����������

        public float CurrentHealth => currentHealth;
        public float MaxHealth => _stats.GetStat(StatType.HealthMax);
        public bool IsDead => currentHealth <= 0;
        public void TakeDamage(float amount)
        public void Heal(float amount)
        public string SaveKey => "PlayerHealth";
        public string CaptureState()
        public void RestoreState(string json)
        [Serializable]
        private class HealthData { public float Health; }
        public void HealOverTime(float totalAmount, float duration)
```

## Assets/Scripts\RPG\PlayerLeveling.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerLeveling : MonoBehaviour, ISaveable
        [SerializeField] private XpCurveSO xpCurve;
        [SerializeField] private int currentXp = 0;
        private PlayerStats _stats;
        public event Action<int, int> OnXpChanged;
        public event Action<int> OnLevelUp;
        public int CurrentXp => currentXp;
        public int MaxLevel => xpCurve ? xpCurve.MaxLevel : 99;
        public int XpForNextLevel => xpCurve ? xpCurve.GetXpForLevel(_stats.Level) : 100;
        public float XpProgress01 => XpForNextLevel > 0 ? Mathf.Clamp01((float)currentXp / XpForNextLevel) : 0f;
        public void AddXp(int amount)
        public string SaveKey => "PlayerLeveling";
        public string CaptureState() => JsonUtility.ToJson(new XpData { Xp = currentXp });
        public void RestoreState(string json)
        [Serializable] private class XpData { public int Xp; }
    }
```

## Assets/Scripts\RPG\PlayerMana.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerMana : MonoBehaviour, ISaveable
        [Header("������� ���������")]
        [SerializeField] private float currentMana;

        [Header("��������� ��������������")]
        [Tooltip("������� ���� ����������������� � �������")]
        [SerializeField] private float regenRate = 2f;

        public event Action<float, float> OnManaChanged; // (current, max)
        public float CurrentMana => currentMana;
        public float MaxMana => _stats.GetStat(StatType.ManaMax);
        public bool TryConsume(float amount)
        public void RestoreMana(float amount)
        public string SaveKey => "PlayerMana";
        public string CaptureState()
        public void RestoreState(string json)
        [Serializable]
        private class ManaData { public float Mana; }
```

## Assets/Scripts\RPG\PlayerStamina.cs
```csharp
namespace Game.RPG
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerStamina : MonoBehaviour, ISaveable
        [Header("������� ���������")]
        [SerializeField] private float currentStamina;

        [Header("��������� ��������������")]
        [Tooltip("������� ������� ����������������� � �������")]
        [SerializeField] private float regenRate = 15f;

        [Tooltip("�������� ����� ������� �������������� ����� ����� (� ��������)")]
        [SerializeField] private float regenDelay = 1f;

        public event Action<float, float> OnStaminaChanged; // (current, max)
        [SerializeField] private float exhaustedRecoveryPercent = 0.3f;
        private bool _isExhausted = false;
        public bool CanSprint => !_isExhausted && currentStamina > 0f;
        public float CurrentStamina => currentStamina;
        public float MaxStamina => _stats.GetStat(StatType.StaminaMax);
        public float StaminaPercent => MaxStamina > 0 ? currentStamina / MaxStamina : 0f;
        public bool HasEnoughStamina => currentStamina > 0f;
        public bool TryConsume(float amount)
        public void RestoreStamina(float amount)
        public string SaveKey => "PlayerStamina";
        public string CaptureState()
        public void RestoreState(string json)
        [Serializable]
        private class StaminaData { public float Stamina; }
```

## Assets/Scripts\RPG\PlayerStats.cs
```csharp
namespace Game.RPG
    public class PlayerStats : MonoBehaviour, ISaveable
        [Header("Base Resources")]
        [SerializeField] private float baseHealth = 100f;
        [SerializeField] private float baseStamina = 50f;
        [SerializeField] private float baseMana = 30f;
        [SerializeField] private float baseSpeed = 5f;

        [Header("Per-Level Growth")]
        [SerializeField] private float healthPerLevel = 10f;
        [SerializeField] private float staminaPerLevel = 5f;
        [SerializeField] private float manaPerLevel = 5f;

        [Header("Magic Scaling")]
        [Tooltip("Сколько маны дает 1 уровень любого магического навыка")]
        [SerializeField] private float manaPerMagicPoint = 2f; // <-- ДОБАВИТЬ

        [Header("Level")]
        [SerializeField] private int level = 1;
        [SerializeField] private int skillPoints = 0;
        [SerializeField] private int startingSkillPoints = 0;

        public event Action OnStatsChanged;
        public event Action<StatType, float> OnStatChanged;
        public int Level => level;
        public int SkillPoints => skillPoints;
        public float GetStat(StatType stat)
        public int GetBaseSkillValue(StatType skill) => _baseSkillValues.TryGetValue(skill, out var v) ? v : 0;
        public int GetTotalMagicSkill()
        public void AddModifier(StatModifier m) { _modifiers.Add(m); NotifyStatChanged(m.Stat); }
        public void RemoveModifiersBySource(string sourceId)
        public IReadOnlyList<StatModifier> GetAllModifiers() => _modifiers;
        public bool TrySpendSkillPoint(StatType skill, int cost, int maxValue)
        public bool TrySpendSkillPoint(SkillDefinitionSO skillDef)
        public void AddSkillPoints(int amount) { if (amount > 0) { skillPoints += amount; OnStatsChanged?.Invoke(); } }
        public void LevelUp(int pointsGranted = 1)
        public string SaveKey => "PlayerStats";
        public string CaptureState()
        public void RestoreState(string json)
    [Serializable]
    public class RPGState
        public int Level; public int SkillPoints;
        public List<SkillEntry> BaseSkills = new(); public List<ModifierEntry> PermanentModifiers = new();
    [Serializable] public class SkillEntry { public StatType Stat; public int Value; }
    [Serializable] public class ModifierEntry { public StatType Stat; public ModifierType Type; public float Value; public string SourceId; public ModifierSource Source; }
}
```

## Assets/Scripts\RPG\Quests\GiveItemAction.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Action_GiveItem_", menuName = "RPG/Dialogue/Action - Give Item")]
    public class GiveItemAction : DialogueAction
        public ItemDataSO ItemToGive;
        public int Amount = 1;
        public override void Execute(GameObject player)
```

## Assets/Scripts\RPG\Quests\GiveXpAction.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Action_GiveXp_", menuName = "RPG/Dialogue/Action - Give XP")]
    public class GiveXpAction : DialogueAction
        public int XpAmount = 50;
        public override void Execute(GameObject player)
```

## Assets/Scripts\RPG\Quests\ModifyReputationAction.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Action_ModifyRep_", menuName = "RPG/Dialogue/Action - Modify Reputation")]
    public class ModifyReputationAction : DialogueAction
        public FactionDataSO Faction;
        public int Amount;
        public override void Execute(GameObject player)
```

## Assets/Scripts\RPG\Quests\QuestDataSO.cs
```csharp
namespace Game.RPG
    public enum ObjectiveType
    [System.Serializable]
    public class QuestObjective
        public ObjectiveType Type;
        [Tooltip("ID ����: ID ����� (��������, wolf), ID �������� (heal_potion) ��� ID NPC")]
        public string TargetId;
        [Tooltip("������� ��� ����� ��������� (����� 5 ������, ������� 3 �����)")]
        public int RequiredAmount = 1;
    [CreateAssetMenu(fileName = "Quest_", menuName = "RPG/Quest")]
    public class QuestDataSO : ScriptableObject
        [Header("Identity")]
        [Tooltip("���������� ID ��� ���������� (��������, quest_find_healer)")]
        public string QuestId;
        public string Title;
        [TextArea(3, 5)]
        public string Description;
        [Header("Objectives")]
        public List<QuestObjective> Objectives = new();
        [Header("Rewards")]
        public int RewardXp;
        public ItemDataSO RewardItem;
        public int RewardItemAmount = 1;
```

## Assets/Scripts\RPG\Quests\QuestManager.cs
```csharp
namespace Game.RPG
    [Serializable]
    public class ActiveQuest
        public QuestDataSO Quest;
        public List<int> Progress = new List<int>();
        public bool IsReadyToTurnIn = false;
        public bool IsCompleted
    public class QuestManager : MonoBehaviour, ISaveable
        public static QuestManager Instance { get; private set; }
        [Header("Active Quests")]
        [SerializeField] private List<ActiveQuest> _activeQuests = new();
        // НОВОЕ: Список сданных квестов, чтобы не брать их повторно
        [SerializeField] private List<QuestDataSO> _completedQuests = new();

        [Header("References")]
        [SerializeField] private PlayerLeveling _playerLeveling;
        [SerializeField] private PlayerInventory _playerInventory;

        public event Action OnQuestsChanged;
        public IReadOnlyList<ActiveQuest> ActiveQuests => _activeQuests;
        public void StartQuest(QuestDataSO quest)
        public void ReportKill(string enemyId)
        public void ReportCollect(string itemId, int amount)
        public void TurnInQuest(QuestDataSO quest)
        public bool IsQuestReadyToTurnIn(QuestDataSO quest)
        public bool IsQuestActive(QuestDataSO quest)
        public bool IsQuestCompleted(QuestDataSO quest)
        public string SaveKey => "QuestManager";
        public string CaptureState() => "{}";
        public void RestoreState(string json) { }
```

## Assets/Scripts\RPG\Quests\StartQuestAction.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Action_StartQuest_", menuName = "RPG/Dialogue/Action - Start Quest")]
    public class StartQuestAction : DialogueAction
        public QuestDataSO QuestToStart;
        public override void Execute(GameObject player)
```

## Assets/Scripts\RPG\ReputationManager.cs
```csharp
namespace Game.RPG
    public class ReputationManager : MonoBehaviour, ISaveable
        [Serializable]
        public class FactionReputation
            public FactionDataSO Faction;
            public int Value;
        [SerializeField] private List<FactionReputation> _reputations = new();

        public event Action OnReputationChanged;
        public int GetReputation(FactionDataSO faction)
        public void ModifyReputation(FactionDataSO faction, int amount)
        public string SaveKey => "ReputationManager";
        [Serializable]
        private class SaveData
            public List<string> FactionIds = new();
            public List<int> Values = new();
        public string CaptureState()
        public void RestoreState(string json)
```

## Assets/Scripts\RPG\SaveManager.cs
```csharp
namespace Game.RPG
    public class SaveManager : MonoBehaviour
        public static SaveManager Instance { get; private set; }
        public void SaveGame()
        public void LoadGame()
```

## Assets/Scripts\RPG\SkillBranchSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Branch_", menuName = "RPG/Skill Branch")]
    public class SkillBranchSO : ScriptableObject
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Icon;
        public List<SkillDefinitionSO> Skills = new();
```

## Assets/Scripts\RPG\SkillCheckService.cs
```csharp
namespace Game.RPG
    public enum CheckDifficulty
    public struct SkillCheckResult
        public StatType Skill;
        public float SkillValue;
        public int Difficulty;
        public float Margin;
        public bool Success;
        public override string ToString() =>
    public static class SkillCheckService
        public static bool VerboseLogging = false;
        public static void Register(PlayerStats player) => _player = player;
        public static void Unregister(PlayerStats player)
        public static bool Check(StatType skill, CheckDifficulty difficulty)
        public static bool Check(StatType skill, int difficulty)
        public static SkillCheckResult Evaluate(StatType skill, CheckDifficulty difficulty)
        public static SkillCheckResult Evaluate(StatType skill, int difficulty)
```

## Assets/Scripts\RPG\SkillDefinitionSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "Skill_", menuName = "RPG/Skill Definition")]
    public class SkillDefinitionSO : ScriptableObject
        [Header("Identity")]
        public StatType Stat;
        [Tooltip("���������� ��������� id ��� ������ � �������. ������ ��������� ����� �������.")]
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Icon;
        [Header("Progression")]
        public SkillBranchSO Branch;
        [Range(1, 30)] public int MaxValue = 20;

        [Header("Skill Point Cost (� �����)")]
        public int CostLow = 1;   // �������� 0-9
        public int CostMid = 2;   // �������� 10-14
        public int CostHigh = 3;   // �������� 15+
        [Header("Tags (��� �������� � ��������)")]
        public string[] CheckTags = { };
        public int GetCostForNextPoint(int currentValue)
```

## Assets/Scripts\RPG\StatModifier.cs
```csharp
namespace Game.RPG
    public enum ModifierType { Flat, Percent }
    public enum ModifierSource
    [Serializable]
    public struct StatModifier
        public StatType Stat;
        public ModifierType Type;
        public float Value;
        public string SourceId;
        public ModifierSource Source;
        public StatModifier(StatType stat, ModifierType type, float value,
        public override string ToString()
```

## Assets/Scripts\RPG\StatType.cs
```csharp
namespace Game.RPG
    public enum StatType
    public static class StatTypeExtensions
        public static bool IsResource(this StatType s) => (int)s >= 0 && (int)s < 100;
        public static bool IsSkill(this StatType s) => (int)s >= 100;
        public static bool IsAgilitySkill(this StatType s) => (int)s >= 100 && (int)s < 200;
        public static bool IsCombatSkill(this StatType s) => (int)s >= 200 && (int)s < 300;
        public static bool IsMagicSkill(this StatType s) => (int)s >= 300 && (int)s < 400;
        public static bool IsSocialSkill(this StatType s) => (int)s >= 400 && (int)s < 500;
        public static bool IsEnduranceSkill(this StatType s) => (int)s >= 500 && (int)s < 600;
```

## Assets/Scripts\RPG\Trade\MerchantInventory.cs
```csharp
namespace Game.RPG
    public class MerchantInventory : MonoBehaviour, ISaveable
        [Header("Faction & Settings")]
        public FactionDataSO Faction;
        [Header("Starting Stats")]
        public int StartGold = 500;
        public List<PlayerInventory.InventorySlot> StartItems = new();
        [Tooltip("��������� ����� ��������� ��� ������� ���������� �����")]
        public int RequiredPersuasionForUnique = 5;
        public int Gold => _currentGold;
        public IReadOnlyList<PlayerInventory.InventorySlot> Items => _items;
        public event Action OnMerchantChanged;
        public void AddGold(int amount) { _currentGold += amount; OnMerchantChanged?.Invoke(); }
        public void RemoveGold(int amount) { _currentGold = Mathf.Max(0, _currentGold - amount); OnMerchantChanged?.Invoke(); }
        public void AddItem(ItemDataSO item, int amount)
        public bool RemoveItem(PlayerInventory.InventorySlot slot, int amount)
        public string SaveKey => $"Merchant_{gameObject.name}";
        [Serializable]
        private class SaveData
            public int Gold;
            public List<string> ItemIds = new();
            public List<int> Amounts = new();
        public string CaptureState()
        public void RestoreState(string json)
```

## Assets/Scripts\RPG\Trade\TradeManager.cs
```csharp
namespace Game.RPG
    public static class TradeManager
        public static int CalculateBuyPrice(ItemDataSO item, PlayerStats stats, ReputationManager rep, FactionDataSO faction)
        public static int CalculateSellPrice(ItemDataSO item, PlayerStats stats, ReputationManager rep, FactionDataSO faction)
        public static bool CanTrade(ReputationManager rep, FactionDataSO faction)
```

## Assets/Scripts\RPG\UI\ChestSlotUI.cs
```csharp
namespace Game.RPG
    public class ChestSlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;

        public PlayerInventory.InventorySlot Slot { get; private set; }
        public void Setup(PlayerInventory.InventorySlot slot, Chest chest)
        public void OnPointerClick(PointerEventData eventData)
        public void OnPointerEnter(PointerEventData eventData)
        public void OnPointerExit(PointerEventData eventData)
        public void OnBeginDrag(PointerEventData eventData)
        public void OnDrag(PointerEventData eventData)
        public void OnEndDrag(PointerEventData eventData)
```

## Assets/Scripts\RPG\UI\ChestUI.cs
```csharp
namespace Game.RPG
    public class ChestUI : MonoBehaviour
        public static ChestUI Instance { get; private set; }
        [Header("References")]
        [SerializeField] private GameObject chestPanel;
        [SerializeField] private Transform chestGridParent;
        [SerializeField] private GameObject chestSlotPrefab;
        [SerializeField] private GameObject characterPanel;

        public bool IsOpen { get; private set; } = false;
        public Chest CurrentChest => _currentChest; // 🛠 Доступ для Drag & Drop
        public void Open(Chest chest)
        public void Close()
        public bool IsPointerOverChest(Vector2 screenPos, Camera cam)
```

## Assets/Scripts\RPG\UI\Core\SceneReloader.cs
```csharp
namespace Game.RPG
    public class SceneReloader : MonoBehaviour
```

## Assets/Scripts\RPG\UI\DeathScreen.cs
```csharp
namespace Game.RPG
    public class DeathScreen : MonoBehaviour
        [SerializeField] private GameObject screenPanel;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private SaveManager saveManager;

        public void OnLoadButtonClicked()
```

## Assets/Scripts\RPG\UI\DialogueUI.cs
```csharp
namespace Game.RPG
    public class DialogueUI : MonoBehaviour
        [Header("Main Panel")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Choices")]
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private GameObject choiceButtonPrefab;

        public void Show(DialogueSO dialogue)
        public void Hide()
        public void SetText(string text) => dialogueText.text = text;
        public void ClearChoices()
        public void SpawnChoiceButton(string text, int targetIndex, bool isAvailable, System.Action<int> onChoiceSelected)
```

## Assets/Scripts\RPG\UI\EnemyHealthBar.cs
```csharp
namespace Game.RPG
    public class EnemyHealthBar : MonoBehaviour
        [SerializeField] private Image fillImage;

        [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0); // ������ ��� �������

        public void Initialize(EnemyHealth health, Transform target)
```

## Assets/Scripts\RPG\UI\EquipmentSlotUI.cs
```csharp
namespace Game.RPG
    public class EquipmentSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
        [SerializeField] private EquipmentType slotType;
        [SerializeField] private Image iconImage;
        [SerializeField] private Outline borderImage;

        public void UpdateUI()
        public void OnPointerClick(PointerEventData eventData)
        public void OnDrop(PointerEventData eventData)
        public void OnPointerEnter(PointerEventData eventData)
        public void OnPointerExit(PointerEventData eventData)
```

## Assets/Scripts\RPG\UI\InventorySlotUI.cs
```csharp
namespace Game.RPG
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
        [Header("UI Elements")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Outline borderImage;
        [SerializeField] private TextMeshProUGUI hotbarText;

        public PlayerInventory.InventorySlot Slot { get; private set; }
        public static InventorySlotUI CurrentlyHoveredSlot { get; set; }
        public void Setup(PlayerInventory.InventorySlot slot, PlayerInventory inventory)
        public void OnPointerClick(PointerEventData eventData)
        public void OnPointerEnter(PointerEventData eventData)
        public void OnPointerExit(PointerEventData eventData)
        public void OnBeginDrag(PointerEventData eventData)
        public void OnDrag(PointerEventData eventData)
        public void OnEndDrag(PointerEventData eventData)
```

## Assets/Scripts\RPG\UI\InventoryUI.cs
```csharp
namespace Game.RPG
    public class InventoryUI : MonoBehaviour
        public static InventoryUI Instance { get; private set; }
        [Header("References")]
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private Transform gridParent;
        [SerializeField] private GameObject slotPrefab;

        public bool IsOpen { get; private set; } = false;
        public void ToggleInventory()
        public void ForceOpen()
        public void ForceClose()
        public void UpdateUI()
        public bool IsPointerOverInventory(Vector2 screenPos, Camera cam)
```

## Assets/Scripts\RPG\UI\PlayerHUD.cs
```csharp
namespace Game.RPG
    public class PlayerHUD : MonoBehaviour
        [Header("References")]
        [SerializeField] private PlayerHealth health;
        [SerializeField] private PlayerStamina stamina;
        [SerializeField] private PlayerLeveling leveling;
        [SerializeField] private PlayerMana mana;

        [Header("UI Elements")]
        [SerializeField] private Image healthFill;
        [SerializeField] private Image staminaFill;
        [SerializeField] private Image manaFill;
        [SerializeField] private Image xpFill;
        [SerializeField] private TextMeshProUGUI levelText;

```

## Assets/Scripts\RPG\UI\QuestJournalEntryUI.cs
```csharp
namespace Game.RPG
    public class QuestJournalEntryUI : MonoBehaviour
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button button;

        public void Setup(ActiveQuest quest, QuestJournalUI journalUI)
```

## Assets/Scripts\RPG\UI\QuestJournalUI.cs
```csharp
namespace Game.RPG
    public class QuestJournalUI : MonoBehaviour
        [Header("References")]
        [SerializeField] private GameObject journalPanel;
        [SerializeField] private Transform questListParent;
        [SerializeField] private GameObject questEntryPrefab;

        [Header("Details")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI objectivesText;

        public void RefreshList()
        public void ShowDetails(ActiveQuest activeQuest)
```

## Assets/Scripts\RPG\UI\QuickSlotUI.cs
```csharp
namespace Game.RPG
    public class QuickSlotUI : MonoBehaviour, IPointerClickHandler, IDropHandler
        [SerializeField] private int slotIndex;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Outline borderImage;

        public void UpdateUI()
        public void OnPointerClick(PointerEventData eventData)
        public void OnDrop(PointerEventData eventData)
```

## Assets/Scripts\RPG\UI\TooltipManager.cs
```csharp
namespace Game.RPG
    public class TooltipManager : MonoBehaviour
        public static TooltipManager Instance { get; private set; }
        [SerializeField] private GameObject tooltipPanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI statsText;

        public void ShowTooltip(ItemDataSO item, Vector2 position)
        public void HideTooltip()
```

## Assets/Scripts\RPG\UI\TradeSlotUI.cs
```csharp
namespace Game.RPG
    public class TradeSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI priceText;

        public PlayerInventory.InventorySlot Slot { get; private set; }
        public bool IsMerchantSlot { get; private set; }
        public void Setup(PlayerInventory.InventorySlot slot, bool isMerchant, int price, TradeUI tradeUI)
        public void OnPointerClick(PointerEventData eventData)
        public void OnPointerEnter(PointerEventData eventData)
        public void OnPointerExit(PointerEventData eventData)
```

## Assets/Scripts\RPG\UI\TradeUI.cs
```csharp
namespace Game.RPG
    public class TradeUI : MonoBehaviour
        public static TradeUI Instance { get; private set; }
        public bool IsOpen { get; private set; } = false; // 🛠 ДОБАВЛЕНО
        [Header("Main Panel")]
        [SerializeField] private GameObject tradePanel;
        [SerializeField] private GameObject characterPanel;

        [Header("Player Side")]
        [SerializeField] private Transform playerGrid;
        [SerializeField] private GameObject tradeSlotPrefab;

        [Header("Merchant Side")]
        [SerializeField] private Transform merchantGrid;
        [SerializeField] private TextMeshProUGUI merchantGoldText;

        [Header("Confirmation Popup")]
        [SerializeField] private GameObject confirmPanel;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        public void OpenTrade(MerchantInventory merchant)
        public void CloseTrade()
        public void TryBuy(PlayerInventory.InventorySlot slot)
        public void TrySell(PlayerInventory.InventorySlot slot)
```

## Assets/Scripts\RPG\XpCurveSO.cs
```csharp
namespace Game.RPG
    [CreateAssetMenu(fileName = "XpCurve_", menuName = "RPG/XP Curve")]
    public class XpCurveSO : ScriptableObject
        [Tooltip("X = уровень, Y = XP для перехода на следующий уровень. " +
                 "Дефолт аппроксимирует 100 × level^1.5.")]
        public AnimationCurve Curve = AnimationCurve.Constant(1, 20, 100);
        [Tooltip("Глобальный множитель поверх кривой. Для тюнинга «громкости» XP.")]
        [Range(0.1f, 10f)]
        public float Multiplier = 1f;
        [Range(2, 50)]
        public int MaxLevel = 20;
        public int GetXpForLevel(int level)
        [ContextMenu("Reset to default curve")]
        private void ResetToDefault()
```

<!-- Всего строк: 5532, символов: 58068 (~14517 токенов) -->
