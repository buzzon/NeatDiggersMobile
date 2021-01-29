using Microsoft.AspNetCore.SignalR.Client;
using NeatDiggers.GameServer;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameControls : MonoBehaviour
{
    public GameObject target;

    public Toggle toggleRotation;

    public Text DiceCount;

    public Button ButtonDig;
    public Button ButtonRolDice;
    public Button ButtonTakeFlag;
    public Button ButtonMove;
    public Button ButtonEndTurn;
    public GameObject Interface;

    public GameObject MoveBasic;
    public GameObject MoveButtons;

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
        Player = room.Players.Find(p => p.Id == GameHub.User.Id);
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

    public async void TakeFlag()
    {
        GameAction action = new GameAction { Type = GameActionType.TakeTheFlag, TargetPosition = new Vector() };
        await GameHub.DoAction(action);
    }

    public void MoveBegin()
    {
        MoveBasic.SetActive(false);
        MoveButtons.SetActive(true);

        toggleRotation.isOn = false;
        target.GetComponent<TargetControl>().StartListen(SetTargetPosition);
        target.SetActive(true);
    }

    private Vector targetPosition;
    private void SetTargetPosition(Vector pos) => targetPosition = pos;


    public async void MoveEnd(bool cancel)
    {
        MoveBasic.SetActive(true);
        MoveButtons.SetActive(false);
        toggleRotation.isOn = true;

        target.SetActive(false);
        target.GetComponent<TargetControl>().StopListen();
        if (cancel) return;
        GameAction action = new GameAction { Type = GameActionType.Move, TargetPosition = targetPosition };
        await GameHub.DoAction(action);
    }
}
