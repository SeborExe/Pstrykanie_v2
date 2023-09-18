using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TourVisual : SingletonMonobehaviour<TourVisual>
{
    [SerializeField] private Volume volume;
    private Vignette vignette;

    private float changingTurnVignetteTimer;
    private float changingTurnVignetteTime = 1f;

    protected override void Awake()
    {
        base.Awake();

        volume.profile.TryGet<Vignette>(out vignette);
    }

    private void Update()
    {
        UpdateTimers();

        switch (GameTourManager.Instance.GetCurrentGameState())
        {
            case GameState.IsChangingTurn:
                SetVignete(GameTourManager.Instance.GetNextGameState(), false);
                if (changingTurnVignetteTimer <= 0f)
                {
                    GameTourManager.Instance.SetStateAfterVignette();
                    SetVignete(GameTourManager.Instance.GetNextGameState(), true);
                }
                break;
        }
    }

    public void SetVignete(GameState nextTeamTurn, bool showLowVisibleViggnete)
    {
        if (!showLowVisibleViggnete)
        {
            vignette.intensity.value = 0.35f;
        }
        else
        {
            vignette.intensity.value = 0.25f;
        }

        if (nextTeamTurn == GameState.TeamOneTurn || nextTeamTurn == GameState.PlacingChipsByTeamOne)
        {
            vignette.color.value = Color.blue;
        }
        else if (nextTeamTurn == GameState.TeamTwoTurn || nextTeamTurn == GameState.PlacingChipsByTeamTwo)
        {
            vignette.color.value = Color.red;
        }
    }


    private void UpdateTimers()
    {
        if (changingTurnVignetteTimer > 0)
        {
            changingTurnVignetteTimer -= Time.deltaTime;
        }
    }

    public void ChangeVigneteTimer()
    {
        changingTurnVignetteTimer = changingTurnVignetteTime;
    }
}
