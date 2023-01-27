using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopManager : MonoBehaviour
{

    public int coins;
    public TMP_Text coinUi;
    public ShopItemSO[] shopItemsS0;
    public GameObject[] shopPanelsGO;

    public ShopTemplate[] shopPanels;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
        coinUi.text = "Coins: " + coins.ToString();
        LoadPanels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function to add coins
    public void AddCoins()
    {
        coins++;
        coinUi.text = "Coins: " + coins.ToString();
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsS0[i].title;
            shopPanels[i].descriptionTxt.text = shopItemsS0[i].description;
            shopPanels[i].costTxt.text = shopItemsS0[i].baseCost.ToString();
        }
    }
}
