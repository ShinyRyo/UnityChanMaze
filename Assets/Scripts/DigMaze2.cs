using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DigMaze2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject goal;
    Transform player_pos;
    Transform goal_pos;
    public int posX;
    public int posZ;
    public UnityEngine.Object wall;
    public UnityEngine.Object ball;
    public int max_x;
    public int max_z;
    int x;
    int z;
    int[] start_pos = { 1, 1 };
    int[,] direction = { { 0, -2 }, { 0, 2 }, { -2, 0 }, { 2, 0 } };
    public int[,] field;

    void Start()
    {
        transform.position = player.transform.position;
        player = GameObject.Find("unitychan");
        player_pos = player.GetComponent<Transform>();

        transform.position = player.transform.position;
        goal = GameObject.Find("Goal");
        goal_pos = goal.GetComponent<Transform>();
        //土台の設計
        field = make_field(max_x, max_z, 1);

        //穴掘り法による迷路の設計
        Dig(start_pos[0], start_pos[1], field);

        field[1, 0] = 0; //スタート地点の壁を撤去する。
        field[max_x - 2, max_z - 1] = 0; //ゴール地点の壁を撤去する。

        //壁の設置
        SetWall(field);

            //Instantiate(wall, transform.position, transform.rotation);
    }
    void Update()
    {
        posX = (int)player_pos.position.x;
        posZ = (int)player_pos.position.z;
        //あるボタンを押したらその場所からの最短距離を導いて道筋を表示
        if (Input.GetButtonUp("Jump"))
        {
            //DijkstraMethod(max_x, max_z, player_pos, goal_pos, field);
        }
    }

    void DijkstraMethod(int max_x, int max_z, Transform player_pos, Transform goal_pos, int[,] map)
    {
        //経路検索に必要な条件
        int[] start_coords = { (int)player_pos.position.x, (int)player_pos.position.y, (int)player_pos.position.z };
        int[] dest_coords = { (int)goal_pos.position.x, (int)goal_pos.position.y, (int)goal_pos.position.z };
        map[start_coords[0], start_coords[2]] = 5;
        map[dest_coords[0], dest_coords[2]] = 6;
        //# 行数、列数
        int nrows = max_z;
        int ncols = max_x;
        //# 線形インデックス（マップ座標のセルをインデックスに変換）
        int start_node = ncols * start_coords[0] + start_coords[2]; //start_node = ncols * (start_coords[0]) + start_coords[1]
        int dest_node = ncols * dest_coords[0] + dest_coords[2]; //dest_node = ncols * (dest_coords[0]) + dest_coords[1]      
        int[,] distanceFromStart = make_field(max_x, max_z,100); 
        int min_dist;
        int argmin=0;
        int current;
        int mi;
        int mj;
        int neighbor_node_r=0;
        int neighbor_node_c=0;
        int[,] parent = make_field(max_x, max_z,0);
        List<int> route = new List<int> { (dest_coords[0] * ncols) + dest_coords[2]};
        //# スタート地点は距離0
        distanceFromStart[start_coords[0], start_coords[2]] = 0;
        //探索開始
        while (true)//while True:
        {
            //# スタート座標から最短距離のセルを探す（最初はスタート座標）
            //# 線形インデックスが得られる
            min_dist = distanceFromStart.Cast<int>().Min(); //#最小値を求める #スタート地点の0->1->2
            current = Argmin(distanceFromStart, min_dist, argmin); //#最小値のindexを求める #配列を1次元化した時の要素数を返す #nrows=10
                                                                   //# ゴールに達したら抜ける
            Debug.Log("debug comment2");
            //パスが100を超えたら強制終了
            if (current == dest_node || min_dist == 100)//if current == dest_node or min_dist == 100:
            {
                break; //break
            }
            //# 通常の座標に変換
            mi = current / ncols; //i = int(current // ncols)
            mj = current % ncols;//j = int(current % ncols) #int(current - (current // ncols) * ncols)
            //# 探索済とする
            map[mi, mj] = 3;//map[i, j] = 3
            //# 次回の探索から外す
            distanceFromStart[mi, mj] = 100;//distanceFromStart[i][j] = 100

            //# カレントセルの上下左右を調べる
            for (int a = 0; a < 4; a++)//for ii in range(1, 5):
            {
                Debug.Log("debug comment2.1");
                if (a == 0)//if ii == 1:
                {
                    if (mi > 0)//if i > 0:
                    {
                        Debug.Log("debug comment2.1.1");
                        neighbor_node_r = mi - 1;//neighbor_node_r = i - 1
                        neighbor_node_c = mj; //neighbor_node_c = j
                    }
                }
                else if (a == 1)//elif ii == 2:
                {
                    if (mi < nrows - 1)//f ii < nrows - 1:
                    {
                        Debug.Log("debug comment2.1.2");
                        neighbor_node_r = mi + 1;//neighbor_node_r = i + 1
                        neighbor_node_c = mj;//neighbor_node_c = j
                    }
                }
                else if (a == 2)//elif ii == 3:
                {
                    if (mj > 0)//if j > 0:
                    {
                        neighbor_node_r = mi;//neighbor_node_r = i
                        neighbor_node_c = mj - 1;//neighbor_node_c = j - 1
                    }
                }
                else if (a == 3)//elif ii == 4:
                {
                    if (mj < ncols - 1)//if j < ncols - 1:
                    {
                        neighbor_node_r = mi;//neighbor_node_r = i
                        neighbor_node_c = mj + 1;//neighbor_node_c = j + 1
                    }
                }
                //# 探索済/障害物/ゴールかどうか
                if (map[neighbor_node_r, neighbor_node_c] != 3 &//if map[neighbor_node_r, neighbor_node_c] != 3 and\
                    map[neighbor_node_r, neighbor_node_c] != 2 &//map[neighbor_node_r, neighbor_node_c] != 2 and\
                        map[neighbor_node_r, neighbor_node_c] != 5)//map[neighbor_node_r, neighbor_node_c] != 5:
                {//# 距離をプラス１
                    int dist = min_dist + 1;//dist = min_dist + 1
                    //# 最短なら更新
                    if (dist < distanceFromStart[neighbor_node_r, neighbor_node_c])//if dist < distanceFromStart[neighbor_node_r, neighbor_node_c]:
                    {
                        distanceFromStart[neighbor_node_r, neighbor_node_c] = dist;
                        parent[neighbor_node_r, neighbor_node_c] = current;
                        map[neighbor_node_r, neighbor_node_c] = 4;
                     }
                 }
            }
        }
        Debug.Log("debug comment3");
        //# 最短経路を表示する
        //# 普通の座標に変換
        int route_r = route[0] / ncols;//route_r = int(route[0] // ncols)
        int route_c = route[0] % ncols;//route_c = int(route[0] % ncols) #int(route[0] - (route[0] // ncols) * ncols)
        //# スタート地点まで
        int c=1;
        while(parent[route_r, route_c] != 0)
        {
            //# 親座標まで１コマ戻る
            route.Add(parent[route_r, route_c]);//route = np.append(parent[route_r,route_c],route)
            //# 0で表示
            map[route_r, route_c] = 2;//map[route_r, route_c] = 0
            map[dest_coords[0], dest_coords[2]] = 6;//map[dest_coords[0], dest_coords[1]] = 6
            //# 座標を更新
            route_r = (route[c] / ncols);//route_r = int(route[0] // ncols)
            route_c = (route[c] % ncols);//route_c = int(route[0] % ncols)//int(route[0] - (route[0] // ncols) * ncols)
            c += 1;
        }
        //map[0,0]=0; //地図空間が表示されるかの確認OK＝＞地図空間が適切に更新されていない
        //ゴールまでの軌跡
        SetWall(map);
    }

    int[,] make_field(int max_x, int max_z, int num)
    {
        int[,] field = new int[max_x,max_z];
        for (x = 0; x < max_x; x++)
        {
            for (z = 0; z < max_z; z++)
            {
                field[x, z] = num;
            }
        }
        return field;
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
                if (arr[x, z] == 2)
                {
                    Instantiate(ball, new Vector3(5.0f * x, 3.0f, 5.0f * z), Quaternion.identity);
                }
            }
        }
    }
    int Argmin(int[,] arr, int min, int argmin)
    {
        int[] array1dim = arr.Cast<int>().ToArray();
        for(int i = 0; i <array1dim.Length ; ++i) 
        {
            if( array1dim[i] == min )
            {
                argmin = i;
            }
        }
        return argmin;
    }
    
}
