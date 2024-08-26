using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType { tree, tomato, wheat, stone, chest }

public class BoardPiece : MonoBehaviour {
    public PieceType _type;
    public int _scoreValue = 5;
    public int _pieceLevel = 0;
}