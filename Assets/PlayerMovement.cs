using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f;

    public GameObject WorldBorderCheckerLeft;
    public GameObject WorldBorderCheckerRight;
    public GameObject WorldBorderCheckerTop;
    public GameObject WorldBorderCheckerBottom;
    public float WorldBorderCheckDistance = 15f;
    
    private Vector3 _currentTransform;

    void Start() {
        WorldBorderCheckerLeft.transform.Translate(Vector3.left * WorldBorderCheckDistance);
        WorldBorderCheckerRight.transform.Translate(Vector3.right * WorldBorderCheckDistance);
        WorldBorderCheckerTop.transform.Translate(Vector3.up * WorldBorderCheckDistance);
        WorldBorderCheckerBottom.transform.Translate(Vector3.down * WorldBorderCheckDistance);
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        _currentTransform = transform.position;
        if (Input.GetKey(KeyCode.A)) 
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    } 
    
    
    private void CollisionCheck() {
      
    }
}