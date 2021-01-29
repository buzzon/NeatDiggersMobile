using NeatDiggers.GameServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Text Player;
    public Text Health;
    public Text Level;
    public Text Score;
    public Text WeaponType;

    public GameObject Abilities;
    public Button AbilityButton;

    public GameObject Effects;
    public Text EffectsText;

    public GameObject Items;
    public Button ItemsButton;

    public Text LeftWeapon;
    public Text RightWeapon;
    public Text Armor;


    void Start()
    {
        GameHub.OnUpdateRoom += UpdateRoom;
    }

    private bool isAbilitiesCreated;
    private List<Button> abilitiesButtons = new List<Button>();
    private List<Text> effectsTexts = new List<Text>();
    private List<Button> itemsButtons = new List<Button>();
    private void UpdateRoom(Room room, GameAction action)
    {
        Player player = room.Players.Find(p => p.Id == GameHub.User.Id);
        Player.text = $"{player.Name}  ({player.Character.Title})";

        Health.text = $"{player.Health}/{player.Character.MaxHealth}";
        Level.text = $"Уровень: {player.Level}";
        Score.text = $"Счёт: {player.Score}";
        string wt = player.Character.WeaponType == NeatDiggers.GameServer.WeaponType.Melee ? "Ближний" : "Дальнее";
        WeaponType.text = $"Оружие: {wt}";

        if (!isAbilitiesCreated)
        {
            foreach (var ability in player.Character.Abilities)
            {
                Button ab = Instantiate(AbilityButton, Abilities.transform);
                ab.interactable = ability.IsActive;
                ab.GetComponentInChildren<Text>().text = ability.Description;
                abilitiesButtons.Add(ab);
            }
            isAbilitiesCreated = true;
        }
        else
            for (int i = 0; i < abilitiesButtons.Count; i++)
            {
                abilitiesButtons[i].interactable = player.Character.Abilities[i].IsActive && 
                    player.Character.Abilities[i].Type != NeatDiggers.GameServer.Abilities.AbilityType.Passive;
                abilitiesButtons[i].GetComponent<AbilityHandler>().Ability = player.Character.Abilities[i];
            }
                

        while (effectsTexts.Count < player.Effects.Count)
            effectsTexts.Add(Instantiate(EffectsText, Effects.transform));
        while (effectsTexts.Count > player.Effects.Count)
            Destroy(effectsTexts[0]);
        for (int i = 0; i < player.Effects.Count; i++)
        {
            string dur = player.Effects[i].Duration > 0 ? $" ({player.Effects[i].Duration})" : "";
            effectsTexts[i].text = player.Effects[i].Title + dur;
        }


        while (itemsButtons.Count < player.Inventory.Items.Count)
            itemsButtons.Add(Instantiate(ItemsButton, Items.transform));
        while (itemsButtons.Count > player.Inventory.Items.Count)
            Destroy(itemsButtons[0]);
        for (int i = 0; i < player.Inventory.Items.Count; i++)
        {
            itemsButtons[i].GetComponentInChildren<Text>().text = $"{player.Inventory.Items[i].Title}";
            itemsButtons[i].GetComponent<ItemHandler>().Item = player.Inventory.Items[i];
            itemsButtons[i].GetComponent<ItemHandler>().Inventory = player.Inventory;
        }

        LeftWeapon.text = $"Левая рука: {player.Inventory.LeftWeapon.Title}";
        RightWeapon.text = $"Правая рука: {player.Inventory.RightWeapon.Title}";
        Armor.text = $"Броня: {player.Inventory.Armor.Title}";
    }
}
