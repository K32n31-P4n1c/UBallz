using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{   
    [SerializeField]
    private GameObject ballPrefab;

    private LaunchPreview launchPreview;

    private Vector3 StartDragPosition;
    private Vector3 endDragPosition;

    private void Awake()
    {
        launchPreview = GetComponent<LaunchPreview>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Give us the exact point that we clicked

        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(worldPosition);
        }
        else if (Input.GetMouseButton(0)) // if player hold the button
        {
            ContinueDrag(worldPosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void EndDrag()
    {
        Vector3 direction = endDragPosition - StartDragPosition;
        direction.Normalize();

        var ball =  Instantiate(ballPrefab, transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody2D>().AddForce(-direction);
    }

    private void StartDrag(Vector3 worldPosition)
    {
        StartDragPosition = worldPosition;
        launchPreview.SetStartPoint(transform.position);
    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        endDragPosition = worldPosition;

        Vector3 direction = endDragPosition - StartDragPosition;

        launchPreview.SetEndPoint(transform.position - direction);
    } 
}
