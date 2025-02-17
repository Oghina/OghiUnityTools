//-----------------------------------------------------------------------
// <copyright file="ClosestPrimitiveCollider.cs" company="">
// Copyright (c) 2025. All rights reserved.
// </copyright>
// <author>Oghina</author>
// <date>2025-02-13 07:54:30 UTC</date>
// <summary>Editor tool to automatically fit primitive colliders to object hierarchies</summary>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(ClosestPrimitiveCollider))]
public class ClosestPrimitiveColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ClosestPrimitiveCollider fitter = (ClosestPrimitiveCollider)target;

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Generate Capsule Collider", GUILayout.Height(30)))
        {
            GenerateCapsuleCollider(fitter.gameObject);
        }
    }

    private void GenerateCapsuleCollider(GameObject targetObject)
    {
        MeshFilter[] meshFilters = targetObject.GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "No meshes found in the hierarchy!", "OK");
            return;
        }

        // Convert all vertices to local space of the target object
        List<Vector3> allLocalVertices = new List<Vector3>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.sharedMesh == null) continue;
            
            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                // Convert from mesh local space to world space, then to target object's local space
                Vector3 worldPoint = meshFilter.transform.TransformPoint(vertices[i]);
                Vector3 localPoint = targetObject.transform.InverseTransformPoint(worldPoint);
                allLocalVertices.Add(localPoint);
            }
        }

        if (allLocalVertices.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "No valid vertices found!", "OK");
            return;
        }

        // Calculate bounds in local space
        Vector3 minPoint = allLocalVertices[0];
        Vector3 maxPoint = allLocalVertices[0];
        Vector3 center = Vector3.zero;

        foreach (Vector3 vertex in allLocalVertices)
        {
            minPoint = Vector3.Min(minPoint, vertex);
            maxPoint = Vector3.Max(maxPoint, vertex);
            center += vertex;
        }
        center /= allLocalVertices.Count;

        // Calculate dimensions in local space
        Vector3 dimensions = maxPoint - minPoint;

        // Get or add CapsuleCollider component
        CapsuleCollider capsuleCollider = targetObject.GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            capsuleCollider = targetObject.AddComponent<CapsuleCollider>();
        }

        // Set the center (already in local space)
        capsuleCollider.center = center;

        // Find the axis with the greatest spread of vertices
        float[] axisSpreads = new float[3];
        for (int axis = 0; axis < 3; axis++)
        {
            float minSpread = float.MaxValue;
            float maxSpread = float.MinValue;
            
            // Project vertices onto each axis
            foreach (Vector3 vertex in allLocalVertices)
            {
                float spread = 0;
                switch (axis)
                {
                    case 0: // X axis
                        spread = Vector3.Project(vertex - center, Vector3.right).magnitude;
                        break;
                    case 1: // Y axis
                        spread = Vector3.Project(vertex - center, Vector3.up).magnitude;
                        break;
                    case 2: // Z axis
                        spread = Vector3.Project(vertex - center, Vector3.forward).magnitude;
                        break;
                }
                minSpread = Mathf.Min(minSpread, spread);
                maxSpread = Mathf.Max(maxSpread, spread);
            }
            axisSpreads[axis] = maxSpread - minSpread;
        }

        // Find the direction with the largest spread
        int direction = 0;
        float maxSpread2 = axisSpreads[0];
        for (int i = 1; i < 3; i++)
        {
            if (axisSpreads[i] > maxSpread2)
            {
                maxSpread2 = axisSpreads[i];
                direction = i;
            }
        }

        // Set the direction based on the axis with greatest vertex spread
        capsuleCollider.direction = direction;

        // Set height using the spread of the chosen axis
        capsuleCollider.height = axisSpreads[direction];

        // Calculate radius from the other two dimensions
        float[] otherDimensions = new float[2];
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            if (i != direction)
            {
                otherDimensions[index] = dimensions[i];
                index++;
            }
        }

        // Use the smaller of the two remaining dimensions for radius
        float radius = Mathf.Min(otherDimensions[0], otherDimensions[1]) * 0.5f;
        
        // Apply tight fit factor (0.7 for even tighter fit)
        capsuleCollider.radius = radius * 0.7f;

        string axisName = direction == 0 ? "X" : direction == 1 ? "Y" : "Z";
        Debug.Log($"Capsule Collider generated for '{targetObject.name}'\n" +
                 $"Height: {capsuleCollider.height}\n" +
                 $"Radius: {capsuleCollider.radius}\n" +
                 $"Direction: {axisName}-axis (local)\n" +
                 $"Axis Spreads: X:{axisSpreads[0]:F2}, Y:{axisSpreads[1]:F2}, Z:{axisSpreads[2]:F2}");
    }
}

public class ClosestPrimitiveCollider : MonoBehaviour
{
    private void Reset()
    {
        if (GetComponent<CapsuleCollider>() == null)
        {
            gameObject.AddComponent<CapsuleCollider>();
        }
    }
}