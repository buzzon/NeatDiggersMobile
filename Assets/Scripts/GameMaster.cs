using NeatDiggers.GameServer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private void Start()
    {
        ShowMap(false);
        GameHub.OnInitRoom += InitMap;
        GameHub.OnUpdateRoom += UpdateRoom;
    }

    public GameObject CameraTarget;
    public GameObject Map;

    public GameObject Flag;
    public GameObject FlagTaked;
    public GameObject[] EmptyCells;
    public GameObject[] DigCells;
    public GameObject SpawnCell;

    public GameObject PlayersParent;

    private async void InitMap(Room room)
    {
        GameMap map = await GameHub.GetGameMap();
        DrawMap(map);
    }

    public void DrawMap(GameMap map)
    {
        DrawFloor(map);
        DrawSpawnPoints(map.SpawnPoints);
        DrawFlag(map.FlagSpawnPoint);
        CameraTarget.transform.position = new Vector3(map.Width / 2, 0, map.Height / 2);
    }

    public void UpdateRoom(Room room, GameAction action)
    {
        UpdatePlayers(room.Players);
    }

    public void ShowMap(bool isVisible)
    {
        Map.SetActive(isVisible);
    }

    private void UpdatePlayers(List<Player> players)
    {
        foreach (Player player in players)
        {
            bool isUpdatedPlayer = false;
            foreach (Transform playerOnScene in PlayersParent.transform)      
            {
                PlayerController pc = playerOnScene.GetComponent<PlayerController>();
                if (pc.Player.Id == player.Id)
                {
                    pc.Player = player;
                    isUpdatedPlayer = true;
                    break;
                }
            }
            if (!isUpdatedPlayer)
                PlayersParent.GetComponent<PlayersManager>().CreatePlayer(player);
        }
    }

    private void AddPlayerToScene(Player player)
    {

    }

    private void UpdatePlayerInfo(Player player, Player newState)
    {

    }

    private void DrawFloor(GameMap map)
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                GameObject cell;
                switch (map.Map[x][y])
                {
                    case Cell.Empty:
                        cell = Instantiate(EmptyCells[Random.Range(0, EmptyCells.Length)], new Vector3(x, 0, y), Quaternion.identity, Map.transform);
                        break;
                    case Cell.Digging:
                        cell = Instantiate(DigCells[Random.Range(0, DigCells.Length)], new Vector3(x, 0, y), Quaternion.identity, Map.transform);
                        break;
                    default:
                        break;
                }
            }
        }
        
    }
    
    private void DrawSpawnPoints(List<Vector> spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            GameObject cell = Instantiate(SpawnCell, new Vector3(spawnPoints[i].X, 0, spawnPoints[i].Y), Quaternion.identity);
            cell.transform.parent = Map.transform;
        }
    }

    private void DrawFlag(Vector pos)
    {
        GameObject cell = Instantiate(Flag, new Vector3(pos.X, 1, pos.Y), Quaternion.identity);
        cell.transform.parent = Map.transform;
    }
}
