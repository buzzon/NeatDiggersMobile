using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using NeatDiggers.GameServer;
using Microsoft.AspNetCore.SignalR.Client;

public class Lobby : MonoBehaviour
{
    public InputField InputName;
    public InputField InputCode;

    private HubConnection connection;

    // Start is called before the first frame update
    async void Start()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("https://neat-diggers.fun/GameHub")
            .Build();
        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public async void Connect()
    {
        var s = await connection.InvokeAsync<Room>("ConnectToRoomAsSpectator", InputCode.text);
    }
}
