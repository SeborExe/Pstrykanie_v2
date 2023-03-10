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

    [SerializeField] Transform launchPoint;

    private LineRenderer line;
    private LineVisual lineVisual;

    private float speed;
    private float streatch;
    private float pushPower;
    private float mass;
    private ChipState currentState = ChipState.Idle;

    private bool isMouseDown;
    private Rigidbody rigidBody;
    private Vector3 currentPosition;
    private Vector3 snapForce;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        chip = GetComponent<Chip>();

        line = GetComponentInChildren<LineRenderer>();
        lineVisual = line.GetComponent<LineVisual>();
    }

    private void Start()
    {
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
        if (GameManager.Instance.GetCurrentGameStateTeamNumber() == chip.GetChipTeamID())
        {
            isMouseDown = true;
        }
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.GetCurrentGameStateTeamNumber() == chip.GetChipTeamID())
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
        }
        else
        {
            ResetStrips();
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
    }

    private void SetStrips(Vector3 position)
    {
        Vector3 direction = position - launchPoint.position; // Obliczamy wektor kierunkowy od punktu startowego do pozycji kursora
        direction.y = 0f; // Ustawiamy Y na 0, aby ko?c?wka linii by?a na p?aszczy?nie XZ
        float distance = direction.magnitude; // Obliczamy d?ugo?? wektora

        if (distance > streatch)
        {
            direction = direction.normalized * streatch; // Skalujemy wektor kierunkowy, aby jego d?ugo?? nie przekracza?a 20
        }

        Vector3 endPoint = launchPoint.position + direction; // Obliczamy ko?cow? pozycj? linii
        endPoint.y = 0f; // Ustawiamy Y na 0, aby ko?c?wka linii by?a na p?aszczy?nie XZ

        snapForce = (endPoint - launchPoint.position) * speed * -1;

        lineVisual.ChangeLineColor(direction, streatch);

        line.SetPosition(0, launchPoint.position);
        line.SetPosition(1, endPoint);
    }

    private void Snap()
    {
        rigidBody.AddForce(new Vector3(snapForce.x, 0, snapForce.z));
        line.gameObject.SetActive(false);

        GameTourManager.Instance.ChangeGameState(chip.GetChipTeamID(), true);
        currentState = ChipState.Moving;
        gameObject.tag = "Player";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Chip" && collision.gameObject.TryGetComponent(out Rigidbody enemyRigidbody))
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            float pushForce = pushPower * Mathf.Abs(rigidBody.velocity.z) * mass;
            enemyRigidbody.AddForce(direction * pushForce);
        }
    } 

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetStreatch(float streatch)
    {
        this.streatch = streatch;
    }

    public void SetPushPower(float pushPower)
    {
        this.pushPower = pushPower;
    }

    public void SetMass(float mass)
    {
        this.mass = mass;
    }
}
