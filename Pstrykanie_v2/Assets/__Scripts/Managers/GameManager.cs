using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public GameTourManager GameTourManager { get; private set; }

    [Header("Chips")]
    [SerializeField] private Chip ChipPrefab;
    List<ChipSO> teamOneChips = new List<ChipSO>();
    List<ChipSO> teamTwoChips = new List<ChipSO>();

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Camera info")]
    [SerializeField] private float cameraSpeed;
    private float defaultCameraLensOrtoSize = 38f;
    private float currentCameraLensOrtoSize;
    [HideInInspector] public GameObject currentSelectedChip;

    private int chipTeamOneCount;
    private int chipTeamTwoCount;

    public int TeamOneChipToPlaceRemains { get; private set; }
    public int TeamTwoChipToPlaceRemains { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameTourManager = GameTourManager.Instance;

        GetTeams();
        
        DeadZone.Instance.OnChipFall += DeadZone_OnChipFall;
    }

    private void GetTeams()
    {
        teamOneChips = GameSettings.Instance.GetTeamChips(1);
        teamTwoChips = GameSettings.Instance.GetTeamChips(2);

        TeamOneChipToPlaceRemains = teamOneChips.Count;
        TeamTwoChipToPlaceRemains = teamTwoChips.Count;

        GameTourManager.RollStartingTeam();
    }

    public void InitializeChip(Vector3 position, int team)
    {
        Chip chip = Instantiate(ChipPrefab, position, Quaternion.identity);

        if (team == 1)
            chip.InitializeChip(teamOneChips[TeamOneChipToPlaceRemains - 1], team);
        else
            chip.InitializeChip(teamTwoChips[TeamTwoChipToPlaceRemains - 1], team);
    }

    public ChipSO GetNextChipToPlace(int team)
    {
        if (team == 1)
            return teamOneChips[TeamOneChipToPlaceRemains - 1];
        else
            return teamTwoChips[TeamTwoChipToPlaceRemains - 1];
    }

    public void SetTeamCount()
    {
        chipTeamOneCount = teamOneChips.Count;
        chipTeamTwoCount = teamTwoChips.Count;
    }

    private void DeadZone_OnChipFall(object sender, DeadZone.OnChipFallArgs args)
    {
        if (args.chipTeamID == 1)
        {
            chipTeamOneCount--;
        }
        else
        {
            chipTeamTwoCount--;
        }

        GameTourManager.Instance.CheckForGameOver();
    }

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        CinemachineShake cinemachineShake = virtualCamera.GetComponent<CinemachineShake>();
        cinemachineShake.ShakeCamera(amplitude, frequency, time);
    }

    public bool CheckIfAnyTeamHasChip()
    {
        return chipTeamTwoCount != 0 && chipTeamOneCount != 0;
    }

    public bool CheckIfTeamHasChip(int teamNumber)
    {
        if (teamNumber == 1) return chipTeamOneCount != 0;
        if (teamNumber == 2) return chipTeamTwoCount != 0;
        else return false;
    }

    public void DecreaseRemainingsChipToPlace(int team)
    {
        if (team == 1)
            TeamOneChipToPlaceRemains--;
        else
            TeamTwoChipToPlaceRemains--;
    }

    public float GetDefaultCameraOrtoSize() => defaultCameraLensOrtoSize;
    public float GetCurrentCameraOrtoSize() => currentCameraLensOrtoSize;

    public void SetCameraOrtoSize(float value, float deltaTime)
    {
        if (currentSelectedChip == null) return;

        currentCameraLensOrtoSize = defaultCameraLensOrtoSize + value;

        if (virtualCamera.m_Lens.OrthographicSize < currentCameraLensOrtoSize)
        {
            virtualCamera.m_Lens.OrthographicSize += deltaTime * cameraSpeed;
        }
    }

    public void SetDefaultCameraOrtoSize(float deltaTime)
    {
        if (currentSelectedChip != null) return;

        if (virtualCamera.m_Lens.OrthographicSize > defaultCameraLensOrtoSize)
        {
            virtualCamera.m_Lens.OrthographicSize -= deltaTime * (cameraSpeed / 3f);
            currentCameraLensOrtoSize = virtualCamera.m_Lens.OrthographicSize;
        }
    }
}
