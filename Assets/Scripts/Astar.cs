using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Astar
{
   PriorityQueue heap;
      int [,]dist;
    int []dx={0,1,0,-1};
    int []dy={-1,0,1,0};
      string s;
     public int[,] findPath(int [][]maze,MyTuple start_positon,MyTuple finish_position,ref bool initFindPath,ref bool findPathtrue,ref bool startFindPath)
    {
        // MyTupleCompare compare=new MyTupleCompare();
         Debug.Log(start_positon.x+" "+start_positon.y);
         Debug.Log(finish_position.x+" "+finish_position.y);


        int row=maze.Length;
        int column=maze[0].Length;
        // Debug.Log("row: "+row+"   column: "+column);
        if(initFindPath==false)
        {
            initFindPath=true;
            heap=new PriorityQueue(300);
            dist=new int[row,column];
             heap.push(new MyTuple(0,start_positon.x*column+start_positon.y));
             for(int i=0;i<row;i++)
                for(int j=0;j<row;j++)dist[i,j]=0x3f3f3f3f;
            dist[start_positon.x,start_positon.y]=0;
        }
        
        
        

        while(heap.size>0)
        {
            // Debug.Log("heap size: "+heap.size);
            int temp=heap.pop().y;

            // Debug.Log(temp/column+"   "+(temp%column));
            MyTuple t=new MyTuple(temp/column,temp%column);
            // draw(t);
            if(t.x==finish_position.x&&t.y==finish_position.y)
            {
                findPathtrue=true;
                for(int i=0;i<row;i++)
                {
                    s="";
                    for(int j=0;j<column;j++)
                    {
                        s=s+dist[i,j].ToString()+" ";
                    }
                }
                startFindPath=false;
                return dist;
            }
            for(int i=0;i<4;i++)
            {
                if((maze[t.x][t.y]&(1<<i))>0)
                {
                    int nx=t.x+dx[i],ny=t.y+dy[i];
                    // Debug.Log(t.x+" "+t.y+" go: "+nx+" "+ny);
                    if(nx<0||nx>=row||ny<0||ny>=column)continue;
                    if(dist[nx,ny]==0x3f3f3f3f)
                    {
                        int k=dist[nx,ny]=dist[t.x,t.y]+1;
                        // Debug.Log(t.x+" "+t.y+" push: "+nx+" "+ny);
                        heap.push(new MyTuple(Math.Abs(nx-finish_position.x)+Math.Abs(ny-finish_position.y)+k,(nx)*column+ny));
                    }
                }
            }

        }
        
        return dist;
    }
    

    // public void draw(MyTuple t)
    // {
    //     for(int i=0;i<wallWidth;i++)
    //     {
    //         for(int j=0;j<wallWidth;j++)
    //         {
    //             Vector3 v3=new Vector3(t.x*(wallWidth+1)+i,0,t.y*(wallWidth+1)+j);
    //             if(checkIfTilePosEmpty(v3))
    //             Instantiate(Tile_current,v3,Quaternion.identity);
    //         }
    //     }
    // }
}