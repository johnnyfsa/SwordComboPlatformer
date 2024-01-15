using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageble))]
public class EquippedItems : MonoBehaviour
{
    public ShieldTypeData equippedShield;
    private Damageble damageble;

    private void Start()
    {
        damageble = GetComponent<Damageble>();
        damageble.ArmorValue = equippedShield.armorValue;
    }
}
