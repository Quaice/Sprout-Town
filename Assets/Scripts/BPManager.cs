using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BPManager : MonoBehaviour {

    public GameObject[] _boardPiecePool;
    public List<GameObject> _piecesOnBoard = new List<GameObject>();

    public GameObject GetNewBoardPiece(Vector2 spawnPoint) {
        int randomBoardPiece = Random.Range(0, _boardPiecePool.Length);
        return Instantiate(_boardPiecePool[randomBoardPiece], spawnPoint, Quaternion.identity);
    }

    public void PlaceBPOnBoard(GameObject bp) {
        bp.layer = LayerMask.NameToLayer("board-piece");
        bp.tag = "board-piece";
        _piecesOnBoard.Add(bp);
    }
}