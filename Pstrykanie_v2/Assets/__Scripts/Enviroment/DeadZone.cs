using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : SingletonMonobehaviour<DeadZone>
{
    public event EventHandler<OnChipFallArgs> OnChipFall;
    public class OnChipFallArgs: EventArgs
    {
        public int chipTeamID;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Chip chip))
        {
            if (chip.IsPlaying())
            {
                OnChipFall?.Invoke(this, new OnChipFallArgs { chipTeamID = chip.GetChipTeamID() });
                chip.DeDeactivateChip();
            }
        }
    }
}
