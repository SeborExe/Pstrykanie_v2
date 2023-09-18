using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Snapping : MonoBehaviour
{
    private enum ChipState
    {
        Idle, 
        Moving
    }

    private Chip chip;
    private InputManager inputManager;

    [SerializeField] Transform launchPoint;

    private LineRenderer line;
    private LineVisual lineVisual;

    private ChipSO chipDetails;
    private ChipState currentState = ChipState.Idle;

    private bool isMouseDown;
    private Rigidbody rigidBody;
    private Vector3 currentPosition;
    private Vector3 snapForce;
    private float currentStreatch;
    private float maximumForceToReturn = 5000f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        chip = GetComponent<Chip>();

        line = GetComponentInChildren<LineRenderer>();
        lineVisual = line.GetComponent<LineVisual>();
    }

    private void Start()
    {
        inputManager = InputManager.Instance;

        launchPoint.gameObject.SetActive(false);

        line.positionCount = 2;
        line.SetPosition(0, launchPoint.position);
        line.SetPosition(1, launchPoint.position);
    }

    private void Update()
    {
        CheckForInput();
        CheckChipState();
    }

    private void OnMouseEnter()
    {
        launchPoint.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (GameTourManager.Instance.GetCurrentGameStateTeamNumber() == chip.GetChipTeamID() && inputManager.IsSnapping)
        {
            isMouseDown = true;
        }
    }

    private void OnMouseUp()
    {
        if (GameTourManager.Instance.GetCurrentGameStateTeamNumber() == chip.GetChipTeamID() && !inputManager.IsSnapping)
        {
            isMouseDown = false;
            Snap();
        }
    }

    private void CheckForInput()
    {
        if (isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;

            line.gameObject.SetActive(true);

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            SetStrips(currentPosition);

            GameManager.Instance.currentSelectedChip = gameObject;
            GameManager.Instance.SetCameraOrtoSize(currentStreatch / 4f, Time.deltaTime);
        }
        else
        {
            ResetStrips();
            GameManager.Instance.SetDefaultCameraOrtoSize(Time.deltaTime);
        }
    }

    private void CheckChipState()
    {
        if (currentState == ChipState.Moving)
        {
            if (GameTourManager.Instance.GetCurrentGameState() == GameState.GameOver)
            {
                currentState = ChipState.Idle;
                return;
            }

            else if (rigidBody.IsSleeping() || !chip.IsPlaying())
            {
                GameTourManager.Instance.ChangeGameState(chip.GetChipTeamID());
                currentState = ChipState.Idle;
                gameObject.tag = "Chip";
            }
        }
    }

    private void ResetStrips()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        currentStreatch = 0;
    }

    private void SetStrips(Vector3 position)
    {
        Vector3 direction = position - launchPoint.position; // Obliczamy wektor kierunkowy od punktu startowego do pozycji kursora
        direction.y = 0f; // Ustawiamy Y na 0, aby koñcówka linii by³a na p³aszczyŸnie XZ
        float distance = direction.magnitude; // Obliczamy d³ugoœæ wektora

        if (distance > chipDetails.maxStretch)
        {
            direction = direction.normalized * chipDetails.maxStretch; // Skalujemy wektor kierunkowy, aby jego d³ugoœæ nie przekracza³a 20
        }

        if (distance > chipDetails.maxStretch)
            distance = chipDetails.maxStretch;

        currentStreatch = distance;

        Vector3 endPoint = launchPoint.position + direction; // Obliczamy koñcow¹ pozycjê linii
        endPoint.y = 0f; // Ustawiamy Y na 0, aby koñcówka linii by³a na p³aszczyŸnie XZ

        snapForce = (endPoint - launchPoint.position) * chipDetails.speed * -1;

        lineVisual.ChangeLineColor(direction, chipDetails.maxStretch);

        line.SetPosition(0, launchPoint.position);
        line.SetPosition(1, endPoint);
    }

    private void Snap()
    {
        line.gameObject.SetActive(false);
        GameManager.Instance.currentSelectedChip = null;

        if (snapForce.magnitude < maximumForceToReturn)
            return;

        rigidBody.AddForce(new Vector3(snapForce.x, 0, snapForce.z));

        GameTourManager.Instance.ChangeGameState(chip.GetChipTeamID(), true);
        currentState = ChipState.Moving;
        gameObject.tag = "Player";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Chip" && collision.gameObject.TryGetComponent(out Rigidbody enemyRigidbody))
        {
            CheckSpecialConditions(collision, out float Multiplier);

            Vector3 direction = (collision.transform.position - transform.position).normalized;
            enemyRigidbody.AddForce(direction * CalculatePushForce() * Multiplier);
        }
    }

    private void CheckSpecialConditions(Collision collision, out float multiplier)
    {
        multiplier = 1.0f;

        CheckIfBothChipAreMetal(ref multiplier, collision);
    }

    private void CheckIfBothChipAreMetal(ref float multiplier, Collision collision)
    {
        if (chipDetails.isMetal && collision.gameObject.GetComponent<Chip>().IsMetal())
            multiplier = GameSettings.Instance.metalVSmetalMultiplier;
    }

    private float CalculatePushForce()
    {
        float totalForce = 0;

        totalForce += chipDetails.pushPower * Mathf.Abs(rigidBody.velocity.magnitude) * chipDetails.mass;

        Debug.Log(totalForce);
        return totalForce;
    }

    public void SetChipDetails(ChipSO chipDetails) => this.chipDetails = chipDetails;
}
