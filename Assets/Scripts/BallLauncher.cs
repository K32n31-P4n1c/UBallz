using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{   
    [SerializeField]
    private Ball ballPrefab;

    private List<Ball> balls = new List<Ball>();

    private LaunchPreview launchPreview;
    private BlockSpawner blockSpawner;

    private Vector3 StartDragPosition;
    private Vector3 endDragPosition;
    private int ballsLoad;

    private void Awake()
    {
        launchPreview = GetComponent<LaunchPreview>();
        blockSpawner = FindObjectOfType<BlockSpawner>();
        CreateBall();
    }

    public void ReturnBall()
    {
        ballsLoad++;

        if (ballsLoad == balls.Count)
        {
            blockSpawner.SpawnRowOfBlocks();
            CreateBall();
        }
    }

    private void CreateBall()
    {
        var ball = Instantiate(ballPrefab);
        balls.Add(ball);
        ballsLoad++;
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
        StartCoroutine(LaunchBalls());
    }

    private IEnumerator LaunchBalls()
    {
        Vector3 direction = endDragPosition - StartDragPosition;    // Gets direction once, only for first ball and apply to all others
        direction.Normalize();

        foreach(var ball in balls)
        {
            ball.transform.position = transform.position;   //  Gets the position of the BallLauncher;
            ball.gameObject.SetActive(true);
            ball.GetComponent<Rigidbody2D>().AddForce(-direction);

            yield return new WaitForSeconds(0.1f);
        }

        ballsLoad  = 0;

    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        endDragPosition = worldPosition;

        Vector3 direction = endDragPosition - StartDragPosition;

        launchPreview.SetEndPoint(transform.position - direction);
    }

    private void StartDrag(Vector3 worldPosition)
    {
        StartDragPosition = worldPosition;
        launchPreview.SetStartPoint(transform.position);
    }

}