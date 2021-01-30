using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanelHandler : MonoBehaviour
{
    private string title;
    public string Title
    {
        get => title;
        set
        {
            title = value;
            transform.Find("Title").GetComponent<Text>().text = title;
        }
    }

    private string description;
    public string Description
    {
        get => description;
        set
        {
            description = value;
            transform.Find("Description").GetComponent<Text>().text = description;
        }
    }

    public void Cancel() => Destroy(transform.gameObject);

    public Action UseAction;
    public void Use()
    {
        UseAction?.Invoke();
    }

    public Action UseLeftAction;
    public void UseLeft()
    {
        UseLeftAction?.Invoke();
    }

    public Action UseRightAction;
    public void UseRight()
    {
        UseRightAction?.Invoke();
    }

    public Action UseTwoAction;
    public void UseTwo()
    {
        UseTwoAction?.Invoke();
    }

    public Action EquipAction;
    public void Equip()
    {
        EquipAction?.Invoke();
    }

    public void DrawButtons()
    {
        transform.Find("ButtonUse").gameObject.SetActive(UseAction != null);
        transform.Find("ButtonUseLeft").gameObject.SetActive(UseLeftAction != null);
        transform.Find("ButtonUseRight").gameObject.SetActive(UseRightAction != null);
        transform.Find("ButtonUseTwo").gameObject.SetActive(UseTwoAction != null);
        transform.Find("ButtonEquip").gameObject.SetActive(EquipAction != null);
    }
}
