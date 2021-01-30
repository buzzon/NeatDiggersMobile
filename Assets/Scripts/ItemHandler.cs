using NeatDiggers.GameServer;
using NeatDiggers.GameServer.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public GameObject DescriptionPanel;
    public Item Item { get; set; }

    public bool IsAlive { get; set; } = true;

    public Inventory Inventory { get; set; }

    public void OnClick()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject panel = Instantiate(DescriptionPanel, canvas.transform);
        DescriptionPanelHandler dph = panel.GetComponent<DescriptionPanelHandler>();
        dph.Title = Item.Title;
        dph.Description = Item.Description;

        switch (Item.Type)
        {
            case ItemType.Active:
                break;
            case ItemType.Weapon:
                break;
            case ItemType.Armor:
                dph.EquipAction = () => Euquip(dph);
                break;
            default:
                break;
        }

        dph.DrawButtons();
    }

    private async void Euquip(DescriptionPanelHandler dph)
    {
        IsAlive = false;
        Item armorBackUp = Inventory.Armor;
        if (Inventory.Armor.Name != ItemName.Empty)
            Inventory.Items.Add(Inventory.Armor);
        Inventory.Armor = Item;
        Item it = Inventory.Items.Find(i => i.Name == Item.Name);
        Inventory.Items.Remove(it);
        bool success = await GameHub.ChangeInventory(Inventory);
        dph.Cancel();
        if (!success)
        {
            IsAlive = true;
            Inventory.Armor = armorBackUp;
            Inventory.Items.Remove(armorBackUp);
            Inventory.Items.Add(Item);
        }
        else
            Destroy(transform.gameObject);
    }
}
