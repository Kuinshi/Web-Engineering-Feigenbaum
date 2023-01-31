using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
        SetUserData();
        LoadPanels();
        CheckPurchaseable();
    }

    void SetUserData()
    { 
        usernameTxt.SetText(PlayerPrefs.GetString("USERNAME"));
        playerId = PlayerPrefs.GetString("PLAYERID");
        GetCoins();
        
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

    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            if (coins >= shopItemsS0[i].baseCost)
            {
                myPurchaseBtns[i].interactable = true;
            }
            else
            {
                myPurchaseBtns[i].interactable = false;
            }
        }
    }

    public void PurchaseItem(int btnNr)
    {
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
            CheckPurchaseable();
            
        }
    }
    
    //fill the panels with item info
    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsS0.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsS0[i].title;
            shopPanels[i].descriptionTxt.text = shopItemsS0[i].description;
            shopPanels[i].costTxt.text = shopItemsS0[i].baseCost.ToString();
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
