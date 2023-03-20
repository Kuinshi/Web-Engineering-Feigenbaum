using TMPro;
using UnityEngine;

namespace PlayFabScripts
{
    public class EquipButton : MonoBehaviour
    {
        public ShopItemSO currentShopItem;
        public GameObject button;
        [SerializeField] private TextMeshProUGUI buttonText;
        
        
        public void ClickEquip()
        {
            if (ShopManager.EQUIP != currentShopItem.title)
            {
                FindObjectOfType<ShopManager>().EquipSkin(currentShopItem.title);
                buttonText.text = "Unequip";
                // NEed to unequip other skins...
            }
            else
            {
                FindObjectOfType<ShopManager>().UnequipSkin();
                buttonText.text = "Equip";
            }
        }
    }
}
