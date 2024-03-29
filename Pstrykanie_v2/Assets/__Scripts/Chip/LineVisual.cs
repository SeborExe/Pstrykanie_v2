using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisual : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private Color easyColor;
    [SerializeField] private Color mediumColor;
    [SerializeField] private Color hardColor;
    [SerializeField] private Color maxColor;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ChangeLineColor(Vector3 direction, float streatch)
    {
        float lineLength = Mathf.Max(Math.Abs(direction.x), Mathf.Abs(direction.z));
        float percentMaxLength = lineLength / streatch;

        if (percentMaxLength < 0.15f)
        {
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }
        else if (percentMaxLength >= 0.15f && percentMaxLength < 0.35f)
        {
            lineRenderer.startColor = easyColor;
            lineRenderer.endColor = easyColor;
        }
        else if (percentMaxLength >= 0.35f && percentMaxLength < 0.65f)
        {
            lineRenderer.startColor = mediumColor;
            lineRenderer.endColor = mediumColor;
        }
        else if (percentMaxLength >= 0.65f && percentMaxLength < 0.9f)
        {
            lineRenderer.startColor = hardColor;
            lineRenderer.endColor = hardColor;
        }
        else
        {
            lineRenderer.startColor = maxColor;
            lineRenderer.endColor = maxColor;
        }
    }
}
