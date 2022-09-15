using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private enum SelectedPlatform { 
        Air,
        Ground,
    }

    [Header("Previews")]
    [SerializeField] private GameObject airPreview;
    [SerializeField] private GameObject groundPreview;
    private SpriteRenderer airSprite, groundSprite;

    [Header("Prefabs")]
    [SerializeField] private GameObject airPrefab;
    [SerializeField] private GameObject groundPrefab;

    private Platform platformScript;
    private SelectedPlatform platform;
    private bool airPreviewSelected, groundPreviewSelected;
    private bool readyToSpawnAir = true, readyToSpawnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        airSprite = airPreview.GetComponent<SpriteRenderer>();
        airSprite.enabled = false;

        groundSprite = groundPreview.GetComponent<SpriteRenderer>();
        groundSprite.enabled = false;
    }

    public void SetAirPreview() {
        airPreviewSelected = !airPreviewSelected;

        if (airPreviewSelected) {
            // If player has groundPreview on, turn it off
            if (groundPreviewSelected)
                SetGroundPreview();

            airSprite.enabled = true;
            platform = SelectedPlatform.Air;
        }
        else {
            airSprite.enabled = false;
        }
    }

    public void SetGroundPreview() {
        groundPreviewSelected = !groundPreviewSelected;

        if (groundPreviewSelected) {
            // If player has airPreview on, turn it off
            if (airPreviewSelected)
                SetAirPreview();

            groundSprite.enabled = true;
            platform = SelectedPlatform.Ground;
        }
        else {
            groundSprite.enabled = false;
        }
    }

    public void SpawnPlatform() {
        if (platform == SelectedPlatform.Air) {
            // If not yet ready to spawn OR preview is not selected, do nothing
            if (!readyToSpawnAir && !airPreviewSelected)
                return;

            readyToSpawnAir = false;
            SetAirPreview();

            // Despawn existing platform
            if (platformScript != null) {
                platformScript.DeletePlatform();
            }

            // Spawn platform at position
            platformScript = Instantiate(airPrefab, airPreview.transform.position, Quaternion.identity).GetComponent<Platform>();
        }
        else if (platform == SelectedPlatform.Ground) {
            // If not yet ready to spawn OR preview is not selected, do nothing
            if (!readyToSpawnGround || !groundPreviewSelected)
                return;

            readyToSpawnGround = false;
            SetGroundPreview();

            // Despawn existing platform
            if (platformScript != null) {
                platformScript.DeletePlatform();
            }

            // Spawn platform at position
            platformScript = Instantiate(groundPrefab, groundPreview.transform.position, Quaternion.identity).GetComponent<Platform>();
        }
    }
    
    // Animation Events
    private void ReadyToSpawnAir() {
        readyToSpawnAir = true;
    }

    private void ReadyToSpawnGround() {
        readyToSpawnGround = true;
    }
}
