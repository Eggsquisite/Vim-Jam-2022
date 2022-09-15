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
    [SerializeField] private SpriteRenderer airDisabled;
    [SerializeField] private SpriteRenderer groundDisabled;
    private SpriteRenderer airSprite, groundSprite;

    [Header("Prefabs")]
    [SerializeField] private GameObject airPrefab;
    [SerializeField] private GameObject groundPrefab;

    private Platform platformScript;
    private SelectedPlatform platform;
    private bool airPreviewSelected, groundPreviewSelected;
    private bool readyToSpawnAir, readyToSpawnGround;

    [Header("Platform Timers")]
    [SerializeField] private float maxAirCooldown;
    [SerializeField] private float maxGroundCooldown;
    private bool airCooldown, groundCooldown;

    // Start is called before the first frame update
    void Start()
    {
        airSprite = airPreview.GetComponent<SpriteRenderer>();
        airSprite.enabled = false;
        airDisabled.enabled = false;

        groundSprite = groundPreview.GetComponent<SpriteRenderer>();
        groundSprite.enabled = false;
        groundDisabled.enabled = false;
    }

    private void Update() {
        UpdateDisabledMarkers();
    }

    private void UpdateDisabledMarkers() { 
        // Check if airPlatform is ready to spawn
        if (airPreviewSelected) {
            switch (readyToSpawnAir)
            {
                case true:
                    airDisabled.enabled = false;
                    break;
                case false:
                    airDisabled.enabled = true;
                    break;
            }
        }
        else {
            airDisabled.enabled = false;
        }

        // Check if groundPlatform is ready to spawn
        if (groundPreviewSelected) {
            switch (readyToSpawnGround)
            {
                case true:
                    groundDisabled.enabled = false;
                    break;
                case false:
                    groundDisabled.enabled = true;
                    break;
            }
        }
        else {
            groundDisabled.enabled = false;
        }
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

            switch (readyToSpawnGround) {
                case true:
                    groundDisabled.enabled = false;
                    break;
                case false:
                    groundDisabled.enabled = true;
                    break;
            }

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
            if (!readyToSpawnAir || !airPreviewSelected)
                return;

            SetAirPreview();
            readyToSpawnAir = false;
            StartCoroutine(StartAirCooldown());

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

            SetGroundPreview();
            readyToSpawnGround = false;
            StartCoroutine(StartGroundCooldown());

            // Despawn existing platform
            if (platformScript != null) {
                platformScript.DeletePlatform();
            }

            // Spawn platform at position
            platformScript = Instantiate(groundPrefab, groundPreview.transform.position, Quaternion.identity).GetComponent<Platform>();
        }
    }

    IEnumerator StartAirCooldown() {
        airCooldown = true;
        yield return new WaitForSeconds(maxAirCooldown);

        airCooldown = false;
    }

    IEnumerator StartGroundCooldown() {
        groundCooldown = true;
        yield return new WaitForSeconds(maxGroundCooldown);

        groundCooldown = false;
    }
    
    // Public Setters
    public void ReadyToSpawnAir(bool flag) {
        // If flag is true, check if airCooldown is done (false)
        if (flag && !airCooldown) { 
            readyToSpawnAir = true;
        }
        else if (!flag || airCooldown) { 
            readyToSpawnAir = false;
        }
    }

    public void ReadyToSpawnGround(bool flag) {
        // If flag is true, check if groundCooldown is done (false)
        if (flag && !groundCooldown) { 
            readyToSpawnGround = true;
        }
        else if (!flag || groundCooldown) { 
            readyToSpawnGround = false;
        }
    }
}
