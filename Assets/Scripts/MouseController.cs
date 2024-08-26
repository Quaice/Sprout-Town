using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MouseController : MonoBehaviour {

    Camera _camera;
    GameManager gameManager;
    bool _mouseOverBoardPiece;

    private GameObject _activeItem;

    void Start() {
        gameManager = GameManager.Instance;
        _camera = Camera.main;
        _mouseOverBoardPiece = false;
        _activeItem = gameManager.activeItem;
        _activeItem.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    void Update() {
        if(MouseInPlayArea()) {
            _activeItem.SetActive(true);
            UpdatePosition();

            // If the player clicks the mouse and the mouse isn't over another board piece...
            if (Input.GetMouseButtonDown(0) && !_mouseOverBoardPiece) {
                _activeItem = gameManager.PlaceAndGetNewBoardPiece();
            }
        } else {
            _activeItem.SetActive(false);
        }
    }

    // Mouse playarea boundry check
    float minX = -0.5f;
    float minY = -2.5f;
    float maxX = 8.5f;
    float maxY = 4.5f;
    public bool MouseInPlayArea() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (mousePos.x >= minX && mousePos.x <= maxX) && (mousePos.y >= minY && mousePos.y <= maxY);
    }

    void UpdatePosition() {
        Vector3 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.zero, 1f, LayerMask.GetMask("board-piece"));

        if (hit.collider != null) {
            if(hit.transform.CompareTag("board-piece")) {
                _mouseOverBoardPiece = true;
                _activeItem.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                _activeItem.transform.position = worldPos;
            }
        } else {
            _activeItem.transform.localScale = Vector3.one;
            _mouseOverBoardPiece = false;
            Vector2 gridPos = new Vector2(
                Mathf.Round(worldPos.x / gameManager.gridSize) * gameManager.gridSize,
                Mathf.Round(worldPos.y / gameManager.gridSize) * gameManager.gridSize
             );
            _activeItem.transform.position = gridPos;
        }
    }
}