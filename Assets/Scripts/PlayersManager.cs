using NeatDiggers.GameServer;
using NeatDiggers.GameServer.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class CharacterPrefab
{
    public CharacterName Name;
    public GameObject Prefab;
}

public class PlayersManager : MonoBehaviour
{
    public CharacterPrefab[] Characters;

    public void CreatePlayer(Player player)
    {
        GameObject prefab = Characters.FirstOrDefault(c => c.Name == player.Character.Name)?.Prefab;
        if (prefab == null)
            return;
        GameObject playerObject = Instantiate(prefab, new Vector3(player.Position.X, 0, player.Position.Y), Quaternion.identity, transform);
        playerObject.GetComponent<PlayerController>().Player = player;
    }
}
