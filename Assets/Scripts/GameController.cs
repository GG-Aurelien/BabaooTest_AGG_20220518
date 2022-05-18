using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int difficultyLevel = 10;

    [SerializeField]
    private GameObject SuccessPanel;
    [SerializeField]
    private Text GOTitle;


    private static List<int[]> ORIGINAL_NEIGHBOR = new List<int[]>() {
        new int[] { -1, 1, 3, -1 }, new int[] { -1, 2, 4, 0 }, new int[] { -1, -1, 5, 1 },
        new int[] { -1, 1, 3, -1 }, new int[] { -1, 2, 4, 0 }, new int[] { -1, -1, 5, 1 },
        new int[] { -1, 1, 3, -1 }, new int[] { -1, 2, 4, 0 }, new int[] { -1, -1, 5, 1 }
    };
    [SerializeField]
    private List<TileController> _allTiles;
    private TileController _blankTile;
    private List<Vector2> _originalPosition = new List<Vector2>();
    private float _timer = 180f;

    private void Start()
    {
        _timer = 180f;
        _allTiles.ForEach(tile =>
        {
            tile.OnTileSlied += Tile_OnTileSlied; ;
            _originalPosition.Add(tile.transform.position);
            if (tile.OriginalIndex == 4)
                _blankTile = tile;
        });
        InitiateGame(difficultyLevel);
    }

    private void Tile_OnTileSlied(TileController active, TileController target)
    {
        MoveTile(active, target);
    }

    private void MoveTile(TileController active, TileController target, bool checkGO = true)
    {
        TileController activeAbove = active.Above;
        TileController activeRight = active.Right;
        TileController activeBeneath = active.Beneath;
        TileController activeLeft = active.Left;

        TileController targetAbove =    target.Above;
        TileController targetRight =    target.Right;
        TileController targetBeneath =  target.Beneath;
        TileController targetLeft =     target.Left;

        active.Above = target.Above == active ? target : target.Above;
        active.Right = target.Right == active ? target : target.Right;
        active.Beneath = target.Beneath == active ? target : target.Beneath;
        active.Left = target.Left == active ? target : target.Left;
        target.Above = activeAbove == target ? active : activeAbove;
        target.Right = activeRight == target ? active : activeRight;
        target.Beneath = activeBeneath == target ? active : activeBeneath;
        target.Left = activeLeft == target ? active : activeLeft;

        if (activeLeft != null && activeLeft != target)
            activeLeft.Right = target;
        if (activeRight != null && activeRight != target)
            activeRight.Left = target;
        if (activeAbove != null && activeAbove != target)
            activeAbove.Beneath = target;
        if (activeBeneath != null && activeBeneath != target)
            activeBeneath.Above = target;

        if (targetLeft != null && targetLeft != active)
            targetLeft.Right = active;
        if (targetRight != null && targetRight != active)
            targetRight.Left = active;
        if (targetAbove != null && targetAbove != active)
            targetAbove.Beneath = active;
        if (targetBeneath != null && targetBeneath != active)
            targetBeneath.Above = active;

        int tmpindex = active.actualIndex;
        active.actualIndex = target.actualIndex;
        target.actualIndex = tmpindex;
        if(checkGO) CheckGameOver();
    }

    private void CheckGameOver() {
        bool isFinished = true;
        _allTiles.ForEach(tile => { isFinished &= tile.IsWellPlaced; });

        if (isFinished)
        {
            float oldsuccess = PlayerPrefs.HasKey("SuccessTime") ? PlayerPrefs.GetFloat("SuccessTime") : 800;
            if (oldsuccess > _timer)
                PlayerPrefs.SetFloat("SuccessTime", _timer);
            GOTitle.text = "BRAVO ! ";
            SuccessPanel.SetActive(true);
        }

        Debug.Log(isFinished);
    }

    private void InitiateGame(int maximumMove) {

        for (int i = 0; i < maximumMove; ++i) {
            TileController target = null;
            switch (Random.Range(0, 4))
            {
                case 0: target = _blankTile.Above; break;
                case 1: target = _blankTile.Right; break;
                case 2: target = _blankTile.Beneath; break;
                case 3: target = _blankTile.Left; break;

            }
            if (target != null)
                MoveTile(_blankTile, target, false);
        }

        List<Vector2> originalPosition = new List<Vector2>();
        _allTiles.ForEach(tile => { Debug.Log(tile.actualIndex); tile.AnchoredPosition = _originalPosition[tile.actualIndex]; });

    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                GOTitle.text = "Game Over !";
                SuccessPanel.SetActive(true);
            }
        }
    }
}
