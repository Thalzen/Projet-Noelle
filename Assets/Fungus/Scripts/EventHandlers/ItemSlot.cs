using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;
    private Inventory _inventory;

    public Image image;
    private TextMeshProUGUI textBox;

    private Choices _choices;
    private Noelle _noelle;

    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();

        textBox = GetComponentInChildren<TextMeshProUGUI>();

        _choices = FindObjectOfType<Choices>();
        _noelle = FindObjectOfType<Noelle>();
    }
    
    void Update()
    {
        
    }

    public void DisplayItems(InventoryItem thisItem)
    {
        item = thisItem;
        textBox.text = item.itemName;
        image.sprite = item.itemIcon;
        gameObject.SetActive(true);
    }

    public void ClearItems()
    {
        item = null;
        image.sprite = null;
        textBox.text = null;
        gameObject.SetActive(false);
    }

    public void OnItemClick()
    {
        if (_noelle.cutSceneInProgress)
        {
            return;
        }

        if (_choices.choice == Choices.Action.Use && _choices._currentItem != null)
        {
            _inventory.CombineItems(_choices._currentItem, item);
        }
        _choices.choice = Choices.Action.Use;
        _choices._currentItem = item;
        _choices.UpdateChoiceTextBox(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _choices.hoveredItemSlot = item.itemName;
        _choices.UpdateChoiceTextBox(null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _choices.hoveredItemSlot = null;
        _choices.UpdateChoiceTextBox(null);
    }
}
