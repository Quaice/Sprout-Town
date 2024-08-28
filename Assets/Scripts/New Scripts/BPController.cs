/*
 *  The BPController (read Board Piece Controller) controlls all handling of board pieces
 *  including creation, destruction, tracking, etc.
 */
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SproutTown {
    public class BPController : MonoBehaviour {

        [SerializeField]
        private GameObject[] _boardPiecePool; // The pool of game peices to utilize, set in Unity Editor
        public List<GameObject> activeBoardPieces = new List<GameObject>(); // The board pieces that are currently active on the board

        public GameObject GetNewBP() {
            int randomIndex = Random.Range(0, _boardPiecePool.Length);
            return Instantiate(_boardPiecePool[randomIndex], new Vector2(50,50), Quaternion.identity);
        }

        public void PlaceBPOnBoard(GameObject _activeBP) {
            _activeBP.layer = LayerMask.NameToLayer("board-piece");
            _activeBP.tag = "board-piece";
            if (!activeBoardPieces.Contains(_activeBP)) {
                activeBoardPieces.Add(_activeBP);
            }
        }

        public List<GameObject> CheckForMatches(GameObject _activeBP) {
            List<GameObject> matchingPieces = new List<GameObject>();
            HashSet<GameObject> visited = new HashSet<GameObject>();
            GetMatchingPieces(_activeBP, matchingPieces, visited);
            if (matchingPieces.Count > 1) {
                matchingPieces.Remove(_activeBP);
                return matchingPieces;
            } else {
                matchingPieces.Clear();
                return matchingPieces;
            }
        }

        void GetMatchingPieces(GameObject current, List<GameObject> matching, HashSet<GameObject> visited) {
            if (visited.Contains(current)) { return; }

            visited.Add(current);
            matching.Add(current);

            Vector2[] directions = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };

            foreach (Vector2 direction in directions) {
                Vector2 point = (Vector2)current.transform.position + direction;
                RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero, 1f, LayerMask.GetMask("board-piece"));
                if(hit.collider != null) {
                    PieceType myType = current.GetComponent<BoardPiece>().type;
                    PieceType hitType = hit.transform.GetComponent<BoardPiece>().type;
                    if (myType == hitType) {
                        GameObject nextPiece = hit.transform.gameObject;
                        if (!matching.Contains(nextPiece)) {
                            GetMatchingPieces(nextPiece, matching, visited);
                        }
                    }
                }
            }
        }
    }
}