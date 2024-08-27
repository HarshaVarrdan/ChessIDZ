using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using Chess.Scripts.Core;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public sealed class ChessBoardPlacementHandler : MonoBehaviour {

    [SerializeField] private GameObject[] enemyGO;
    [SerializeField] private GameObject[] _Pieces;
    [SerializeField] private GameObject[] _rowsArray;
    [SerializeField] private GameObject[] _outRowsArray;

    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private GameObject _enemyhighlightPrefab;
    [SerializeField] private GameObject _EnemyKing;
    [SerializeField] private GameObject _PlayerKing;

    public GameObject[] _HL = new GameObject[64];
    public GameObject[,] _chessBoard;
    public GameObject[,] _outBoard;
    public GameObject selectedPiece;

    public int PlayerScore = 0;
    public int EnemyScore = 0;
    public int HLCount = 0;

    public bool canPlayerPlay = true;
    internal static ChessBoardPlacementHandler Instance;

    private void Awake() {
        Instance = this;
        GenerateArray();
        GenerateOutArray();
    }

    private void GenerateOutArray()
    {
        _outBoard = new GameObject[4, 8];
        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 8; j++) {
                _outBoard[i, j] = _outRowsArray[i].transform.GetChild(j).gameObject;
            }
        }
    }

    private void GenerateArray() {
        _chessBoard = new GameObject[8, 8];
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++) {
                _chessBoard[i, j] = _rowsArray[i].transform.GetChild(j).gameObject;
            }
        }
    }

    internal GameObject GetOutTile(int i) {
        if(i > 7)
        {
            try {
                return _outBoard[i/8, i % 8];
            } catch (Exception) {
                Debug.LogError("Invalid row or column.");
                return null;
            }
        }
        else
        {
            try {
                return _outBoard[0, i];
            } catch (Exception) {
                Debug.LogError("Invalid row or column.");
                return null;
            }
        }
    }

    internal GameObject GetTile(int i, int j) {
        try {
            return _chessBoard[i, j];
        } catch (Exception) {
            Debug.LogError("Invalid row or column.");
            return null;
        }
    }

    internal void Highlight(int row, int col) {
        if (GetTile(row, col) == null){ return; }
        var tile = GetTile(row, col).transform;
        _HL[HLCount] = Instantiate(_highlightPrefab, tile.transform.position, Quaternion.identity, tile.transform) as GameObject;
        _HL[HLCount].GetComponent<HL_Manager>().setRow(row,col,false,null);
        HLCount += 1;
    }

    internal bool checkForPiece(int row,int col,bool NoAttack = false){
        bool temp = false;
        //Debug.Log("Lenght : " + _Pieces.Length);
        if(row < 0 || col < 0 || row > 7 || col > 7){ return false;}
        for(int k = 0; k < _Pieces.Length ; k++)
        {
            if(GetTile(row,col) == null)
            { 
                return false;
            }
            else
            {
                if( _Pieces[k].transform.position == _chessBoard[row,col].transform.position && _Pieces[k].GetComponent<ChessPlayerPlacementHandler>().canPlay )
                {
                    Debug.Log(_Pieces[k].transform.position + " = " + _chessBoard[row,col].transform.position);
                    return false;
                }
                else if(NoAttack && enemyGO[k].transform.position == _chessBoard[row,col].transform.position && enemyGO[k].GetComponent<ChessAIPlacementHandler>().canPlay)
                {
                    return false;
                }
                else if(enemyGO[k].transform.position == _chessBoard[row,col].transform.position && enemyGO[k].GetComponent<ChessAIPlacementHandler>().canPlay && !NoAttack)
                {
                    var tile = GetTile(row, col).transform;
                    _HL[HLCount] = Instantiate(_enemyhighlightPrefab, tile.transform.position, Quaternion.identity, tile.transform) as GameObject;
                    _HL[HLCount].GetComponent<HL_Manager>().setRow(row,col,true,enemyGO[k]);
                    HLCount += 1;

                    return false;   
    
                }
                else{
                    temp = true;
                }
            }
        }
        return temp;
    }

    internal bool checkForEnemyPiece(int row,int col,bool NoAttack = false){
        bool temp = false;
        //Debug.Log("Lenght : " + _Pieces.Length);
        if(row < 0 || col < 0 || row > 7 || col > 7){ return false;}
        for(int k = 0; k < _Pieces.Length ; k++)
        {
            Debug.Log(k);
            if(GetTile(row,col) == null)
            { 
                return false;
            }
            else
            {
                if( enemyGO[k].transform.position == _chessBoard[row,col].transform.position && enemyGO[k].GetComponent<ChessAIPlacementHandler>().canPlay)
                {
                    Debug.Log(enemyGO[k].transform.position + " = " + _chessBoard[row,col].transform.position );
                    return false;
                }
                else if(NoAttack && _Pieces[k].transform.position == _chessBoard[row,col].transform.position && _Pieces[k].GetComponent<ChessPlayerPlacementHandler>().canPlay)
                {
                    return false;
                }
                else if(_Pieces[k].transform.position == _chessBoard[row,col].transform.position && _Pieces[k].GetComponent<ChessPlayerPlacementHandler>().canPlay && !NoAttack)
                {
                    var tile = GetTile(row, col).transform;
                    _HL[HLCount] = Instantiate(_enemyhighlightPrefab, tile.transform.position, Quaternion.identity, tile.transform) as GameObject;
                    _HL[HLCount].GetComponent<HL_Manager>().setRow(row,col,true,_Pieces[k]);
                    Debug.Log(_HL[HLCount].name + " " + _Pieces[k].name);
                    HLCount += 1;

                    return false;   
    
                }
                else{
                    temp = true;
                }
            }
        }
        return temp;
    }

    internal void ClearHighlights() {
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++) {
                var tile = GetTile(i, j);
                if (tile.transform.childCount <= 0) continue;
                foreach (Transform childTransform in tile.transform) {
                    Destroy(childTransform.gameObject);
                }
            }
        }
        _HL = new GameObject[64];
        HLCount = 0;
    }

    internal void CheckAttack(GameObject checkGO,GameObject attackGO = null,bool CheckKingInDanger = false,bool isEnemy = false)
    {
        if(isEnemy)
        {
            if(attackGO != null)
            { 
                for(int k = 0; k < _Pieces.Length ; k++)
                {
                    if(_Pieces[k].GetComponent<ChessPlayerPlacementHandler>().canPlay && _Pieces[k] == attackGO)
                    {
                        Debug.Log("Can Attack : " + _Pieces[k].name);
                        return;
                    }
                }
            }
            if(CheckKingInDanger)
            {
                
            }
        }
    }

    internal void StartAI()
    {
        for(int i = 0; i < enemyGO.Length; i++)
        {
            if(enemyGO[i].GetComponent<ChessAIPlacementHandler>().canPlay)
            {
                if(enemyGO[i].GetComponent<ChessAIPlacementHandler>().CanAttack())
                {
                    enemyGO[i].GetComponent<ChessAIPlacementHandler>().Move(false);
                    Debug.Log(enemyGO[i].name + " Can Attack");
                    return;
                }
            }
        }
        Debug.Log("Nothing Can Attack");
        GameObject selEnemy = enemyGO[UnityEngine.Random.Range(0,enemyGO.Length)];
        if(!selEnemy.GetComponent<ChessAIPlacementHandler>().canPlay){ StartAI(); return;}  
        selEnemy.GetComponent<ChessAIPlacementHandler>().Move(false);
        Debug.Log(selEnemy.name);
    }
    
}