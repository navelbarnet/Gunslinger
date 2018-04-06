﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InventoryScript : MonoBehaviour {

    public Image clean;

    public int inventorySize = 3;

    List<Image> slots = new List<Image>();

    public RevolverUIscript revUI;

	// Use this for initialization
	void Start () {
        //mayby change the spawn method into something more dynamic
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(Instantiate(clean, new Vector3((i * 100) + 50, 50, 0), Quaternion.identity, this.gameObject.transform));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            revUI.LoadBullet(slots[0].sprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            revUI.LoadBullet(slots[1].sprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            revUI.LoadBullet(slots[2].sprite);
        }
    }


    public void CmdPick(int type, Sprite bulletImage)
    {
        if (type != 0)
        {
            slots[type-1].sprite = bulletImage;
        }
    }
}
