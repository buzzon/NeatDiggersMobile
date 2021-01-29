using Microsoft.AspNetCore.SignalR.Client;
using NeatDiggers.GameServer;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameControls : MonoBehaviour
{
    public Toggle toggleRotation;

    public Text DiceCount;

    public Button ButtonDig;
    public Button ButtonRolDice;
    public Button ButtonTakeFlag;
    public Button ButtonMove;
    public Button ButtonEndTurn;
    public GameObject Interface;

    private int diceValue;
    public Player Player { get; set; }

    private void Start()
    {
        GameHub.OnUpdateRoom += UpdateRoom;
        GameHub.OnInitRoom += (room) => UpdateRoom(room, null);
    }

    private void UpdateRoom(Room room, GameAction action)
    {
        Interface.SetActive(room.Players[room.PlayerTurn].Id == GameHub.User.Id);
    }

    public void EndTurn()
    {
        GameHub.EndTurn();
    }

    public async void RollDice()
    {
        diceValue = await GameHub.RollTheDice();
        DiceCount.text = diceValue.ToString();
        await Task.Delay(3000);
        DiceCount.text = "";
    }

    public async void Dig()
    {
        GameAction action = new GameAction { Type = GameActionType.Dig, TargetPosition = Player.Position };
        await GameHub.DoAction(action);
    }

    public void Move()
    {
        toggleRotation.isOn = false;
        //on touch move show target
        //on touch end show hint move
    }
}
