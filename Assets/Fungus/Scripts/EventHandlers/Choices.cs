using System;
using UnityEngine;
using TMPro;
using Fungus;

public class Choices : MonoBehaviour
{
    public string walkString = "Walk to ";
    public string useString = "Use ";

    public string currentClickable;
    public InventoryItem _currentItem;
    public string hoveredItemSlot;
    public bool combinability;

    public Inventory _inventory;

    public enum Action {Walk, Use };
    public Action choice = Action.Walk;
    private TextMeshProUGUI choiceTextBox;
    private Flowchart[] flowcharts;

    private void Start()
    {
        choiceTextBox = GetComponentInChildren<TextMeshProUGUI>();
        choiceTextBox.text = "";
        flowcharts = FindObjectsOfType<Flowchart>();
        _inventory = FindObjectOfType<Inventory>();
    }

    public void UpdateChoiceTextBox(string currentClickable)
    {
        SetChoiceInFlowchart();
        if (choice == Action.Walk)
        {
            combinability = false;
            choiceTextBox.text = walkString + currentClickable;
        }
        else if (choice == Action.Use)
        {
            if (_inventory.canvasGroup.interactable == true)
            {
                combinability = true;
                choiceTextBox.text = useString + _currentItem.itemName + " with " + hoveredItemSlot;
            }
            else if (currentClickable == null)
            {
                choiceTextBox.text = useString + _currentItem.itemName + " with ";
            }
            else
            {
                combinability = false;
                choiceTextBox.text = useString + _currentItem.itemName + " with " + currentClickable;
            }
            
        }
        

    }

    private void SetChoiceInFlowchart()
    {
        foreach (Flowchart flowchart in flowcharts)
        {
            if (flowchart.HasVariable("choice"))
            {
                flowchart.SetStringVariable("choice",choice.ToString());
            }

            if (_currentItem == null)
            {
                return;
            }

            if (flowchart.HasVariable("currentItem"))
            {
                flowchart.SetStringVariable("currentItem", _currentItem.itemName);
            }
        }
    }
}
