using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType { tree, tomato, wheat, stone, chest }

public class BoardPiece : MonoBehaviour {
    public PieceType type;
    public int scoreValue { get; private set; } = 5;
    public int pieceLevel { get; private set; } = 0;
}