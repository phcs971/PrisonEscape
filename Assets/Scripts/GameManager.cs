using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private Cell[] cells;
    private Guard[] guards;
    private Player player;

    private Cell selectedCell;
    private Cell lastCell;
    public Cell initialCell;

    public bool isPlayerInCell = true;

    public int points = 0;
    public int life = 3;

    [Header("Labels:")]

    public TextMeshProUGUI lifeLabel;
    public TextMeshProUGUI pointsLabel;

    void Start()
    {
        FindObjects();
        SetupCursor();
        RandomizeSelectedCell(initialCell);
        SetupGuards();
        SetupLabels();
    }

    void FindObjects()
    {
        cells = GameObject.FindObjectsOfType<Cell>();
        guards = GameObject.FindObjectsOfType<Guard>();
        player = GameObject.FindObjectOfType<Player>();

        lastCell = initialCell;
    }

    void SetupCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void SetupGuards()
    {
        foreach (Guard guard in guards)
        {
            DeployGuard(guard);
        }
    }

    void SetupLabels()
    {
        SetLife(life);
        SetPoints(points);
    }

    public void DeployGuard(Guard guard)
    {
        Cell cell;
        if (guard.currentDestination == null)
        {
            cell = GetRandomCell();
        } else
        {
            cell = GetRandomCell(excluding: guard.currentDestination);
        }
        guard.MoveTo(cell);
    }

    Cell GetRandomCell(Cell excluding = null)
    {
        var toSelect = cells.Where(cell => cell != excluding).ToArray();
        return toSelect[Random.Range(0, toSelect.Length)];
    }

    void RandomizeSelectedCell(Cell currentCell)
    {
        currentCell.cellLight.enabled = false;
        selectedCell = GetRandomCell(excluding: currentCell);
        selectedCell.cellLight.enabled = true;
    }

    public void DidEnterCell(Cell cell)
    {
        isPlayerInCell = true;
        lastCell = cell;
        if (cell == selectedCell) {
            SetPoints(points+1);
            RandomizeSelectedCell(cell);
        }
    }

    public void DidExitCell(Cell cell)
    {
        isPlayerInCell = false;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GuardDidSeePlayer()
    {
        if (!isPlayerInCell)
        {
            SetLife(life - 1);
            if (life == 0)
            {
                ResetScene();
            } else {
                player.transform.position = lastCell.transform.position;
            }
        }
    }

    public void SetPoints(int value)
    {
        points = value;
        pointsLabel.text = string.Format("Pontos: {0}", points);
    }

    public void SetLife(int value)
    {
        life = value;
        lifeLabel.text = string.Format("Vida: {0}", life);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
