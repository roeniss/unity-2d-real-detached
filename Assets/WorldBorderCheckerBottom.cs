using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorderCheckerBottom : MonoBehaviour {
    public MainCameraScript mainCamera;

    void Start() {
        mainCamera = GameObject.Find("MainCamera").GetComponent<MainCameraScript>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Border") {
            Debug.Log("BottomBorder in");
            mainCamera.StopMoveDown();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Border") {
            Debug.Log("BottomBorder out");
            mainCamera.StartMoveDown();
        }
    }
}