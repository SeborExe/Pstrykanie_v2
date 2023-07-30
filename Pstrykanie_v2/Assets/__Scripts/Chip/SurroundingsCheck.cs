using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingsCheck : MonoBehaviour
{
    private Rigidbody rigidBody;
    private MeshCollider meshCollider;

    private float chanceToBetrayal;
    private bool physicDisabled;

    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody>();
        meshCollider = GetComponentInParent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Chip")
        {
            CheckBetrayal();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (physicDisabled)
        {
            ChangePhysics(true);
            physicDisabled = false;
        }
    }

    private void ChangePhysics(bool active)
    {
        meshCollider.enabled = active;
        rigidBody.useGravity = active;
    }

    private void CheckBetrayal()
    {
        if (Random.Range(0, 100) < chanceToBetrayal)
            StartCoroutine(BetrayalCoroutine());
    }

    private IEnumerator BetrayalCoroutine()
    {
        ChangePhysics(false);
        physicDisabled = true;

        yield return new WaitForSeconds(3);

        ChangePhysics(true);
    }

    public void SetChanceToBetrayal(float chanceToBetrayal)
    {
        this.chanceToBetrayal = chanceToBetrayal;
    }
}
