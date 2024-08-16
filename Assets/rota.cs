using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class rota : MonoBehaviour
{
    private static float previousTime;
    private static float fallTime = 0.6f;
    public static int height = 40; // 20 + 19 (height) = 39, array index from 0 to 39
    public static int width = 9; // 5 + 4 (width) = 9, array index from 0 to 9
    private static Transform[,] grid = new Transform[width, height];

    private const int zOffset = 4;
    private const int yOffset = 19;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(0, 0, -1);
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 0, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0, 0, 1);
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 0, -1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(90, 0, 0);
            if (!ValidMove())
            {
                int i = RotaMove();
                transform.Rotate(-90, 0, 0);
                transform.position += new Vector3(0, 0, i);
                transform.Rotate(90, 0, 0);
            }
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.Space) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 1, 0);
                AddToGrid();
                ClearFullRows();
                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().Newtetromiono();
            }
            previousTime = Time.time;
        }
    }

    void AddToGrid()
    {
        foreach (Transform t in transform)
        {
            int roundedZ = Mathf.RoundToInt(t.transform.position.z) + zOffset;
            int roundedY = Mathf.RoundToInt(t.transform.position.y) + yOffset;

            if (roundedZ >= 0 && roundedZ < width && roundedY >= 0 && roundedY < height)
            {
                grid[roundedZ, roundedY] = t;                
            }
            if (roundedY >= yOffset-1)
            {
                SpawnTetromino.Instance.Gameover();
            }
        }
    }


    bool IsLineFull(int y)
    {
        int arrayY = y + yOffset;

        if (arrayY < 0 || arrayY >= height)
        {
            return false;
        }

        for (int z = 0; z < width; z++)
        {
            if (grid[z, arrayY] == null)
            {
                return false;
            }
        }

        return true;
    }

    void ClearFullRows()
    {
        for (int y = height - yOffset - 1; y >= -yOffset; y--)
        {
            if (IsLineFull(y))
            {
                DestroyRow(y);
                DecreaseRow(y);
            }
        }
    }

    void DestroyRow(int y)
    {
        int arrayY = y + yOffset;

        if (arrayY < 0 || arrayY >= height)
        {
            Debug.LogError($"Index out of bounds when destroying row: y={y}, arrayY={arrayY}");
            return;
        }

        for (int z = 0; z < width; z++)
        {
            if (grid[z, arrayY] != null)
            {
                Destroy(grid[z, arrayY].gameObject);
                grid[z, arrayY] = null;
            }
        }
    }

    void DecreaseRow(int row)
    {
        for(int y = row; y < height-yOffset- 1; y++)
        {
            for(int z = 0; z < width; z++)
            {
                if (grid[z,y+yOffset+1] != null)
                {
                    grid[z, y + yOffset] = grid[z, y + yOffset + 1];
                    grid[z, y + yOffset + 1] = null;
                    grid[z, y + yOffset].gameObject.transform.position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    bool ValidMove()
    {
        foreach (Transform t in transform)
        {
            int roundedZ = Mathf.RoundToInt(t.transform.position.z) + zOffset;
            int roundedY = Mathf.RoundToInt(t.transform.position.y) + yOffset;

            if (roundedZ < 0 || roundedZ >= width || roundedY < 0 || roundedY >= height)
            {
                Debug.LogError($"Invalid move: roundedZ={roundedZ - zOffset}, roundedY={roundedY - yOffset}");
                return false;
            }

            
             if (grid[roundedZ, roundedY] != null)
            {
                Debug.LogError($"Position already occupied: roundedZ={roundedZ - zOffset}, roundedY={roundedY - yOffset}");
                return false;
            }
        }
        return true;
    }
    int RotaMove()
    {
        List<int> ints = new List<int>();
        int rotaint = 0;
        foreach (Transform t in transform)
        {
            int roundedZ = Mathf.RoundToInt(t.transform.position.z) + zOffset;
            ints.Add(roundedZ);            
        }
        if (ints.Min() < 0)
        {
            rotaint = 0 - ints.Min();
        }
        else if (ints.Max() >= width)
        {
            rotaint = width - ints.Max()-1;
        }
        return rotaint;
    }
}