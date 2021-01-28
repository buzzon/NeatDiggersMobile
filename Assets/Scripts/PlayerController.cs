using NeatDiggers.GameServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Player Player { get; set; }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.Position.X, 1, Player.Position.Y);
    }
}
