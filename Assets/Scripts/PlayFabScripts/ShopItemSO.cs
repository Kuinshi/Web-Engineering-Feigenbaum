using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "shopMenu",menuName = "ScriptableObjects/New Shop Item", order = 1)]
public class ShopItemSO : ScriptableObject
{
   public int cosmeticId;
   
   public string title;
   public Texture2D previewImage;
   public string description;
   public int baseCost;
}
