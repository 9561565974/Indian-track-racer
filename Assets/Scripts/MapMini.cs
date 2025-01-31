using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMini : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject waypointOrigins;

    public GameObject MiniMapCam;    // Mini-map camera
    public GameObject PlayerPrefab; // Prefab that indicates the player's position on the minimap
    public GameObject EnemyPrefab;  // Prefab that indicates the opponent's position on the minimap
    public GameObject Opponent;     // Directly assign the opponent GameObject in the Inspector

    void Start()
    {
        // Initialize line renderer for waypoints
        lineRenderer = GetComponent<LineRenderer>();
        waypointOrigins = this.gameObject;

        int num_of_path = waypointOrigins.transform.childCount;
        lineRenderer.positionCount = num_of_path + 1;

        for (int x = 0; x < num_of_path; x++)
        {
            lineRenderer.SetPosition(x, new Vector3(waypointOrigins.transform.GetChild(x).transform.position.x, 4, waypointOrigins.transform.GetChild(x).transform.position.z));
        }

        lineRenderer.SetPosition(num_of_path, lineRenderer.GetPosition(0));
        lineRenderer.startWidth = 40f;
        lineRenderer.endWidth = 40f;
    }

    void Update()
    {
        // Find the active player
        GameObject activePlayer = FindActiveObjectWithTag("Player");
        if (activePlayer != null && PlayerPrefab != null)
        {
            // Set minimap camera to follow the active player
            MiniMapCam.transform.position = new Vector3(activePlayer.transform.position.x, MiniMapCam.transform.position.y, activePlayer.transform.position.z);

            // Update the position of the player's minimap indicator
            PlayerPrefab.transform.position = new Vector3(activePlayer.transform.position.x, PlayerPrefab.transform.position.y, activePlayer.transform.position.z);
        }

        // Update the opponent's minimap indicator if assigned
        if (Opponent != null && EnemyPrefab != null)
        {
            EnemyPrefab.transform.position = new Vector3(Opponent.transform.position.x, EnemyPrefab.transform.position.y, Opponent.transform.position.z);
        }
    }

    // Helper function to find the active object with a specific tag
    GameObject FindActiveObjectWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            if (obj.activeInHierarchy) // Return the first active object found
            {
                return obj;
            }
        }
        return null; // No active object found
    }
}



