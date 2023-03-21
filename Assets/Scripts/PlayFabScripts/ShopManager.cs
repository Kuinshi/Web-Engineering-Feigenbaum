using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public int coins;
    public TMP_Text coinUi;
    public ShopItemSO[] shopItemsS0;
    public GameObject[] shopPanelsGO;
    public Button[] myPurchaseBtns;
    public TMP_Text usernameTxt;
    private string playerId;
    public ShopTemplate[] shopPanels;

    public static string EQUIP;
    private Dictionary<string, bool> purchasedSkins = new Dictionary<string, bool>();
    private bool receivedUserData;
    
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
        SetUserData();

        while (!receivedUserData)
        {
            yield return null;
            Debug.Log("Waiting for user Data");
        }
        
        LoadPanels();
        CheckPurchaseable();
        EQUIP = GetEquippedSkin();
        CheckEquipped();
    }

    private void CheckEquipped()
    {
        foreach (var shopPanel in shopPanelsGO)
        {
            if(!shopPanel.activeInHierarchy)
                continue;

            var shopTemplate = shopPanel.GetComponent<ShopTemplate>();
            if (AlreadyPurchased(shopTemplate.equipButton.currentShopItem.title))
            {
                shopTemplate.equipButton.button.SetActive(true);

                if (EQUIP == shopTemplate.equipButton.currentShopItem.title)
                {
                    var textField = shopTemplate.equipButton.button.GetComponentInChildren<TextMeshProUGUI>();
                    textField.text = "Unequip";
                }
            }
        }
    }

    void SetUserData()
    { 
        usernameTxt.SetText(PlayerPrefs.GetString("USERNAME"));
        playerId = PlayerPrefs.GetString("PLAYERID");
        GetCoins();
        GetBoughtSkins();
    }

    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(2);
    }

    //function to add coins
    public void AddCoins()
    {
        coins++;
        coinUi.text = "Coins: " + coins.ToString();
        setCoins(coins);
        CheckPurchaseable();
    }

    public void setCoins(int newCoins)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"coins",newCoins.ToString()}
            }
        }, SetDataSuccess,SetDataFailure);
    }
    
    private void GetCoins()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = playerId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data.ContainsKey("coins"))
            {
                coins = Int32.Parse(result.Data["coins"].Value);
                coinUi.text = "Coins: " + coins;
                CheckPurchaseable();
            }
            else
            {
                setCoins(0);
                coins = 0;
                coinUi.text = "Coins: 0";
                CheckPurchaseable();
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    private void GetBoughtSkins()
    {
        foreach (var shopItem in shopItemsS0)
        {
            purchasedSkins.Add(shopItem.title, false);
        }
        
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = playerId,
            Keys = null
        }, result => {

            
            foreach (var kvp in result.Data)
            {
                if (purchasedSkins.ContainsKey(kvp.Key))
                    purchasedSkins[kvp.Key] = true;
            }
            ReceivedUserData();

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
        
    }

    private void ReceivedUserData()
    {
        receivedUserData = true;
    }

    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            if (coins >= shopItemsS0[i].baseCost && !AlreadyPurchased(shopItemsS0[i].title))
            {
                myPurchaseBtns[i].interactable = true;
            }
            else
            {
                myPurchaseBtns[i].interactable = false;
            }

            if (AlreadyPurchased(shopItemsS0[i].title))
            {
                shopPanels[i].equipButton.button.SetActive(true);
                Debug.Log("Activated equip button");
            }
        }
    }

    public void PurchaseItem(int btnNr)
    {
        Debug.Log("Button Number is " + btnNr);
        if (coins >= shopItemsS0[btnNr].baseCost)
        {
            setCoins(coins- shopItemsS0[btnNr].baseCost);
            coins = coins - shopItemsS0[btnNr].baseCost;
            coinUi.text = "Coins: " + coins.ToString();
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>()
                {
                    {shopItemsS0[btnNr].title,shopItemsS0[btnNr].description}
                }
            }, SetDataSuccess,SetDataFailure);
            
            if (purchasedSkins.ContainsKey(shopItemsS0[btnNr].title))
            {
                purchasedSkins[shopItemsS0[btnNr].title] = true;
            }
            
            CheckPurchaseable();
        }
    }

    private bool AlreadyPurchased(string skin)
    {
        Debug.Log("Checking Purchase Statss for " + skin);
        if (purchasedSkins.ContainsKey(skin))
        {
            Debug.Log("PURCHASED");
            return purchasedSkins[skin];
        }
        Debug.Log("NOT PURCHASED");
        return false;
    }

    public void EquipSkin(string skinTitle)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"equippedSkin", skinTitle}
            }
        }, SetDataSuccess,SetDataFailure);

        EQUIP = skinTitle;
    }

    public void UnequipSkin()
    {
        EquipSkin(String.Empty);
    }

    private string GetEquippedSkin()
    {
        string equipped = String.Empty;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = playerId,
            Keys = null
        }, result => {
            if (result.Data.ContainsKey("equippedSkin"))
            { 
                equipped = result.Data["equippedSkin"].Value;
                for (int i = 0; i < shopItemsS0.Length; i++)
                {
                    if (shopItemsS0[i].title == equipped)
                    {
                        shopPanels[i].equipButton.button.SetActive(true);
                        shopPanels[i].equipButton.button.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
                    }
                }
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });

        return equipped;
    }
    
    //fill the panels with item info
    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsS0[i].title;
            shopPanels[i].descriptionTxt.text = shopItemsS0[i].description;
            shopPanels[i].costTxt.text = shopItemsS0[i].baseCost.ToString();
            shopPanels[i].previewImage.texture = shopItemsS0[i].previewImage;
            shopPanels[i].equipButton.currentShopItem = shopItemsS0[i];
        }
    }
    void SetDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log(result.DataVersion);
    }
    void SetDataFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
}
