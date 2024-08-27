using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

namespace Chess.Scripts.Core {
    public class ChessPlayerPlacementHandler : MonoBehaviour
    {
        [SerializeField] public int row, column;

        public bool canPlay = true;
        bool isFirst = true;

        internal static ChessPlayerPlacementHandler CP_Instance;


        private void Awake() 
        {
            CP_Instance = this;
        }
        private void Start() 
        {
            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
        }

        public void movePiece(int r, int c)
        {
            row = r;
            column = c;

            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
            ChessBoardPlacementHandler.Instance.ClearHighlights();
            ChessBoardPlacementHandler.Instance.selectedPiece = null;
            ChessBoardPlacementHandler.Instance.canPlayerPlay = false;
            StartCoroutine(TriggerAI());
        }

        IEnumerator TriggerAI()
        {
            yield return new WaitForSeconds(2f);
            ChessBoardPlacementHandler.Instance.StartAI();
        }

        void OnMouseDown()
        {
            if(ChessBoardPlacementHandler.Instance.canPlayerPlay)
            {
                if(canPlay)
                {   
                    ChessBoardPlacementHandler.Instance.selectedPiece = this.gameObject;
                    ChessBoardPlacementHandler.Instance.ClearHighlights();

                    if(this.gameObject.name == "King")
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
                    else if(this.gameObject.name == "Queen")
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
                    else if(this.gameObject.name == "Pawn")
                    {
                        if(isFirst)
                        {
                            MoveUp(2,true);
                            PawnAttack();
                            isFirst = false;
                        }
                        else
                        {
                            MoveUp(1,true);
                            PawnAttack();
                        }
                    }
                    else if(this.gameObject.name == "Bishop")
                    {
                        MoveRightCrossUp(8);
                        MoveLeftCrossUp(8);
                        MoveRightCrossDown(8);
                        MoveLeftCrossDown(8);
                    }
                    else if(this.gameObject.name == "Knight")
                    {
                        MoveKnight();
                    }
                    else if(this.gameObject.name == "Rook")
                    {
                        MoveUp(8);
                        MoveDown(8);
                        MoveRight(8);
                        MoveLeft(8);
                    }
                }
            }
        }

        void MoveUp(int MaxMove, bool isPawn = false)
        {
            for(int i = 1; i < MaxMove + 1; i++)
            {
                if(isPawn)
                {
                    if(ChessBoardPlacementHandler.Instance.checkForPiece(row + i, column, true))
                    {
                        ChessBoardPlacementHandler.Instance.Highlight(row + i,column);
                    }
                    else{ return; }
                }
                else
                {
                    if(ChessBoardPlacementHandler.Instance.checkForPiece(row + i, column))
                    {
                        ChessBoardPlacementHandler.Instance.Highlight(row + i,column);
                    }
                    else{ return; }
                }
            }
        }
        void MoveDown(int MaxMove)
        {
            for(int i = 1; i < MaxMove + 1; i++)
                {
                    if(ChessBoardPlacementHandler.Instance.checkForPiece(row - i, column))
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
                if(ChessBoardPlacementHandler.Instance.checkForPiece(row, column + i))
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
                if(ChessBoardPlacementHandler.Instance.checkForPiece(row, column - i))
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
                if(ChessBoardPlacementHandler.Instance.checkForPiece(row + i, column + i))
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
                if(ChessBoardPlacementHandler.Instance.checkForPiece(row + i, column - i))
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
                if(ChessBoardPlacementHandler.Instance.checkForPiece(row - i, column + i))
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
                if(ChessBoardPlacementHandler.Instance.checkForPiece(row - i, column - i))
                {
                    ChessBoardPlacementHandler.Instance.Highlight(row - i, column - i);
                }
                else{ return; }
            }
        }
        void MoveKnight()
        {
            //Up
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row + 2, column + 1))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row + 2, column + 1);
            }
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row + 2, column - 1))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row + 2, column - 1);
            }

            //Down
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row - 2, column - 1))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row - 2, column - 1);
            }
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row - 2, column + 1))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row - 2, column + 1);
            }

            //Right
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row + 1, column + 2))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row + 1, column + 2);
            }
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row - 1, column + 2))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row - 1, column + 2);
            }

            //Left
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row + 1, column - 2))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row + 1, column - 2);
            }
            if(ChessBoardPlacementHandler.Instance.checkForPiece(row - 1, column - 2))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row - 1, column - 2);
            }
        }

        void PawnAttack()
        {
            ChessBoardPlacementHandler.Instance.checkForPiece(row + 1, column + 1);
            ChessBoardPlacementHandler.Instance.checkForPiece(row + 1, column - 1);
        }

    }

}