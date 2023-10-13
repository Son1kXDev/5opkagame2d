using UnityEngine;
using Enjine.Data.InventorySystem;

namespace Enjine
{

    [CreateAssetMenu(fileName = "InventoryItemData", menuName = "Resources/Game/Item Data", order = 0)]
    public class InventoryItemData : ScriptableObject, IInventoryItemInfo
    {
        [field: SerializeField, StatusIcon("")] public string ID { get; private set; }

        [field: SerializeField, StatusIcon("")] public string Title { get; private set; }

        [field: SerializeField, StatusIcon("")] public string Description { get; private set; }

        [field: SerializeField, StatusIcon(minValue: 1)] public int StackSize { get; private set; }

        [field: SerializeField, StatusIcon] public Sprite Icon { get; private set; }
    }
}
