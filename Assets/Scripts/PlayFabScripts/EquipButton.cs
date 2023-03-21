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
            Debug.Log("Clicked Equip");
            if (ShopManager.EQUIP != currentShopItem.title)
            {
                FindObjectOfType<ShopManager>().EquipSkin(currentShopItem.title);
                buttonText.text = "Unequip";
                var otherButtons = FindObjectsOfType<EquipButton>();
                foreach (var equipButton in otherButtons)
                {
                    if (equipButton != this)
                    {
                        equipButton.ManualUnequip();
                    }
                }
            }
            else
            {
                FindObjectOfType<ShopManager>().UnequipSkin();
                buttonText.text = "Equip";
            }
        }

        public void ManualUnequip()
        {
            buttonText.text = "Equip";
        }
    }
}
