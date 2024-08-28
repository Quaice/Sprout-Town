/* 
 * MAIN CONTROLLERS
 * 
 * Most things should be handled through this controller and it's Update method
 * 
 */

using System.Collections.Generic;
using UnityEngine;

namespace SproutTown {

    public class GameController : MonoBehaviour {

        // Singleton
        public static GameController Instance;

        // Controllers
        private InputController InputController;
        private BPController BPController;
        private BPAnimate BPAnimate;

        private GameObject _activeBP;   // The currently active board piece in the player's hand
        private Vector2 _spawnPoint = new Vector2(50, 50);

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            InputController = GetComponent<InputController>();
            BPController = GetComponent<BPController>();
            BPAnimate = GetComponent<BPAnimate>();

            InputController.OnSnapToGrid += BPSnappedToGrid;    // Subscribe to the OnSnapToGrid event
            _activeBP = BPController.GetNewBP();                // Get the first board piece
        }

        private void Update() {
            InputController.UpdateInputController(_activeBP);

            if (Input.GetMouseButtonDown(0)
                && InputController.MouseInPlayArea()
                && !InputController.MouseIsOverBP())
            {
                BPController.PlaceBPOnBoard(_activeBP);
                _activeBP = BPController.GetNewBP();
            }
        }

        private void BPSnappedToGrid(GameObject _activeBP) {
            BPAnimate.AnimateMatches(BPController.CheckForMatches(_activeBP));
        }
    }

    public static class GAMEBOARD {
        public static float GRIDSIZE = 1.0f;
        public static Dictionary<string, Vector2> CONSTRAINTS = new Dictionary<string, Vector2>()
        {
            { "min", new Vector2(0, 0) },
            { "max", new Vector2(8, 6) }
        };
    }
}