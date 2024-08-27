using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    private BPManager _bpManager;
    private InputManager _inputManager;

    public GameObject activeItem {  get; private set; }
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
        if (_inputManager == null) { _inputManager = GetComponent<InputManager>();}

        activeItem = _bpManager.GetNewBoardPiece(_inputManager.mousePosition);
    }

    public GameObject PlaceAndGetNewBP() {
        // Put the piece on the board
        _bpManager.PlaceBPOnBoard(activeItem);

        // Add the active item's score value to the total score
        AddToScore(activeItem.GetComponent<BoardPiece>()._scoreValue);

        // Get a new active item and return it
        activeItem = _bpManager.GetNewBoardPiece(_inputManager.mousePosition);
        return activeItem;
    }

    public void AddToScore(int value) {
        _score += value;
    }

    private void Update() {
        // If the mouse is inside the play area an is not over top another board piece...
        if (_inputManager.MouseInPlayArea() || !_inputManager.mouseOverBP) { activeItem.SetActive(true); }

        // If the mouse is out of the play area or is over top another board piece...
        if (!_inputManager.MouseInPlayArea() || _inputManager.mouseOverBP) { activeItem.SetActive(false); }

        // If the mouse is in the play area, attach the active item to it
        if (_inputManager.MouseInPlayArea()) {
            _inputManager.AttachBPToMouse(activeItem);

            // If the user clicks the mouse button, place the active item and get a new one
            if (Input.GetMouseButtonDown(0)) {
                activeItem = PlaceAndGetNewBP();
            }
        }
    }
}