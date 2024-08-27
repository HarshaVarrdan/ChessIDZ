using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chess.Scripts.Core;


public class ChessAIPlacementHandler : MonoBehaviour
{
    [SerializeField] public int row, column;

    public bool canPlay;
    public bool canAttack = false;

    bool isFirst = true;

    internal static ChessAIPlacementHandler CAI_Instance;


    private void Awake() 
    {
        CAI_Instance = this;
    }
    private void Start() 
    {
        transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndAI()
    {
        Debug.Log(this.gameObject.name + " Selected");

        /*int endvalue = 0;*/
        if(ChessBoardPlacementHandler.Instance._HL[0] == null || !canPlay)
        {
            ChessBoardPlacementHandler.Instance.StartAI();
            return;
        }
        for(int i =0; i < ChessBoardPlacementHandler.Instance.HLCount; i++)
        {
            //Debug.Log(ChessBoardPlacementHandler.Instance._HL[i].GetComponent<HL_Manager>().row + ChessBoardPlacementHandler.Instance._HL[i].GetComponent<HL_Manager>().col);
            if(ChessBoardPlacementHandler.Instance._HL[i].name == "Enemy_HL(Clone)")
            {
                ChessBoardPlacementHandler.Instance._HL[i].GetComponent<HL_Manager>().OverEnemy();
                row = ChessBoardPlacementHandler.Instance._HL[i].GetComponent<HL_Manager>().row;
                column = ChessBoardPlacementHandler.Instance._HL[i].GetComponent<HL_Manager>().col;
                transform.position = ChessBoardPlacementHandler.Instance._HL[i].transform.position;
                ChessBoardPlacementHandler.Instance.ClearHighlights();
                ChessBoardPlacementHandler.Instance.selectedPiece = null;
                ChessBoardPlacementHandler.Instance.canPlayerPlay = true;
                return;
            }
        }
        GameObject hl = ChessBoardPlacementHandler.Instance._HL[Random.Range(0,ChessBoardPlacementHandler.Instance.HLCount)];
        transform.position = hl.transform.position;
        row = hl.GetComponent<HL_Manager>().row;
        column = hl.GetComponent<HL_Manager>().col;
        ChessPlayerPlacementHandler.CP_Instance.canPlay = true;
        ChessBoardPlacementHandler.Instance.ClearHighlights();
        ChessBoardPlacementHandler.Instance.selectedPiece = null;
        ChessBoardPlacementHandler.Instance.canPlayerPlay = true;

    }


    public bool CanAttack()
    {
        Move(true);
        return canAttack;
    }


    public void Move(bool check)
    {
        ChessBoardPlacementHandler.Instance.selectedPiece = this.gameObject;
        ChessBoardPlacementHandler.Instance.ClearHighlights();

        if(this.gameObject.name == "White_King")
        {
            MoveUp(1);
            MoveDown(1);
            MoveRight(1);
            MoveLeft(1);
            MoveRightCrossUp(1);
            MoveLeftCrossUp(1);
            MoveRightCrossDown(1);
            MoveLeftCrossDown(1);
        }
        else if(this.gameObject.name == "White_Queen")
        {
            MoveUp(8);
            MoveDown(8);
            MoveRight(8);
            MoveLeft(8);
            MoveRightCrossUp(8);
            MoveLeftCrossUp(8);
            MoveRightCrossDown(8);
            MoveLeftCrossDown(8);
        }
        else if(this.gameObject.name == "White_Pawn")
        {
            if(isFirst)
            {
                MoveDown(2,true);
                PawnAttack();
                isFirst = false;
            }
            else
            {
                MoveDown(1,true);
                PawnAttack();
            }
        }
        else if(this.gameObject.name == "White_Bishop")
        {
            MoveRightCrossUp(8);
            MoveLeftCrossUp(8);
            MoveRightCrossDown(8);
            MoveLeftCrossDown(8);
        }
        else if(this.gameObject.name == "White_Knight")
        {
            MoveKnight();
        }
        else if(this.gameObject.name == "White_Rook")
        {
            MoveUp(8);
            MoveDown(8);
            MoveRight(8);
            MoveLeft(8);
        }
        if(check)
        {
            for(int i =0; i < ChessBoardPlacementHandler.Instance.HLCount; i++)
            {
                if(ChessBoardPlacementHandler.Instance._HL[i].name == "Enemy_HL(Clone)")
                {
                    canAttack = true;
                    return;
                }
            }
            return;
        }
        EndAI();
    }

    void MoveUp(int MaxMove)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row + i, column))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row + i,column);
            }
            else{ return; }
        }
    }
    void MoveDown(int MaxMove, bool isPawn = false)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(isPawn)
            {
                if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - i, column, true))
                {
                    ChessBoardPlacementHandler.Instance.Highlight(row - i,column);
                }
                else{ return; }
            }

            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - i, column))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row - i,column);
            }
            else{ return; }
        }
    }
    void MoveRight(int MaxMove)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row, column + i))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row, column + i);
            }
            else{ return; }
        }
    }
    void MoveLeft(int MaxMove)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row, column - i))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row, column - i);
            }
            else{ return; }
        }
    }
    void MoveRightCrossUp(int MaxMove)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row + i, column + i))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row + i,column + i);
            }
            else{ return; }
        }
    }
    void MoveLeftCrossUp(int MaxMove)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row + i, column - i))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row + i,column - i);
            }
            else{ return; }
        }
    }
    void MoveRightCrossDown(int MaxMove)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - i, column + i))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row - i, column + i);
            }
            else{ return; }
        }
    }
    void MoveLeftCrossDown(int MaxMove)
    {
        for(int i = 1; i < MaxMove + 1; i++)
        {
            if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - i, column - i))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row - i, column - i);
            }
            else{ return; }
        }
    }
    void MoveKnight()
    {
        //Up
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row + 2, column + 1))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row + 2, column + 1);
        }
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row + 2, column - 1))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row + 2, column - 1);
        }

        //Down
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - 2, column - 1))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row - 2, column - 1);
        }
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - 2, column + 1))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row - 2, column + 1);
        }

        //Right
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row + 1, column + 2))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row + 1, column + 2);
        }
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - 1, column + 2))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row - 1, column + 2);
        }

        //Left
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row + 1, column - 2))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row + 1, column - 2);
        }
        if(ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - 1, column - 2))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row - 1, column - 2);
        }
    }

    void PawnAttack()
        {
            ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - 1, column + 1);
            ChessBoardPlacementHandler.Instance.checkForEnemyPiece(row - 1, column - 1);
        }
}
