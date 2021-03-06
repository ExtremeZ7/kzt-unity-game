﻿using UnityEngine;
using System.Collections;
using Helper;
using Common.Extensions;
using Controllers;

public class LevelSelectWheelScript : MonoBehaviour
{

    public iTween.EaseType rotationEaseType;
    public iTween.EaseType spiralDownEaseType;
    public iTween.EaseType scaleUpEaseType;
    public iTween.EaseType scaleDownEaseType;
    public iTween.EaseType moveUpEaseType;
    public iTween.EaseType moveDownEaseType;

    [Space(10)]
    public Transform inventory;
    public Transform[] items = new Transform[4];
    public Transform[] itemHiders = new Transform[2];

    [Space(10)]
    public GUIStyle textStyle;
    public GUIStyle strokeStyle;

    [Space(10)]
    public GameObject enabledAudioSource;
    public GameObject snapAudioSource;
    public float snapDelay;
    public GameObject changeAudioSource;
    public GameObject disabledAudioSource;

    int lastSelectedLevelIndex = 1;
    int lastSelectionState;
    LevelSelectionScript levelSelectionScript;
    string levelName;

    int origFontSize = 40;
    int actualFontSize = 40;

    bool rotated;

    void Awake()
    {
        iTween.Init(gameObject);
    }

    void Start()
    {
        levelSelectionScript = FindObjectOfType<LevelSelectionScript>();
        UpdateLevelName();

        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (!GameController.Instance.paused)
        {
            if (levelSelectionScript.getSelectionState() == (int)LevelSelectionScript.SelectionState.SelectingALevel)
            {

                //When Level Select Is First Activated
                if (lastSelectionState != levelSelectionScript.getSelectionState())
                {
                    UpdateLevelName();
                    UpdateInventory();
                    StartCoroutine(OpenDoors(true, 0.75f));
                    StartCoroutine(CreateObjectAfterDelay(enabledAudioSource));
                    StartCoroutine(CreateObjectAfterDelay(snapAudioSource, snapDelay));

                    if (rotated)
                    {
                        iTween.StopByName("RotateWheel");
                        iTween.StopByName("SpiralDown");
                    }

                    iTween.StopByName("MoveDown");

                    iTween.ScaleTo(gameObject, iTween.Hash(
                            "scale", Vector3.one,
                            "time", 0.75f,
                            "easeType", scaleUpEaseType
                        ));

                    iTween.MoveTo(inventory.gameObject, iTween.Hash(
                            "name", "MoveUp",
                            "position", new Vector3(0f, -0.25f),
                            "islocal", true,
                            "time", 0.75f,
                            "easeType", moveUpEaseType
                        ));

                    transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 160f));
                }

                int levelSelectIndex = levelSelectionScript.getLevelSelectIndex();

                if (lastSelectedLevelIndex != levelSelectIndex)
                {
                    UpdateLevelName();
                    StartCoroutine(CreateObjectAfterDelay(changeAudioSource));
                    StartCoroutine(OpenDoors(false));
                    StopAllCoroutines();
                    StartCoroutine(OpenDoors(true, 0.5f));

                    iTween.RotateTo(gameObject, iTween.Hash(
                            "name", "RotateWheel",
                            "rotation", new Vector3(0f, 0f, 160 + (72 * (levelSelectIndex - 1))),
                            "time", 1.5f,
                            "easeType", rotationEaseType)
                    );

                    rotated = true;

                    iTween.PunchScale(transform.GetChild(5 - levelSelectIndex).gameObject, iTween.Hash(
                            "amount", new Vector3(1f, 1f, 0f),
                            "time", 1.0f,
                            "delay", 0.3f
                        ));

                    actualFontSize = 47;
                }

                lastSelectedLevelIndex = levelSelectIndex;
            }
            else
            {
                if (lastSelectionState != levelSelectionScript.getSelectionState())
                {
                    StopAllCoroutines();
                    StartCoroutine(CreateObjectAfterDelay(disabledAudioSource));
                    StartCoroutine(OpenDoors(false));

                    iTween.ScaleTo(gameObject, iTween.Hash(
                            "scale", Vector3.zero,
                            "time", 0.75f,
                            "easeType", scaleDownEaseType
                        ));

                    if (rotated)
                        iTween.StopByName("RotateWheel");
                    iTween.StopByName("MoveUp");

                    iTween.MoveTo(inventory.gameObject, iTween.Hash(
                            "name", "MoveDown",
                            "position", new Vector3(0f, -8.5f),
                            "islocal", true,
                            "time", 0.5f,
                            "easeType", moveDownEaseType
                        ));

                    iTween.RotateBy(gameObject, iTween.Hash(
                            "name", "SpiralDown",
                            "amount", new Vector3(0f, 0f, 1f),
                            "time", 0.75f,
                            "easeType", spiralDownEaseType
                        ));

                    rotated = true;

                    levelSelectionScript.setLevelSelectIndex(1);
                    lastSelectedLevelIndex = 1;
                }
            }

            lastSelectionState = levelSelectionScript.getSelectionState();

            textStyle.fontSize = (int)Mathf.Ceil(((float)actualFontSize) * transform.localScale.x);
            strokeStyle.fontSize = (int)Mathf.Ceil(((float)actualFontSize) * transform.localScale.x);

