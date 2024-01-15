using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    public static UnityAction<Transform, int> OnCharacterDamaged;
    public static UnityAction<Transform, int> OnCharacterHealed;
}