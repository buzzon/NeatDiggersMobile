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

    public InputField InputName;
    public InputField InputCode;

    private HubConnection connection;
    private GameMaster gameMaster;

    // Start is called before the first frame update
    async void Start()
    {
        WWW magic = new WWW("magic"); // Самая ценная строчка кода <3
        gameMaster = gameObject.GetComponent<GameMaster>();

        StartPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        SelectCharacterPanel.SetActive(true);
        LobbyPLayersPanel.SetActive(false);

        StartGameButton.interactable = false;
        ReadyButton.interactable = false;
        ConnectButton.interactable = false;

        connection = new HubConnectionBuilder()
            .WithUrl("https://neat-diggers.fun/GameHub")
            .Build();
        try
        {
            await connection.StartAsync();
            ConnectButton.interactable = true;

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            StartPanel.SetActive(false);
        }
    }

    public async void Connect()
    {
        ConnectButton.interactable = false;
        var token = await connection.InvokeAsync<string>("GetToken", InputCode.text);
        User user = new User { Name = InputName.text, Code = InputCode.text, Token = token };
        var room = await connection.InvokeAsync<Room>("ConnectToRoom", user);

        if (room != null)
        {
            ShowSelectCharacterPanel();
            connection.On<Room>("ChangeState", (rm) => UpdateRoom(rm, null));
            connection.On<Room, GameAction>("ChangeStateWithAction", (rm, action) => UpdateRoom(rm, action));
            var map = await connection.InvokeAsync<GameMap>("GetGameMap");
            gameMaster.DrawMap(map);
        }
        ConnectButton.interactable = true;
    }

    public void UpdateRoom(Room room, GameAction action)
    {
        if (room.IsStarted)
        {
            StartPanel.SetActive(false);
            LobbyPanel.SetActive(false);
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
            var name = i;
            button.onClick.AddListener(() => SelectCharacter(name));
        }
    }

    public async void SelectCharacter(int name)
    {
        ReadyButton.interactable = true;
        var character = await connection.InvokeAsync<Character>("ChangeCharacter", name);
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
        await connection.InvokeAsync("ChangeReady");

        LobbyPLayersPanel.SetActive(SelectCharacterPanel.activeSelf);
        SelectCharacterPanel.SetActive(!SelectCharacterPanel.activeSelf);
        ReadyButton.GetComponentInChildren<Text>().text = SelectCharacterPanel.activeSelf ? "Готов" : "Не готов";
    }

    public async void StartGame()
    {
        await connection.InvokeAsync("StartGame");
    }
}
