using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTourManager : SingletonMonobehaviour<GameTourManager>
{
    public event EventHandler OnStateChanged;
    public event EventHandler OnGameOver;
    public event EventHandler OnPlacingChipEnded;
    public event EventHandler<OnNextStateChangedArgs> OnNextStateChanged;

    private GameManager gameManager;

    [Header("Layer Masks")]
    [SerializeField] LayerMask whatIsSpawnPointTeamOne;
    [SerializeField] LayerMask whatIsSpawnPointTeamTwo;
    [SerializeField] LayerMask whatIsChip;
    [Header("Teams Location")]
    [SerializeField] MeshRenderer teamBlueMeshRenderer;
    [SerializeField] MeshRenderer teamRedMeshRenderer;

    [Header("Enviroment")]
    [SerializeField] private TourBarrier barrier;
    private bool isBarrierActive = true;

    public class OnNextStateChangedArgs : EventArgs
    {
        public GameState nextGameState;
    }

    private GameState currentState;
    private GameState previousGameState;
    private GameState nextGameState;

    private int winningTeamID;
    private bool placingComplete;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        switch (nextGameState)
        {
            case GameState.TeamOneTurn:
            case GameState.TeamTwoTurn:
                OnNextStateChanged(this, new OnNextStateChangedArgs { nextGameState = nextGameState });
                break;
            default:
                OnNextStateChanged(this, new OnNextStateChangedArgs { nextGameState = GameState.None });
                break;
        }

        switch (currentState)
        {
            case GameState.PlacingChipsByTeamOne:
                if (TryPlanceChip(1) && !placingComplete)
                {
                    teamBlueMeshRenderer.enabled = false;
                    teamRedMeshRenderer.enabled = true;
                    nextGameState = GameState.PlacingChipsByTeamTwo;
                    ChangingTurn();
                }
                break;
            case GameState.PlacingChipsByTeamTwo:
                if (TryPlanceChip(2) && !placingComplete)
                {
                    teamBlueMeshRenderer.enabled = true;
                    teamRedMeshRenderer.enabled = false;
                    nextGameState = GameState.PlacingChipsByTeamOne;
                    ChangingTurn();
                }
                break;
            default:
                break;
        }
    }

    private bool TryPlanceChip(int team)
    {
        if (team == 1)
        {
            return HandleRaycastOnPlacingChips(whatIsSpawnPointTeamOne, GameState.TeamTwoTurn, team);
        }
        else
        {
            return HandleRaycastOnPlacingChips(whatIsSpawnPointTeamTwo, GameState.TeamOneTurn, team);
        }
    }

    private bool HandleRaycastOnPlacingChips(LayerMask layerMaskToPlaceChip, GameState nextGameStateToSetIfAllChipsOnBoard, int team)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, layerMaskToPlaceChip))
            {
                gameManager.InitializeChip(hit.point, team);
                gameManager.DecreaseRemainingsChipToPlace(team);

                if (gameManager.TeamOneChipToPlaceRemains == 0 && gameManager.TeamTwoChipToPlaceRemains == 0)
                {
                    placingComplete = true;
                    gameManager.SetTeamCount();

                    teamBlueMeshRenderer.enabled = false;
                    teamRedMeshRenderer.enabled = false;

                    OnPlacingChipEnded?.Invoke(this, EventArgs.Empty);

                    nextGameState = nextGameStateToSetIfAllChipsOnBoard;
                    ChangingTurn();
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    private void ChangingTurn()
    {
        currentState = GameState.IsChangingTurn;
        TourVisual.Instance.ChangeVigneteTimer();
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RollStartingTeam()
    {
        int startingTeam = UnityEngine.Random.Range(1, 3);
        if (startingTeam == 1)
        {
            teamBlueMeshRenderer.enabled = true;
            nextGameState = GameState.PlacingChipsByTeamOne;
            ChangingTurn();
        }
        else
        {
            teamRedMeshRenderer.enabled = true;
            nextGameState = GameState.PlacingChipsByTeamTwo;
            ChangingTurn();
        }
    }

    public void ChangeGameState(int chipID, bool wait = false)
    {
        if (chipID == 1 && !wait && GameManager.Instance.CheckIfAnyTeamHasChip())
        {
            nextGameState = GameState.TeamTwoTurn;
            DisableBarrier();
            ChangingTurn();
        }
        else if (chipID == 2 && !wait && GameManager.Instance.CheckIfAnyTeamHasChip())
        {
            nextGameState = GameState.TeamOneTurn;
            DisableBarrier();
            ChangingTurn();
        }
        else
        {
            currentState = GameState.Waiting;
            if (chipID == 1)
            {
                previousGameState = GameState.TeamOneTurn;
            }
            else
            {
                previousGameState = GameState.TeamTwoTurn;
            }
        }

        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeGameState(GameState gameState)
    {
        currentState = gameState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void CheckForGameOver()
    {
        if (!GameManager.Instance.CheckIfTeamHasChip(1))
        {
            winningTeamID = 2;
            GameOver();
        }

        if (!GameManager.Instance.CheckIfTeamHasChip(2))
        {
            winningTeamID = 1;
            GameOver();
        }
    }

    private void GameOver()
    {
        currentState = GameState.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void SetStateAfterVignette()
    {
        ChangeGameState(nextGameState);
        nextGameState = GameState.None;
    }

    public int GetCurrentGameStateTeamNumber()
    {
        if (GetCurrentGameState() == GameState.TeamOneTurn) return 1;

        if (GetCurrentGameState() == GameState.TeamTwoTurn) return 2;

        else return 0;
    }

    public GameState GetCurrentGameState()
    {
        return currentState;
    }

    public GameState GetNextGameState()
    {
        return nextGameState;
    }

    public int GetWinningTeamID()
    {
        return winningTeamID;
    }

    private void DisableBarrier()
    {
        if (isBarrierActive)
        {
            barrier.gameObject.SetActive(false);
            isBarrierActive = false;
        }
    }
}
