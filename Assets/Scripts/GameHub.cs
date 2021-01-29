﻿using Microsoft.AspNetCore.SignalR.Client;
using NeatDiggers.GameServer;
using NeatDiggers.GameServer.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class GameHub
{
    public static User User;
    public static event Action<Room, GameAction> OnUpdateRoom;
    public static event Action<Room> OnInitRoom;
    public static event Action OnConected;

    private static readonly HubConnection connection;

    static GameHub()
    {
        User = null;
        connection = new HubConnectionBuilder()
            .WithUrl("https://neat-diggers.fun/GameHub")
            .Build();

        connection.Closed += (error) => Task.Run(() => Connect());
        connection.On<Room>("ChangeState", (room) => OnUpdateRoom?.Invoke(room, null));
        connection.On<Room, GameAction>("ChangeStateWithAction", (room, action) => OnUpdateRoom?.Invoke(room, action));
        Connect();
    }

    private static async void Connect()
    {
        bool isConected = false;

        while (!isConected)
        {
            try
            {
                await connection.StartAsync();
                OnConected?.Invoke();
                isConected = true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
    }

    public static async Task<bool> DoAction(GameAction action)
    {
        return await connection.InvokeAsync<bool>("DoAction", action);
    }

    public static async Task<User> Connect(string code)
    {
        return await connection.InvokeAsync<User>("Connect", code);
    }

    public static async Task<Character> ChangeCharacter(CharacterName name)
    {
        return await connection.InvokeAsync<Character>("ChangeCharacter", name);
    }

    public static async Task ChangeReady()
    {
        await connection.InvokeAsync("ChangeReady");
    }

    public static async Task StartGame()
    {
        await connection.InvokeAsync("StartGame");
    }

    public static async Task<Room> ConnectToRoom(string name, string code)
    {
        if (User == null)
        {
            User = await Connect(code);
            User.Name = name;
        }

        return await connection.InvokeAsync<Room>("ConnectToRoom", User);
    }

    public static async Task<GameMap> GetGameMap()
    {
        return await connection.InvokeAsync<GameMap>("GetGameMap");
    }

    public static void InitRoom(Room room) => OnInitRoom?.Invoke(room);
}
