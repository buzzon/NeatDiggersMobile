using NeatDiggers.GameServer.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    public GameObject DescriptionPanel;
    public Ability Ability { get; set; }

    public void OnClick()
    {
        Instantiate(DescriptionPanel);
        DescriptionPanelHandler dph = DescriptionPanel.GetComponent<DescriptionPanelHandler>();
    }
}
