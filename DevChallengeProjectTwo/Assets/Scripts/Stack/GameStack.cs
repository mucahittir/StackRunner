using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStack : PoolObject
{
    private bool dirRight = true;
    [SerializeField] private float speed;
    [SerializeField] AudioSource audioSource;
    public void Move()
    {
        if (dirRight)
            transform.position += Vector3.right * speed * Time.deltaTime;
        else
            transform.position += -Vector3.right * speed * Time.deltaTime;


        if (transform.position.x >= 2.0f)
        {
            dirRight = false;
        }

        if (transform.position.x <= -2f)
        {
            dirRight = true;
        }
    }

    public bool SplitThis(GameStack beforeStack, float tolerationOffset)
    {
        float hangover = transform.position.x - beforeStack.transform.position.x;
        float isRight = (hangover > 0) ? 1f : -1f;

        if (Mathf.Abs(hangover) < tolerationOffset)
        {
            hangover = 0;
            audioSource.Play();
        }

        float newXSize = beforeStack.transform.localScale.x - Mathf.Abs(hangover);
        if(newXSize <= 0)
        {
            return false;
        }
        float newXPosition = beforeStack.transform.position.x + (hangover/2f);

        
        float fallingStackSize = transform.localScale.x - newXSize;
        float cubeEdge = transform.position.x + (newXSize/2f * isRight);
        float fallingXPosition = cubeEdge + (fallingStackSize / 2f * isRight);


        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        if(hangover != 0)
        spawnFallingCube(cubeEdge, fallingStackSize);

        return true;
    }

    private void spawnFallingCube(float xPos, float xSize)
    {
        Vector3 scale = new Vector3(xSize, transform.localScale.y, transform.localScale.z);
        Vector3 position = new Vector3(xPos, transform.position.y, transform.position.z);

        PoolObject fallingCube = PoolManager.Instance.GetItem("FallingCube");
        fallingCube.SetActiveWithTransform(position,Quaternion.identity, scale);
    }
}
