/*----------------------------------------------------------------------------*
 * Code written by Quaice (https://github.com/Quaice)
 * "BP" (read BoardPiece) always refers to a BoardPiece GameObject
 *----------------------------------------------------------------------------*/
using System.Collections.Generic;
using UnityEngine;

public static class GAMEBOARD {
    public static Dictionary<string, Vector2> CONSTRAINTS = new Dictionary<string, Vector2>()
    {
        { "min", new Vector2(0, 0) },
        { "max", new Vector2(8, 6) }
    };

    public static Dictionary<string, Vector2> DIRECTION = new Dictionary<string, Vector2>()
    {
        { "UP", Vector2.up },
        { "DOWN", Vector2.down },
        { "LEFT", Vector2.left },
        { "RIGHT", Vector2.right }
    };
}

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    private BPManager _bpManager;
    private BPAnimator _bpAnimator;
    private InputManager _inputManager;

    public GameObject activeBoardPiece {  get; private set; }
    public int _score { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (_bpManager == null) { _bpManager = GetComponent<BPManager>(); }
        if (_bpAnimator == null) {  _bpAnimator = GetComponent<BPAnimator>(); }
        if (_inputManager == null) { _inputManager = GetComponent<InputManager>();}
        activeBoardPiece = _bpManager.GetNewBoardPiece(_inputManager.mousePosition);

        _inputManager.OnSnapToGrid += BPSnappedToGrid;                                      // Subscribe to the OnSnapToGrid event
    }

    private void BPSnappedToGrid(GameObject BP) {
        _bpAnimator.StopAnimateMatches();
        _bpManager.OnBoardPieceSnap(BP);                                                    // Notify BPManager that a piece has snapped to the grid
    }

    public GameObject PlaceAndGetNewBP() {
        _bpManager.PlaceBPOnBoard(activeBoardPiece);                                        // Put the piece on the board
        AddToScore(activeBoardPiece.GetComponent<BoardPiece>().scoreValue);                 // Add the active item's score value to the total score
        return activeBoardPiece = _bpManager.GetNewBoardPiece(_inputManager.mousePosition); // Get a new active item and return it
    }

    public void AddToScore(int value) {
        _score += value;
    }

    private void Update() {
        if (_inputManager.AttachBPToMouse(activeBoardPiece)) {
            activeBoardPiece.SetActive(true);                                               // Make sure the currently held board piece is active on the board
            if (Input.GetMouseButtonDown(0)) {
                activeBoardPiece = PlaceAndGetNewBP();
            }
        } else {
            activeBoardPiece.SetActive(false);                                              // The mouse is outside the play area, deactivate the held board piece
        }
    }

    public bool IsInPlayArea(Vector2 pos) {
        return (pos.x >= GAMEBOARD.CONSTRAINTS["min"].x && pos.x <= GAMEBOARD.CONSTRAINTS["max"].x) && (pos.y >= GAMEBOARD.CONSTRAINTS["min"].y && pos.y <= GAMEBOARD.CONSTRAINTS["max"].y);
    }
}