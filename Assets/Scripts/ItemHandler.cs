using NeatDiggers.GameServer;
using NeatDiggers.GameServer.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour
{
    public GameObject DescriptionPanel;
    public GameObject UseItemButtons;
    public GameObject TargetPrefab;
    private GameObject target;
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
                dph.UseAction = () => Use(dph);
                break;
            case ItemType.Weapon:
                switch (Item.WeaponHanded)
                {
                    case WeaponHanded.One:
                        dph.UseLeftAction = () => EuquipLeft(dph);
                        dph.UseRightAction = () => EuquipRight(dph);
                        break;
                    case WeaponHanded.Two:
                        dph.UseTwoAction = () => EuquipTwo(dph);
                        break;
                    default:
                        break;
                }
                break;
            case ItemType.Armor:
                dph.EquipAction = () => Euquip(dph);
                break;
            default:
                break;
        }

        dph.DrawButtons();
    }

    private async void Use(DescriptionPanelHandler dph)
    {
        IsAlive = false;
        switch (Item.Target)
        {
            case Target.None:
                GameAction action = new GameAction() { Type = GameActionType.UseItem,  Item = Item };
                bool success = await GameHub.DoAction(action);
                dph.Cancel();
                if (success)
                    Destroy(transform.gameObject);
                break;
            case Target.Player:
                break;
            case Target.Position:
                target = Instantiate(TargetPrefab);
                target.GetComponent<TargetControl>().StartListen(SetTargetPosition);
                dph.Cancel();
                GameObject.Find("PanelInventory").SetActive(false);
                GameObject useItemButtons = Instantiate(UseItemButtons);
                Transform use = useItemButtons.transform.Find("ButtonUseEnd");
                use.gameObject.GetComponent<Button>().onClick.AddListener(() => UseOnPositionEnd(false, useItemButtons));
                Transform cancel = useItemButtons.transform.Find("ButtonMoveCancel");
                cancel.gameObject.GetComponent<Button>().onClick.AddListener(() => UseOnPositionEnd(true, useItemButtons));
                break;
            default:
                break;
        }
    }


    private Vector targetPosition;
    private void SetTargetPosition(Vector pos) => targetPosition = pos;

    public async void UseOnPositionEnd(bool cancel, GameObject useItemButtons)
    {
        Destroy(useItemButtons);
        Destroy(target);
        GameObject.Find("PanelInventory").SetActive(true);
        if (cancel) return;
        GameAction action = new GameAction() { Type = GameActionType.UseItem, Item = Item, TargetPosition = targetPosition };
        bool success = await GameHub.DoAction(action);
        if (success)
            Destroy(transform.gameObject);
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

    private async void EuquipLeft(DescriptionPanelHandler dph)
    {
        IsAlive = false;
        Item itemBackUp = Inventory.LeftWeapon;
        if (Inventory.LeftWeapon.Name != ItemName.Empty)
            Inventory.Items.Add(Inventory.LeftWeapon);
        Inventory.LeftWeapon = Item;
        Item it = Inventory.Items.Find(i => i.Name == Item.Name);
        Inventory.Items.Remove(it);
        bool success = await GameHub.ChangeInventory(Inventory);
        dph.Cancel();
        if (!success)
        {
            IsAlive = true;
            Inventory.LeftWeapon = itemBackUp;
            Inventory.Items.Remove(itemBackUp);
            Inventory.Items.Add(Item);
        }
        else
            Destroy(transform.gameObject);
    }

    private async void EuquipRight(DescriptionPanelHandler dph)
    {
        IsAlive = false;
        Item itemBackUp = Inventory.RightWeapon;
        Item leftBackUp = Inventory.LeftWeapon;
        if (Inventory.LeftWeapon.WeaponHanded == WeaponHanded.Two)
        {
            Inventory.Items.Add(Inventory.LeftWeapon);
            Inventory.LeftWeapon = new Item { Name = ItemName.Empty };
        }

        if (Inventory.RightWeapon.Name != ItemName.Empty)
            Inventory.Items.Add(Inventory.RightWeapon);

        Inventory.RightWeapon = Item;
        Item it = Inventory.Items.Find(i => i.Name == Item.Name);
        Inventory.Items.Remove(it);
        bool success = await GameHub.ChangeInventory(Inventory);
        dph.Cancel();
        if (!success)
        {
            IsAlive = true;
            Inventory.RightWeapon = itemBackUp;
            Inventory.LeftWeapon = leftBackUp;
            Inventory.Items.Remove(itemBackUp);
            Inventory.Items.Add(Item);
        }
        else
            Destroy(transform.gameObject);
    }

    private async void EuquipTwo(DescriptionPanelHandler dph)
    {
        IsAlive = false;
        Item itemBackUp = Inventory.LeftWeapon;
        Item rightBackUp = Inventory.RightWeapon;
        if (Inventory.LeftWeapon.Name != ItemName.Empty)
            Inventory.Items.Add(Inventory.LeftWeapon);
        if (Inventory.RightWeapon.Name != ItemName.Empty)
        {
            Inventory.Items.Add(Inventory.LeftWeapon);
            Inventory.RightWeapon = new Item { Name = ItemName.Empty };
        }

        Inventory.LeftWeapon = Item;
        Item it = Inventory.Items.Find(i => i.Name == Item.Name);
        Inventory.Items.Remove(it);
        bool success = await GameHub.ChangeInventory(Inventory);
        dph.Cancel();
        if (!success)
        {
            IsAlive = true;
            Inventory.LeftWeapon = itemBackUp;
            Inventory.RightWeapon = rightBackUp;
            Inventory.Items.Remove(itemBackUp);
            Inventory.Items.Add(Item);
        }
        else
            Destroy(transform.gameObject);
    }
}
