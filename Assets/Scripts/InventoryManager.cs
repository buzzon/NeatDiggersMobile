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

    void Start()
    {
        GameHub.OnUpdateRoom += UpdateRoom;
        GameHub.OnInitRoom += InitRoom;
    }

    private void InitRoom(Room room)
    {
        Player player = room.Players.Find(p => p.Id == GameHub.User.Id);
        Player.text = $"{player.Name}  ({player.Character.Title})";

        foreach (var ability in player.Character.Abilities)
        {
            Button ab = Instantiate(AbilityButton, Abilities.transform);
            ab.interactable = ability.IsActive;
        }
    }

    private void UpdateRoom(Room room, GameAction action)
    {
        Player player = room.Players.Find(p => p.Id == GameHub.User.Id);

        Health.text = $"{player.Health}/{player.Character.MaxHealth}";
        Level.text = $"Уровень: {player.Level}";
        Score.text = $"Счёт: {player.Score}";
        string wt = player.Character.WeaponType == NeatDiggers.GameServer.WeaponType.Melee ? "Ближний" : "Дальнее";
        WeaponType.text = $"Оружие: {wt}";

        for (int i = 0; i < player.Character.Abilities.Count; i++)
        {
            Abilities.transform.GetChild(i).GetComponent<Button>().interactable = player.Character.Abilities[i].IsActive;
            Abilities.transform.GetChild(i).GetComponentInChildren<Text>().text = player.Character.Abilities[i].Description;
        }
    }
}
