using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour {

    // The current position of the mouse on the game screen
    public Vector2 mousePosition;

    // Is the mouse over another board piece or not?
    public bool mouseOverBP = false;

    float minX = -0.5f;
    float minY = -2.5f;
    float maxX = 8.5f;
    float maxY = 4.5f;
    public bool MouseInPlayArea() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (mousePos.x >= minX && mousePos.x <= maxX) && (mousePos.y >= minY && mousePos.y <= maxY);
    }

    // Attache the active board piece to the mouse
    public void AttachBPToMouse(GameObject BP) {
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1f, LayerMask.GetMask("board-piece"));

        if(hit.collider != null && hit.transform.CompareTag("board-piece")) {
            mouseOverBP = true;
        } else if(hit.collider == null) {
            mouseOverBP = false;
            BP.transform.localScale = Vector3.one;
            Vector2 gridPos = new Vector2(Mathf.Round(mousePosition.x / 1f) * 1f, Mathf.Round(mousePosition.y / 1f) * 1f);
            BP.transform.localPosition = gridPos;
        }
    }

    private void Update() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}