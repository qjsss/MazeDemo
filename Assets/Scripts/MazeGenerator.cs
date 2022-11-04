using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MazeGenerator : MonoBehaviour
{
    public int row,column,wallWidth;
    public Transform walls,Tile_regular,Tile_current;
    // public class MyTuple{
    //     public int x,y;
    //     public MyTuple():this(0,0){}
    //     public MyTuple(int x,int y){
    //         this.x=x;
    //         this.y=y;
    //     }
    // }
    int []dx={0,1,0,-1};
    int []dy={-1,0,1,0};
    const int west=1<<0;
    const int south=1<<1;
    const int east=1<<2;
    const int north=1<<3;
    const int visited=1<<4;
    private int[][] maze;
    public int visitedCells;
    public Stack<MyTuple>stack=new Stack<MyTuple>();
    private List<Transform> previousCurrent = new List<Transform>();
    System.Random random=new System.Random();

    bool startCreate;
    public InputField RowInput;
    public InputField ColumnInput;
    public Text MessageText;
    public Toggle skipToggle;
    public Text PauseButtonText;
    private bool skip;

    public bool findPathtrue=false;
    Stack<MyTuple>path=new Stack<MyTuple>();
    bool finishWait=false;
    float passedTime=0f;
    float targetTime=2f;
    Astar astar=new Astar();
    //astar
    bool drawPath=false;
    public class PriorityQueue
    {
        int Compare(MyTuple x1,MyTuple x2)
        {
            return x1.x-x2.x;
        }
        public MyTuple[]heap;
        public int size{get;private set;}
        public PriorityQueue():this(200){}
        // public PriorityQueue(int capacity):this(capacity,null){}
        // public PriorityQueue(IComparer<T>comparer):this(200,null){}
        // public PriorityQueue(int capacity,IComparer<T>comparer)
        // {
        //     heap=new T[capacity+1];
        //     size=0;
        //     this.comparer=(comparer==null)?Comparer<T>.Default:comparer;
        // }
        public PriorityQueue(int capacity)
        {
            heap=new MyTuple[capacity+1];
            size=0;
        }
        public void push(MyTuple x)
        {
            if(size+1>heap.Length)Array.Resize(ref heap,heap.Length<<1+1);
            int i=++size;
            for(;i!=1&&Compare(x,heap[i>>1])<0;i>>=1)
            {
                heap[i]=heap[i>>1];
            }
            heap[i]=x;
        }
        public MyTuple pop()
        {
            if(size==0)throw new InvalidOperationException("优先队列为空");
            MyTuple res=heap[1];
            MyTuple t=heap[size--];
            int p=1;
            for(int s;p<<1<=size;p=s)
            {
                s=p<<1;
                if(s+1<=size&&Compare(heap[s],heap[s+1])>0)s++;
                if(Compare(t,heap[s])<0)break;
                heap[p]=heap[s];
            }
            heap[p]=t;
            return res;
        }   
        public MyTuple top()
        {
            return heap[1];
        }
    }
    // public class MyTupleCompare : IComparer<MyTuple>
    // {
    //     public int Compare(MyTuple x, MyTuple y)
    //     {
    //         // return new CaseInsensitiveComparer().Compare(x.x,x.x);
    //         return x.x-y.x;
    //     }
    // }
    
      PriorityQueue heap;
      int [,]dist;
      string s;
      int fx=-1,fy=-1;
      float time;
      public float interval;
    //  public void findPath(MyTuple start_positon,MyTuple finish_position)
    // {
    //     // MyTupleCompare compare=new MyTupleCompare();
         

        
    //     if(initFindPath==false)
    //     {
    //         initFindPath=true;
    //         heap=new PriorityQueue(300);
    //         dist=new int[row,column];
    //          heap.push(new MyTuple(0,start_positon.x*column+start_positon.y));
    //          for(int i=0;i<row;i++)
    //             for(int j=0;j<row;j++)dist[i,j]=0x3f3f3f3f;
    //         dist[start_positon.x,start_positon.y]=0;
    //     }
        
        
        

    //     while(heap.size>0)
    //     {
    //         Debug.Log("heap size: "+heap.size);
    //         int temp=heap.pop().y;

    //         Debug.Log(temp/column+"   "+(temp%column));
    //         MyTuple t=new MyTuple(temp/column,temp%column);
    //         // draw(t);
    //         if(t.x==finish_position.x&&t.y==finish_position.y)
    //         {
    //             Debug.Log(dist[finish_position.x,finish_position.y]);
    //             findPathtrue=true;
    //             for(int i=0;i<row;i++)
    //             {
    //                 s="";
    //                 for(int j=0;j<column;j++)
    //                 {
    //                     s=s+dist[i,j].ToString()+" ";
    //                 }
    //                 Debug.Log(s);
    //             }
    //              fx=finish_position.x;
    //              fy=finish_position.y;
    //             while(fx!=0||fy!=0)
    //             {
    //                 for(int i=0;i<4;i++)
    //                 {
    //                     if((maze[fx][fy]&(1<<i))>0)
    //                     {
    //                         int nx=fx+dx[i],ny=fy+dy[i];
    //                         if(dist[nx,ny]==dist[fx,fy]-1)
    //                         {
    //                             MyTuple tempt=new MyTuple(fx,fy);
    //                             StartCoroutine(waitForDraw(tempt));
    //                             // finishWait=false;
    //                             // draw();
    //                             fx=nx;
    //                             fy=ny;
    //                         }
    //                     }
    //                 }
    //             }
    //             if(fx==0&&fy==0)draw(new MyTuple(start_positon.x,start_positon.y));
    //             startFindPath=false;
    //             return ;
    //         }
    //         for(int i=0;i<4;i++)
    //         {
    //             if((maze[t.x][t.y]&(1<<i))>0)
    //             {
    //                 int nx=t.x+dx[i],ny=t.y+dy[i];
    //                 Debug.Log(t.x+" "+t.y+" go: "+nx+" "+ny);
    //                 if(nx<0||nx>=row||ny<0||ny>=column)continue;
    //                 if(dist[nx,ny]==0x3f3f3f3f)
    //                 {
    //                     int k=dist[nx,ny]=dist[t.x,t.y]+1;
    //                     Debug.Log(t.x+" "+t.y+" push: "+nx+" "+ny);
    //                     heap.push(new MyTuple(Math.Abs(nx-finish_position.x)+Math.Abs(ny-finish_position.y)+k,(nx)*column+ny));
    //                 }
    //             }
    //         }

    //     }
    // }
    public bool initFindPath=false;
    public bool startFindPath=false;

    public void OnFindPathButtonClick()
    {
        startFindPath=true;
    }
    public void OnPauseButtonClick()
    {   
        if(visitedCells>=row*column)
        {
            MessageText.text="已经生成完啦!";
            return ;
        }
        if(startCreate==true)
        {
            startCreate=false;
            PauseButtonText.text="Continue";
        }
        else 
        {
            startCreate=true;
            PauseButtonText.text="Pause";
        }
    }
    public void OnStartButtonClick()
    {   
        findPathtrue=false;
        initFindPath=false;
        startCreate=false;
        GameObject[] allTiles=GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject t in allTiles)
        {
            Destroy(t);
        }
        if(string.IsNullOrEmpty(RowInput.text)||string.IsNullOrEmpty(ColumnInput.text))
        {
            MessageText.text="输入错误";
            return ;
        }
        skip=skipToggle.isOn;
        MessageText.text="";
        row=int.Parse(RowInput.text);
        column=int.Parse(ColumnInput.text);
        startCreate=true;
        InitializeMazeStructure();
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        if(startCreate)
        {
            RB_Algorithm();
            if(!skip)
                DrawEverything();
            else if(visitedCells>=row*column)
            {
                DrawEverything();
            }
            if(visitedCells>=row*column)
            {
                
                // Astar astar=this.GetComponent<Astar>();
                // astar.findPath(maze,new Astar.MyTuple(0,0),new Astar.MyTuple(row,column));
                if(startFindPath&&findPathtrue==false)
                {
                    dist=astar.findPath(maze,new MyTuple(0,0),new MyTuple(row-1,column-1),ref initFindPath,ref findPathtrue,ref startFindPath);
                    drawPath=false;
                }
                else if(findPathtrue&&drawPath==false)
                {
                    if(fx==-1&&fy==-1)
                    {
                        fx=row-1;
                        fy=column-1;
                    }
                    Debug.LogError("aaaaaaaaaaaaaaaaa");
                    time+=Time.deltaTime;
                    while(fx!=0||fy!=0)
                    {
                        for(int i=0;i<4;i++)
                        {
                            if((maze[fx][fy]&(1<<i))>0)
                            {
                                int nx=fx+dx[i],ny=fy+dy[i];
                                if(dist[nx,ny]==dist[fx,fy]-1)
                                {
                                    MyTuple tempt=new MyTuple(fx,fy);
                                    Debug.LogError(fx+"  "+fy);
                                    if(time<=interval)break;
                                    time-=interval;
                                    draw(tempt);
                                    fx=nx;
                                    fy=ny;
                                }
                            }
                        }
                        if(time<interval)break;
                    }
                    if(fx==0&&fy==0&&time>=interval)
                    {
                        draw(new MyTuple(0,0));
                        drawPath=true;
                        time=0;
                    }
                }
            }
        }
    }
    //initialize the first maze
    // with row ,column 
    // Tiles are  wallWidth  times as big as wall 
    void InitializeMazeStructure()
    {
        maze=new int[row][];
        for(int i=0;i<row;i++)maze[i]=new int[column];
        int start_x=random.Next(row),start_y=random.Next(column);
        stack.Push(new MyTuple(start_x,start_y));
        maze[start_x][start_y]=visited;
        visitedCells=1;
        for(int x=0;x<row; x++)
        {
            for(int y = 0;y<column;y++)
            {
                for(int py=0; py < wallWidth+1; py++)
                {
                    for(int px=0;px<wallWidth+1;px++)
                    {
                        Vector3 v3=new Vector3(x*(wallWidth+1)+px,0,y*(wallWidth+1)+py);
                        if(px==wallWidth||py==wallWidth)
                            Instantiate(walls,v3,Quaternion.identity);
                        else
                            Instantiate(Tile_regular,v3,Quaternion.identity);
                    }
                }
                
            }
        }
        for(int x=0;x<column*(wallWidth+1);x++)
            Instantiate(walls,new Vector3(-1,0,x),Quaternion.identity);
        for(int x=-1;x<row*(wallWidth+1);x++)
            Instantiate(walls,new Vector3(x,0,-1),Quaternion.identity);
    }
    // maze gernerator
    void RB_Algorithm()
    {
        int []neighbours = new int[4];
        int idx=0;
        if (visitedCells < row * column)
        {
            int x=stack.Peek().x,y=stack.Peek().y;
            for(int i=0;i<4;i++)
            {
                if(x+dx[i]<0||x+dx[i]>=row)continue;
                if(y+dy[i]<0||y+dy[i]>=column)continue;
                if((maze[x+dx[i]][y+dy[i]]&visited)>0)continue;
                neighbours[idx++]=i;
            }
            if (idx > 0)
            {
                int next = neighbours[random.Next(idx)];
                maze[x+dx[next]][y+dy[next]]|=visited|(1<<((next+2)%4));
                maze[x][y]|=(1<<next);
                stack.Push(new MyTuple(x+dx[next],y+dy[next]));
                visitedCells++; 
            }
            else
                stack.Pop();
            // DrawEverything();
        }
    }
    //draw the process of build a maze
    void DrawEverything()
    {
        // Draw Maze
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                // destroy the wall
                for (int p = 0; p < wallWidth; p++)
                {
                    Vector3 v3 = new Vector3(x * (wallWidth + 1) + p, 0, y*(wallWidth+1)+wallWidth);
                    Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + p);
                    if (((maze[x][y] & east)>0) && checkIfTilePosEmpty(v3))
                        {
                            Instantiate(Tile_regular, v3, Quaternion.identity);
                        }
                    if (((maze[x][y] &south)>0) && checkIfTilePosEmpty(v3_2))
                        {
                            Instantiate(Tile_regular, v3_2, Quaternion.identity);
                        }
                }
            }
        }
        //destroy last moving
        foreach (Transform t in previousCurrent)
        {
            if(t != null) 
            {
                Instantiate(Tile_regular, t.position, Quaternion.identity);
                Destroy(t.gameObject);
            }
        }
        //generate the current position
        for (int py = 0; py < wallWidth; py++)
        {
            for (int px = 0; px < wallWidth; px++)
            {
                Vector3 v3 = new Vector3(stack.Peek().x * (wallWidth + 1) + px, 0, stack.Peek().y * (wallWidth + 1) + py);
                if(visitedCells<row*column)
                if(checkIfTilePosEmpty(v3))
                    previousCurrent.Add(Instantiate(Tile_current, v3, Quaternion.identity));                
            }
        }
    }
    //find gameobject with tag "Tile" and Destroy by Vector3 
    private bool checkIfTilePosEmpty(Vector3 targetPos)
    {
        GameObject[] allTilings = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in allTilings)
        {
            if (t.transform.position == targetPos)
            {
                Destroy(t);
            }
        }
        return true;
    }
    //draw a red Tile 
    public void draw(MyTuple t)
    {
        for(int i=0;i<wallWidth;i++)
        {
            for(int j=0;j<wallWidth;j++)
            {
                Vector3 v3=new Vector3(t.x*(wallWidth+1)+i,0,t.y*(wallWidth+1)+j);
                if(checkIfTilePosEmpty(v3))
                Instantiate(Tile_current,v3,Quaternion.identity);
            }
        }
    }
    // IEnumerator waitForDraw(MyTuple tuple)
    // {
    //     yield return new WaitForSeconds(2);
    //     draw(tuple);
    // }
}
