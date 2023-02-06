using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System.Linq;

public class Inventory : MonoBehaviour
{
    private MenuDialog[] menuDialogs;
    private SayDialog[] sayDialogs;
    public CanvasGroup canvasGroup;
    private Noelle _noelle;
    

    public InventoryItem[] _inventoryItems;
    public ItemSlot[] _itemSlots;
    
    private Flowchart[] flowcharts;

    private static Inventory _inven;



    private void Start()
    {
        menuDialogs = FindObjectsOfType<MenuDialog>(); 
        sayDialogs = FindObjectsOfType<SayDialog>();
        canvasGroup = GetComponent<CanvasGroup>();
        _noelle = FindObjectOfType<Noelle>();
        flowcharts = FindObjectsOfType<Flowchart>();
        // if (_inven != null && _inven != this)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        // _inven = this;
        // DontDestroyOnLoad(this);
        

    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            ToggleInventory(!canvasGroup.interactable);
        }
    }

    private void ToggleInventory(bool setting)
    {
        ToggleCanvasGroup(canvasGroup, setting);
        InitializeItemSlots();

        if (!_noelle.cutSceneInProgress)
        {
            _noelle.InDialogue = setting;
        }

        foreach (MenuDialog menuDialog in menuDialogs)
        {
            ToggleCanvasGroup(menuDialog.GetComponent<CanvasGroup>(), !setting);
        }

        foreach (SayDialog sayDialog in sayDialogs)
        {
            sayDialog.dialogEnabled = !setting;
            if (setting)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            ToggleCanvasGroup(sayDialog.GetComponent<CanvasGroup>(), !setting);
        }
    }

    private void InitializeItemSlots()
    {
        List<InventoryItem> ownedItems = GetOwnedItems(_inventoryItems.ToList());
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (i < ownedItems.Count)
            {
                _itemSlots[i].DisplayItems(ownedItems[i]);
            }
            else
            {
                _itemSlots[i].ClearItems();
            }
            
        }
    }

    public List<InventoryItem> GetOwnedItems(List<InventoryItem> inventoryItems)
    {
        List<InventoryItem> ownedItems = new List<InventoryItem>();
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemOwned)
            {
                ownedItems.Add(item);
            }
            
        }

        return ownedItems;
    }

    private void ToggleCanvasGroup(CanvasGroup canvasGroup, bool setting)
    {
        canvasGroup.alpha = setting ? 1f : 0f;
        canvasGroup.interactable = setting;
        canvasGroup.blocksRaycasts = setting;
    }

    public void CombineItems(InventoryItem item1, InventoryItem item2)
    {
        if (item1.combinable == true && item2.combinable == true)
        {
            for (int i = 0; i < item1.combinableItems.Length; i++)
            {
                if (item1.combinableItems[i] == item2)
                {
                    foreach (Flowchart flowchart in flowcharts)
                    {
                        if (flowchart.HasBlock(item1.successBlockNames[i]))
                        {
                            ToggleInventory(false);
                            _noelle.EnterDialogue();
                            flowchart.ExecuteBlock(item1.successBlockNames[i]);
                            return;
                        }
                    }
                }
            }
        }

        foreach (Flowchart flowchart in flowcharts)
        {
            if (flowchart.HasBlock(item1.failBlockName))
            {
                ToggleInventory(false);
                _noelle.EnterDialogue();
                flowchart.ExecuteBlock(item1.failBlockName);
            }
        }
    }
}
