using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    private Vector2 lastTapPos;
    private Vector3 startRotation;

    public Transform topTransform;
    public Transform goalTransform;
    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();
    private float helixDistance;
    private List<GameObject> spawnedLevels = new List<GameObject>();

    void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - goalTransform.localPosition.y;

        LoadStage(0);
    }

    void Update()
    {
        // When the mouse button is held down
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;

            // First frame of dragging — store initial position
            if (lastTapPos == Vector2.zero)
                lastTapPos = curTapPos;

            // Calculate horizontal movement delta
            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            // Rotate helix around Y-axis (left–right)
            transform.Rotate(Vector3.up * delta);
        }

        // When the mouse button is released, reset
        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }


    }

    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];

        if (allStages == null || allStages.Count == 0)
        {
            Debug.LogError("allStages list is empty or not assigned!");
            return;
        }

        if (stageNumber < 0 || stageNumber >= allStages.Count)
        {
            Debug.LogError("Stage number " + stageNumber + " is out of range! allStages count: " + allStages.Count);
            return;
        }

      // safe to access now

        if (stage == null)
        {
            Debug.LogError("No Stage " + stageNumber + " found in allStages List. Are all stages assigned in the list?");
            return;
        }

        // Change Stage Background Color
        Camera.main.backgroundColor = stage.stageBackgroundColor;

        // Change Ball Color
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = stage.stageBallColor;


        // Reset Helix Rotation
        transform.localEulerAngles = startRotation;

        // destroy the old levels if there are any
        foreach (GameObject go in spawnedLevels)
            Destroy(go);

        // create new level / platforms
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            // Creates level within scene
            GameObject level = Instantiate(helixLevelPrefab, transform);
            Debug.Log("Levels spawned");
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            level.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360f), 0);

            spawnedLevels.Add(level);

            // Creating the Gaps
            int partsToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disableParts = new List<GameObject>();

            while (disableParts.Count < partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disableParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disableParts.Add(randomPart);
                }
            }

            List<GameObject> leftParts = new List<GameObject>();

            foreach (Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
                
                if (t.gameObject.activeInHierarchy)
                    leftParts.Add(t.gameObject);
            }



            // Creating the deathparts
            List<GameObject> deathParts = new List<GameObject>();

            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];
                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            }






        }
    }
}
   