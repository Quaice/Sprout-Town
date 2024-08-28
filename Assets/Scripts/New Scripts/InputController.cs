/*
 * The input controller should handle all input, including mouse functions and keyboard input
 */

using System;
using UnityEngine;

namespace SproutTown {
    public class InputController : MonoBehaviour {

        public Vector2 mousePosition { get; private set; }  // An up-to-date vector2 of the mouse position within the game space
        private Vector2 _gridPosition = Vector2.zero;       // The current grid sqaure the mouse is in

        public event Action<GameObject> OnSnapToGrid;

        public void UpdateInputController(GameObject activeBP) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AttachBPToMouse(activeBP);
        }

        public void AttachBPToMouse(GameObject _activeBP) {
            if(MouseInPlayArea() && !MouseIsOverBP()) {
                _activeBP.SetActive(true);
                SnapBPToGrid(_activeBP);
            } else {
                _activeBP.SetActive(false);
            }
        }

        void SnapBPToGrid(GameObject _activeBP) {
            if(! MouseInsideGridBounds(mousePosition, _gridPosition)) {
                UpdateGridPosition();
                _activeBP.transform.position = _gridPosition;
                OnSnapToGrid?.Invoke(_activeBP);
            }
        }

        public bool MouseInPlayArea() {
            return (mousePosition.x >= GAMEBOARD.CONSTRAINTS["min"].x - 0.5f
                && mousePosition.x <= GAMEBOARD.CONSTRAINTS["max"].x + 0.5f)
                && (mousePosition.y >= GAMEBOARD.CONSTRAINTS["min"].y - 0.5f
                && mousePosition.y <= GAMEBOARD.CONSTRAINTS["max"].y + 0.5f);
        }

        public bool MouseIsOverBP() {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1f, LayerMask.GetMask("board-piece"));
            return hit.collider;
        }
        bool MouseInsideGridBounds(Vector2 v1, Vector2 v2, float tolerance = 0.5f) {
            Vector2 dif = v1 - v2;
            if (Mathf.Abs(dif.x) <= tolerance && Mathf.Abs(dif.y) <= tolerance) return true;
            return false;
        }

        void UpdateGridPosition() {
            _gridPosition = new Vector2 (
                Mathf.Round(mousePosition.x / GAMEBOARD.GRIDSIZE) * GAMEBOARD.GRIDSIZE,
                Mathf.Round(mousePosition.y / GAMEBOARD.GRIDSIZE) * GAMEBOARD.GRIDSIZE
            );
        }
    }
}