using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] Pipe powerSource;
    public List<SubSystem> subSystems;

    public GridMap<GameObject> puzzle;
    public List<Pipe> allPipes;
    private Pathfinding pathfinding;

    [SerializeField] GameObject subSystemDisplay;
    [SerializeField] GameObject poweredItemsPanel;

    [SerializeField] GameObject pipePrefab;
    List<GameObject> targetPipes = new List<GameObject>();

    private Inventory inventory;
    void Awake()
    {
        InitSingleton(this);

        allPipes = new List<Pipe>(FindObjectsOfType<Pipe>());
        SetUpGrid();
        SetUpPuzzle();
        MakeNeighbors();
        pathfinding = new Pathfinding(puzzle);

        FindSubSystems();

        inventory = new Inventory();
    }
    void Start()
    {
        CheckConnections();
        foreach (Pipe p in allPipes)
        {
            if (p.GetComponent<SubSystem>() == null)
            {
                targetPipes.Add(p.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DamageRandomPipe();
        }
    }

    public void DamageRandomPipe()
    {
        // Get the pipe to damage and the replacement
        GameObject target = targetPipes[Random.Range(0, targetPipes.Count)];
        GameObject replacement = Instantiate(pipePrefab, transform);
        // Put the replacement at the same sibling index and remove and destroy the target
        replacement.transform.SetSiblingIndex(target.transform.GetSiblingIndex());
        target.transform.SetParent(null);
        Destroy(target);
        // Redo the puzzle and pathfinding. 
        SetUpPuzzle();
        MakeNeighbors();
        pathfinding = new Pathfinding(puzzle);
        CheckConnections();
    }

    private void FindSubSystems()
    {
        foreach (Pipe p in allPipes)
        {
            if (p.TryGetComponent(out SubSystem sub))
            {
                subSystems.Add(sub);

            }
        }
        DisplaySubSystems();
    }

    private void DisplaySubSystems()
    {
        foreach (SubSystem sub in subSystems)
        {
            GameObject display = Instantiate(subSystemDisplay, poweredItemsPanel.transform);
            Text t = display.GetComponentInChildren<Text>();
            t.text = sub.systemName;
            sub.bgSlider = display.GetComponent<Slider>();
            sub.powerImg = display.transform.GetChild(1).GetComponent<Image>();
            sub.powerImg.transform.GetChild(0).GetComponent<Image>().sprite = sub.icon;
            Instantiate(sub.powerImg, sub.transform);
        }
    }
    private void SetUpPuzzle()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int x = i % puzzle.Width;
            int y = i / puzzle.Height;

            var item = transform.GetChild(i);
            item.GetComponent<Pipe>().X = x;
            item.GetComponent<Pipe>().Y = y;


            puzzle.gridArray[x, y] = item.gameObject;
        }

    }

    private void SetUpGrid()
    {
        GameObject powerPanel = GameObject.Find("Power Con Panel");

        Vector2 size = powerPanel.GetComponent<FlexibleGridLayout>().size;
        Vector2 pos = new Vector2(0, 0);

        puzzle = new GridMap<GameObject>((int)size.x, (int)size.y, pos);
    }

    private void MakeNeighbors()
    {
        for (int y = 0; y < puzzle.Height; y++)
        {
            for (int x = 0; x < puzzle.Width; x++)
            {
                int modedY = (puzzle.Height-1) - y;
                Pipe tile = puzzle.gridArray[x, y].GetComponent<Pipe>();

                if (x > 0)
                {
                    Pipe.MakeEastWestNeighbors( puzzle.gridArray[x - 1, y].GetComponent<Pipe>(), tile);
                }
                if (y > 0)
                {
                    Pipe.MakeNorthSouthNeighbors( puzzle.gridArray[x, y - 1].GetComponent<Pipe>(), tile);
                }
            }
        }
    }

    private void Shuffle()
    {
        foreach (GameObject pipeGO in puzzle.gridArray)
        {
            int rotAmount = Random.Range(0, 4);
            for (int i = 0; i < rotAmount; i++)
            {
                pipeGO.GetComponent<Pipe>().RotatePipe();
            }
        }
    }

    public void CheckConnections()
    {
        // Turn off all pipes that dont provide power. 
        foreach (GameObject pipeGO in puzzle.gridArray)
        {
            pipeGO.GetComponent<Pipe>().isPowered = pipeGO.GetComponent<Pipe>().providesPower;
        }
        // Check each subsystem to see what is powered. 
        foreach (SubSystem system in subSystems)
        {

            List<Pipe> pathOne = pathfinding.FindPath(powerSource.X, powerSource.Y, system.pipe.X, system.pipe.Y);
            if (null != pathOne)
            {
                
                foreach (Pipe pipe in pathOne)
                {
                    pipe.isPowered = true;
                }
            }

        }
        UpdatePipeDisplay();
    }

    private void UpdatePipeDisplay()
    {
        foreach (GameObject pipeGO in puzzle.gridArray)
        {
            pipeGO.GetComponent<Image>().color = pipeGO.GetComponent<Pipe>().isPowered ? Color.red : Color.white;
        }
    }
}
