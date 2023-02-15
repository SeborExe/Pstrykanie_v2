using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Snapping : MonoBehaviour
{
    private Chip chip;

    [SerializeField] Transform launchPoint;
    [SerializeField] LineRenderer line;

    private float speed;
    private float streatch;
    private float pushPower;
    private float mass;

    private bool isMouseDown;
    private Rigidbody rigidBody;
    private Vector3 currentPosition;
    private Vector3 snapForce;

    private bool isSnapped = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        chip = GetComponent<Chip>();
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

        if (isSnapped)
        {
            if (rigidBody.IsSleeping() || !chip.IsPlaying())
            {
                isSnapped = false;
                GameManager.Instance.ChangeGameState(chip.GetChipTeamID());
            }
        }
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
            snapForce = (currentPosition - launchPoint.position) * speed * -1;

            SetStrips(currentPosition);
        }
        else
        {
            ResetStrips();
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
        direction.y = 0f; // Ustawiamy Y na 0, aby koñcówka linii by³a na p³aszczyŸnie XZ
        float distance = direction.magnitude; // Obliczamy d³ugoœæ wektora

        if (distance > streatch)
        {
            direction = direction.normalized * streatch; // Skalujemy wektor kierunkowy, aby jego d³ugoœæ nie przekracza³a 20
        }

        Vector3 endPoint = launchPoint.position + direction; // Obliczamy koñcow¹ pozycjê linii
        endPoint.y = 0f; // Ustawiamy Y na 0, aby koñcówka linii by³a na p³aszczyŸnie XZ
        line.SetPosition(0, launchPoint.position);
        line.SetPosition(1, endPoint);
    }

    private void Snap()
    {
        rigidBody.AddForce(new Vector3(snapForce.x, 0, snapForce.z));
        line.gameObject.SetActive(false);

        GameManager.Instance.ChangeGameState(chip.GetChipTeamID(), true);
        isSnapped = true;
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Chip" && collision.gameObject.TryGetComponent(out Rigidbody enemyRigidbody))
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction = direction.normalized;
            enemyRigidbody.AddForce(direction * pushPower * rigidBody.velocity.z * mass);
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
