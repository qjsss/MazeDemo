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
        int row=maze.Length;
        int column=maze[0].Length;
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
            int temp=heap.pop().y;
            MyTuple t=new MyTuple(temp/column,temp%column);
            if(t.x==finish_position.x&&t.y==finish_position.y)
            {
                findPathtrue=true;
                startFindPath=false;
                return dist;
            }
            for(int i=0;i<4;i++)
            {
                if((maze[t.x][t.y]&(1<<i))>0)
                {
                    int nx=t.x+dx[i],ny=t.y+dy[i];
                    if(nx<0||nx>=row||ny<0||ny>=column)continue;
                    if(dist[nx,ny]==0x3f3f3f3f)
                    {
                        int k=dist[nx,ny]=dist[t.x,t.y]+1;
                        heap.push(new MyTuple(Math.Abs(nx-finish_position.x)+Math.Abs(ny-finish_position.y)+k,(nx)*column+ny));
                    }
                }
            }

        }
        
        return dist;
    }
    
}   
/*
inf    2    1    inf    inf
inf    1    0    1    inf
3      2    3    2    inf

inf    3    2    inf    inf
inf    2    1    2    inf
2      1    0    1    inf

inf    5    3    inf    inf
inf    3    1    3    inf
5      3    3    3    inf
*/
