using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DigMaze : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.Object wall;
    public int max_x;
    public int max_z;
    int x;
    int z;
    int[] start_pos = { 1, 1 };
    int[,] direction = { { 0, -2 }, { 0, 2 }, { -2, 0 }, { 2, 0 } };
    public int[,] field;

    void Start()
    {
        //土台の設計
        field = new int[max_x, max_z];

        for (x = 0; x < max_x; x++)
        {
            for (z = 0; z < max_z; z++)
            {
                field[x, z] = 1;
            }
        }

        //穴掘り法による迷路の設計
        Dig(start_pos[0], start_pos[1], field);

        field[1, 0] = 0; //スタート地点の壁を撤去する。
        field[max_z - 2, max_x - 1] = 0; //ゴール地点の壁を撤去する。

        //壁の設置
        SetWall(field);

            //Instantiate(wall, transform.position, transform.rotation);
    }

    //穴掘り法
    int[,] Dig(int x, int z, int[,] arr)
    {
        int[] rnd_arr = new int[] { 1, 2, 3, 4 };
        rnd_arr = rnd_arr.OrderBy(i => Guid.NewGuid()).ToArray();
        foreach (int index in rnd_arr)
        {
            int index2 = index - 1;
            if (z + direction[index2, 1] < 1 || z + direction[index2, 1] > max_z - 1)
            {
                continue;
            }
            else if (x + direction[index2, 0] < 1 || x + direction[index2, 0] > max_x - 1)
            {
                continue;
            }
            else if (arr[z + direction[index2, 1], x + direction[index2, 0]] == 0)
            {
                continue;
            }
            else
            {

            }

            arr[z + direction[index2, 1], x + direction[index2, 0]] = 0;
            if (index2 == 0)
            {
                arr[z + direction[index2, 1] + 1, x + direction[index2, 0]] = 0;
            }
            else if (index2 == 1)
            {
                arr[z + direction[index2, 1] - 1, x + direction[index2, 0]] = 0;
            }
            else if (index2 == 2)
            {
                arr[z + direction[index2, 1], x + direction[index2, 0] + 1] = 0;
            }
            else if (index2 == 3)
            {
                arr[z + direction[index2, 1], x + direction[index2, 0] - 1] = 0;
            }
            else
            {

            }

            //seep(0,2)
            //fn_print_maze(maze_array)
            Dig(x + direction[index2, 0], z + direction[index2, 1], arr);
        }
        return field;
    }

    //壁の設置
    void SetWall(int[,] arr)
    {
        for (x = 0; x < max_x; x++)
        {
            for (z = 0; z < max_z; z++)
            {
                if (arr[x, z] == 0)
                {

                }
                if (arr[x, z] == 1)
                {
                    Instantiate(wall, new Vector3(5.0f * x, 5.0f, 5.0f * z), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
