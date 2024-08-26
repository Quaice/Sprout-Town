using UnityEditor;
using UnityEngine;

// Feeds items to the player one at a time
// Shows the next item in the queue to be fed to the player

public class ItemFeeder : MonoBehaviour {

    public static ItemFeeder Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public GameObject[] m_objectPool;

    public GameObject GetNewBoardPiece() {
        return Instantiate(m_objectPool[Random.Range(0, m_objectPool.Length)], new Vector3(-6.5f, 0, 0), Quaternion.identity);
    }

}