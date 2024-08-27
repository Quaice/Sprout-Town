using System.Collections.Generic;
using UnityEngine;

public class BPAnimator : MonoBehaviour {

    private bool pulseMatches = false;
    private List<GameObject> _matches = new List<GameObject>();
    private Vector2 _target;

    public float speed = 5f;        // The speed of the oscillation
    private float time = 0f;         // A time tracker to control the oscillation

    // Dictionary to store the original positions of the objects
    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();

    void Update() {
        if (pulseMatches) {
            PulseMatches();
        }
    }

    public void PulseMatches() {
        // Update the time tracker
        time += Time.deltaTime * speed;
        if (_matches.Count > 0) {
            foreach (GameObject bp in _matches) {
                // Calculate the halfway point between the original position and the target
                Vector2 halfwayPoint = Vector2.Lerp(originalPositions[bp], _target, 0.2f);

                // Calculate the new position using a sinusoidal function, oscillating around the halfway point
                Vector2 newPosition = Vector2.Lerp(originalPositions[bp], halfwayPoint, (Mathf.Sin(time) + 1f) / 2f);

                // Apply the new position to the transform
                bp.transform.position = newPosition;
            }
        }
    }

    public void AnimateMatches(List<GameObject> matches, Vector2 target) {
        _matches = matches;
        _target = target;
        pulseMatches = true;

        // Store the original positions of the objects
        originalPositions.Clear();
        foreach (GameObject bp in _matches) {
            originalPositions[bp] = bp.transform.position;
        }
    }

    public void StopAnimateMatches() {
        Debug.Log("Animation stopping...");
        pulseMatches = false;

        // Reset each object to its original position
        foreach (GameObject bp in _matches) {
            if (originalPositions.ContainsKey(bp)) {
                bp.transform.position = originalPositions[bp];
            }
        }
    }
}
