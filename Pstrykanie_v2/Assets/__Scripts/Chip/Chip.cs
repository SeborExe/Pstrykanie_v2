using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    Renderer chipRenderer;
    Rigidbody rigidBody;
    Snapping snapping;
    MeshCollider meshCollider;
    ChipSO chipDetails;
    [SerializeField] private SurroundingsCheck surroundingsCheck;

    private int chipTeamID;
    private bool isPlaying = true;

    private void Awake()
    {
        chipRenderer = GetComponent<Renderer>();
        rigidBody = GetComponent<Rigidbody>();
        snapping = GetComponent<Snapping>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void InitializeChip(ChipSO chipDetails, int chipTeamID)
    {
        this.chipDetails = chipDetails;

        chipRenderer.material.SetTexture("_BaseMap", chipDetails.Image.texture);
        transform.localScale = chipDetails.size;
        rigidBody.mass = chipDetails.mass;
        meshCollider.material = chipDetails.physicMaterial;

        snapping.SetChipDetails(chipDetails);
        surroundingsCheck.SetChanceToBetrayal(chipDetails.chanceToBetrayal);

        this.chipTeamID = chipTeamID;
    }

    public int GetChipTeamID() => chipTeamID;
    public bool IsPlaying() => isPlaying;
    public void DeDeactivateChip() => isPlaying = false;
    public bool IsMetal() => chipDetails.isMetal;
}
