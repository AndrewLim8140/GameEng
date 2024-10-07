using System.Collections;
using System.Collections.Generic;
using CreatorKitCode;
using UnityEngine;
using UnityEngine.Serialization;

public class AddHealthUsageEffect : UsableItem.UsageEffect
{
    [FormerlySerializedAs("HealthPurcentageAmount")] 
    public int HealthPercentageAmount = 20;
    
    public override bool Use(CharacterData user)
    {
        if (user.Stats.CurrentHealth == user.Stats.stats.health)
            return false;

        VFXManager.PlayVFX(VFXType.Healing, user.transform.position);

        user.Stats.ChangeHealth( Mathf.FloorToInt(HealthPercentageAmount/100.0f * user.Stats.stats.health) );

        return true;
    }
}