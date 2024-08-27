using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BPManager : MonoBehaviour {

    private BPAnimator _bpAnimator;

    public GameObject[] _boardPiecePool;
    public List<GameObject> _piecesOnBoard = new List<GameObject>();

    private void Awake() {
        _bpAnimator = GetComponent<BPAnimator>();
    }

    public GameObject GetNewBoardPiece(Vector2 spawnPoint) {
        int randomBoardPiece = Random.Range(0, _boardPiecePool.Length);
        return Instantiate(_boardPiecePool[randomBoardPiece], spawnPoint, Quaternion.identity);
    }

    public void PlaceBPOnBoard(GameObject BP) {
        BP.layer = LayerMask.NameToLayer("board-piece");
        BP.tag = "board-piece";
        if (!_piecesOnBoard.Contains(BP)) {
            _piecesOnBoard.Add(BP);
        }
    }

    public void OnBoardPieceSnap(GameObject BP) {
        CheckStuff(BP);
    }

    void CheckStuff(GameObject BP) {
        List<GameObject> _matches = new List<GameObject>();
        float bpX = BP.transform.position.x;
        float bpY = BP.transform.position.y;

        foreach (Vector2 dir in GAMEBOARD.DIRECTION.Values) {
            Vector2 spotCheck = new Vector2(bpX + dir.x, bpY + dir.y);
            RaycastHit2D piece = Physics2D.Raycast(spotCheck, Vector2.zero, 1f, LayerMask.GetMask("board-piece"));
            if (piece.collider != null) {
                PieceType myType = BP.GetComponent<BoardPiece>().type;
                PieceType hitType = piece.transform.GetComponent<BoardPiece>().type;
                if (myType == hitType) {
                    // TODO: Check the matches for any adjacent matches
                    piece.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
                    _matches.Add(piece.transform.gameObject);
                }
            }
        }

        if (_matches.Count > 1) {
            foreach(GameObject match in _matches) {
                _bpAnimator.AnimateMatches(_matches, BP.transform.position);
            }
        }
    }
}