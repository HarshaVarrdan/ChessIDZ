using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chess.Scripts.Core;

public class HL_Manager : MonoBehaviour
{

    public int row, col;
    public bool isOverEnemy = false;
    public GameObject overGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRow(int r, int c, bool e, GameObject go)
    {
        row = r;
        col = c;
        isOverEnemy = e;
        overGO = go;
    }

    void OnMouseDown()
    {
        if(isOverEnemy)
        {
            overGO.GetComponent<ChessAIPlacementHandler>().canPlay = false;
            overGO.transform.position = ChessBoardPlacementHandler.Instance.GetOutTile(ChessBoardPlacementHandler.Instance.PlayerScore).transform.position;
            ChessBoardPlacementHandler.Instance.PlayerScore += 1;
        }
        ChessBoardPlacementHandler.Instance.selectedPiece.GetComponent<ChessPlayerPlacementHandler>().movePiece(row,col);
    }

    public void OverEnemy()
    {
        overGO.GetComponent<ChessPlayerPlacementHandler>().canPlay = false;
        overGO.transform.position = ChessBoardPlacementHandler.Instance.GetOutTile(16 - ChessBoardPlacementHandler.Instance.EnemyScore).transform.position;
        ChessBoardPlacementHandler.Instance.EnemyScore += 1;
    }
}
