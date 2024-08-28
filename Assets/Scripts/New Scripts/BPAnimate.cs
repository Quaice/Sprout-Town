using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPAnimate : MonoBehaviour {

    public void AnimateMatches(List<GameObject> matches) {
        foreach (GameObject match in matches) {
            match.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}