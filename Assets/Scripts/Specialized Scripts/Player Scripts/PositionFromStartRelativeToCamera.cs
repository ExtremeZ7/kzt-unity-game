﻿using UnityEngine;
using AssemblyCSharp;

public class PositionFromStartRelativeToCamera : MonoBehaviour
{

    public Vector2 positionRatio = Vector2.one;
    public Vector2 positionOffset;

    void OnDrawGizmos()
    {
        Vector3 startCheckpointPosition = GameObject.FindGameObjectWithTag("Checkpoint List").transform.GetChild(0).position;

        Gizmos.color = Color.cyan;
        int sections = 5;
        for (int i = 0; i < sections; i++)
        {
            Gizmos.DrawLine(Helper.GetDividingPoint(transform.position, startCheckpointPosition, sections * 2, i * 2 + 1)
				, Helper.GetDividingPoint(transform.position, startCheckpointPosition, sections * 2, i * 2 + 2));
        }

        Transform camera = Camera.main.transform;
        Gizmos.color = Color.red;
        for (int i = 0; i < sections; i++)
        {
            Gizmos.DrawLine(Helper.GetDividingPoint(camera.position, startCheckpointPosition, sections * 2, i * 2 + 1)
				, Helper.GetDividingPoint(camera.position, startCheckpointPosition, sections * 2, i * 2 + 2));
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);

        OnUpdate();
    }

    void OnValidate()
    {
        OnUpdate();
    }

    void OnUpdate()
    {
        Transform camera = Camera.main.transform;

        transform.position = new Vector3(-camera.position.x / (positionRatio.x != 0 ? positionRatio.x : Mathf.Infinity) + positionOffset.x
			, -camera.position.y / (positionRatio.y != 0 ? positionRatio.y : Mathf.Infinity) + positionOffset.y);
    }
}
