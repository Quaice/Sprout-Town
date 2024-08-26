using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public ItemFeeder itemFeeder;

    public GameObject activeItem {  get; private set; }
    public int _score { get; private set; }
    public float gridSize { get; private set; } = 1.0f;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (itemFeeder == null) { itemFeeder = ItemFeeder.Instance; }
        activeItem = itemFeeder.GetNewBoardPiece();
    }

    public GameObject PlaceAndGetNewBoardPiece() {
        activeItem.layer = LayerMask.NameToLayer("board-piece");
        activeItem.tag = "board-piece";
        activeItem.GetComponent<SpriteRenderer>().sortingOrder = 0;
        AddToScore(activeItem.GetComponent<BoardPiece>()._scoreValue);
        activeItem = itemFeeder.GetNewBoardPiece();
        return activeItem;
    }

    public void AddToScore(int value) {
        _score += value;
    }
}