            actualFontSize = Help.IntMoveTowards(actualFontSize, origFontSize, 2);
        }
    }

    void UpdateInventory()
    {
        SetSprites(levelSelectionScript.getLevelSelectIndex(), levelSelectionScript.getWorldSelectIndex());
        SetItemLocations(levelSelectionScript.getLevelSelectIndex(), levelSelectionScript.getWorldSelectIndex());
    }

    void SetSprites(int levelIndex, int worldIndex)
    {
        //Set X Marks
        for (int i = 3; i >= 0; i--)
            transform.GetChild(i).GetChild(0).gameObject.SetActive(
                GameController.Instance.restrictLocked && !GameController.Instance.progress.levelsUnlocked[worldIndex, 4 - i]);
		
        //Set Boss Head
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = 
            (Sprite)Res.GetCharacterProfile(worldIndex);

        //Set Inventory Boss Head
        inventory.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = 
            (Sprite)Res.GetCharacterProfile(worldIndex);

        //Set Gem Sprite
        items[0].GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = Res.GetInventoryItemSprite(0, levelIndex, worldIndex) as Sprite;

        //Set Gem Transform
        items[0].localPosition = new Vector2(items[0].localPosition.x, levelIndex != 4 ? -0.85f : -2.5f);
			
        //Set KZT Sprites
        foreach (Transform item in items.SubArray(1,3))
        {
            item.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = levelIndex < 4;
        }

        //Set All Item Transforms
        if (levelIndex > 0 && levelIndex < 5)
        {
            items[0].localPosition = new Vector3(
                Mathf.Abs(items[0].localPosition.x) * (GameController.Instance.progress.gemsCollected[worldIndex - 1, levelIndex - 1] ? -1 : 1), items[0].localPosition.y);
            if (levelIndex < 4)
            {
                for (int i = 0; i < 3; i++)
                {
                    items[i + 1].localPosition = new Vector3(
                        Mathf.Abs(items[i + 1].localPosition.x) * (GameController.Instance.progress.lettersCollected[worldIndex - 1, levelIndex - 1, i] ? -1 : 1), items[i + 1].localPosition.y);
                }
            }
        }
    }

    void SetItemLocations(int levelIndex, int worldIndex)
    {
        if (levelIndex < 5)
        {
            Vector2 gemPosition = items[0].localPosition;
            gemPosition.x = Mathf.Abs(gemPosition.x) * (GameController.Instance.progress.gemsCollected[worldIndex - 1, levelIndex - 1] ? -1 : 1);
            items[0].localPosition = gemPosition;

            if (levelIndex < 4)
            {
                foreach (Transform item in items.SubArray(1,3))
                {
                    Vector2 letterPosition = item.localPosition;
                    letterPosition.x = Mathf.Abs(letterPosition.x) * (GameController.Instance.progress.lettersCollected[worldIndex - 1, levelIndex - 1, items.IndexOf(item) - 1] ? -1 : 1);
                    item.localPosition = letterPosition;
                }
            }
        }
    }

    void UpdateLevelName()
    {
        levelName = Help.GetLevelTag(levelSelectionScript.getWorldSelectIndex(), levelSelectionScript.getLevelSelectIndex());
        levelName = levelName + "\n" + Help.GetLevelName(levelSelectionScript.getWorldSelectIndex(), levelSelectionScript.getLevelSelectIndex());
    }

    public bool AttemptLevelEntry(int worldSelectIndex, int levelSelectIndex)
    {

        bool levelIsUnlocked = GameController.Instance.progress.levelsUnlocked[worldSelectIndex - 1, levelSelectIndex - 1];

        //Shake the level object if the level is locked
        if (GameController.Instance.restrictLocked && !levelIsUnlocked)
        {
            iTween.ShakeRotation(transform.GetChild(5 - levelSelectIndex).gameObject, iTween.Hash(
                    "amount", new Vector3(0f, 0f, 45f),
                    "time", 0.25f,
                    "space", Space.Self)
            );
        }

        return levelIsUnlocked;
    }

    void OnGUI()
    {
        if (transform.localScale != Vector3.zero && transform.localScale.x > 0.25f)
        {
            float standardScreenWidth = GameController.Instance.standardScreenWidth;
            float standardScreenHeight = GameController.Instance.standardScreenHeight;

            float xScale = Screen.width / standardScreenWidth;
            float yScale = Screen.height / standardScreenHeight;
            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(xScale, yScale, 1));

            float rectWidth = 200;
            float rectHeight = 100;

            int levelIndex = levelSelectionScript.getLevelSelectIndex();

            switch (levelIndex)
            {
                case 1:
                case 2:
                case 3:
                    textStyle.normal.textColor = new Color(1.0f, 1.0f, 0.0f);
                    strokeStyle.normal.textColor = new Color(1.0f, 0.0f, 1.0f);
                    break;
                case 4:
                    textStyle.normal.textColor = new Color(0.25f, 1.0f, 1.0f);
                    strokeStyle.normal.textColor = new Color(0.0f, 0.0f, 0.0f);
                    break;
                case 5:
                    textStyle.normal.textColor = new Color(1.0f, 0.0f, 0.0f);
                    strokeStyle.normal.textColor = new Color(1.0f, 1.0f, 0.0f);
                    break;
            }

            Rect levelNameRect = new Rect(standardScreenWidth / 2 - rectWidth / 2, standardScreenHeight / 2 - rectHeight / 2, rectWidth, rectHeight);

            GUI.Label(levelNameRect, levelName, strokeStyle);
            GUI.Label(levelNameRect, levelName, textStyle);
        }
    }

    IEnumerator OpenDoors(bool open, float delay = 0f)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        foreach (Transform hider in itemHiders)
        {
            foreach (Animator animator in hider.GetComponentsInChildren<Animator>())
            {
                animator.SetBool("Open", open);
                if (open)
                    UpdateInventory();
            }
        }
        yield return null;
    }

    IEnumerator CreateObjectAfterDelay(GameObject prefab, float delay = 0f)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
