using System;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public event Action<GameObject> OnSnapToGrid;

    // The current position of the mouse on the game screen
    public Vector2 mousePosition;
    private Vector2 gridPos = Vector2.zero;

    // Is the mouse over another board piece or not?
    public bool mouseOverBP = false;

    
    public bool MouseInPlayArea() {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (pos.x >= GAMEBOARD.CONSTRAINTS["min"].x -0.5f && pos.x <= GAMEBOARD.CONSTRAINTS["max"].x +0.5f) && (pos.y >= GAMEBOARD.CONSTRAINTS["min"].y -0.5f && pos.y <= GAMEBOARD.CONSTRAINTS["max"].y + 0.5f);
    }

    // Attache the active board piece to the mouse
    public bool AttachBPToMouse(GameObject BP) {
        if (MouseInPlayArea()) {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1f, LayerMask.GetMask("board-piece"));
            if (hit.collider != null) {
                return false;
            } else {
                SnapToGrid(BP);
                return true;
            }
        } else {
            return false;
        }
    }

    private void SnapToGrid(GameObject BP) {
        if(!WithinTolerance(mousePosition, gridPos)) {
            UpdateGridPosition();
            BP.transform.localPosition = gridPos;
            OnSnapToGrid?.Invoke(BP);
        }
    }

    bool WithinTolerance(Vector2 v1, Vector2 v2, float tolerance = 0.5f) {
        Vector2 dif = v1 - v2;
        if(Mathf.Abs(dif.x) <= tolerance && Mathf.Abs(dif.y) <= tolerance) return true;
        return false;
    }

    void UpdateGridPosition() {
        gridPos = new Vector2(Mathf.Round(mousePosition.x / 1f) * 1f, Mathf.Round(mousePosition.y / 1f) * 1f);
    }

    private void Update() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}