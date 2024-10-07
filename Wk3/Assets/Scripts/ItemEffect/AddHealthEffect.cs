using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatorKitCode;

public class AddHealthEffect : UsableItem.UsageEffect
{
    public int healthRecovery;

    public override bool Use(CharacterData user)
    {
        user.Stats.ChangeHealth(healthRecovery);
        return true;
    }
}
