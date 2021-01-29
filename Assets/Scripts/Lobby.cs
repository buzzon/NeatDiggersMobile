using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using NeatDiggers.GameServer;
using Microsoft.AspNetCore.SignalR.Client;
using NeatDiggers.GameServer.Characters;

public class Lobby : MonoBehaviour
{
    public Button StartGameButton;
    public Button ReadyButton;
    public Button ConnectButton;

    public Button buttonPrefab;
    public Text CharacterDescriptionText;
    public Text PlayersStateText;

    public GameObject StartPanel;
    public GameObject LobbyPanel;
    public GameObject SelectCharacterPanel;
    public GameObject LobbyPLayersPanel;
    public GameObject InterfacePanel;

    public InputField InputName;
    public InputField InputCode;

    // Start is called before the first frame update
    void Start()
    {
        WWW magic = new WWW("magic"); // Самая ценная строчка кода <3
        GameHub.OnConected += OnConected;

        LobbyPanel.SetActive(false);
        LobbyPLayersPanel.SetActive(false);
        InterfacePanel.SetActive(false);
        StartGameButton.interactable = false;
        ReadyButton.interactable = false;
        ConnectButton.interactable = false;
    }

    void OnConected()
    {
        StartPanel.SetActive(true);
        SelectCharacterPanel.SetActive(true);
        ConnectButton.interactable = true;
    }

    public async void ConnectToRoom()
    {
        ConnectButton.interactable = false;

        var room = await GameHub.ConnectToRoom(InputName.text, InputCode.text);

        if (room != null)
        {
            GameHub.InitRoom(room);
            GameHub.OnUpdateRoom += UpdateRoom;
            ShowSelectCharacterPanel();
        }
        ConnectButton.interactable = true;
    }

    public void UpdateRoom(Room room, GameAction action)
    {
        if (room.IsStarted)
        {
            StartPanel.SetActive(false);
            LobbyPanel.SetActive(false);
            InterfacePanel.SetActive(true);
            GameHub.OnUpdateRoom -= UpdateRoom;
        }
        else
        {
            LoadPlayersState(room.Players);
            StartGameButton.interactable = room.IsCanStart();
        }
    }

    private void LoadPlayersState(List<Player> players)
    {
        PlayersStateText.text = "";
        for (int i = 0; i < players.Count; i++)
        {
            string isReady = players[i].IsReady ? "Готов" : "Не готов";
            PlayersStateText.text += $"{players[i].Name} ({players[i].Character.Title}) - {isReady}\n";
        }
    }

    private void ShowSelectCharacterPanel()
    {
        StartPanel.SetActive(false);
        LobbyPanel.SetActive(true);

        var characters = Enum.GetNames(typeof(CharacterName));
        for (int i = 1; i < characters.Length; i++)
        {
            Button button = Instantiate(buttonPrefab);
            button.transform.SetParent(SelectCharacterPanel.transform);
            button.GetComponentInChildren<Text>().text = characters[i];
            var pos = button.GetComponent<RectTransform>().anchoredPosition;
            button.GetComponent<RectTransform>().position = new Vector3(-pos.x, -pos.y - i * 150, 0);
            CharacterName name = (CharacterName)i;
            button.onClick.AddListener(() => SelectCharacter(name));
        }
    }

    public async void SelectCharacter(CharacterName name)
    {
        ReadyButton.interactable = true;
        var character = await GameHub.ChangeCharacter(name);
        CharacterDescriptionText.text =
            $"{character.Title}\n" +
            $"Здоровье: {character.MaxHealth}\n" +
            $"Тип оружия: {character.WeaponType}\n\n" +
            $"Способности:\n";
        for (int i = 0; i < character.Abilities.Count; i++)
            CharacterDescriptionText.text += $"[{i}] {character.Abilities[i].Description} \n";
    }

    public async void ChangeReady()
    {
        await GameHub.ChangeReady();

        LobbyPLayersPanel.SetActive(SelectCharacterPanel.activeSelf);
        SelectCharacterPanel.SetActive(!SelectCharacterPanel.activeSelf);
        ReadyButton.GetComponentInChildren<Text>().text = SelectCharacterPanel.activeSelf ? "Готов" : "Не готов";
    }

    public async void StartGame() => await GameHub.StartGame();
}
