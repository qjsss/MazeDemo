using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MazeGenerator : MonoBehaviour
{
    #region Properties
    public int row,column,wallWidth;
    public Transform walls,Tile_regular,Tile_current;
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
      PriorityQueue heap;
      int [,]dist;
      string s;
      int fx=-1,fy=-1;
      float time;
      public float interval;
    
    public bool initFindPath=false;
    public bool startFindPath=false;

    public bool drawMazeFinished;
    #endregion Properties

    #region ButtonEvent
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
        fx=-1;
        fy=-1;
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
    #endregion ButtonEvent
    
    void Update()
    {
        if(startCreate)
        {
            RB_Algorithm();
            if(!skip)
            {
                if(drawMazeFinished==false)
                DrawEverything();
            }
            else if(visitedCells>=row*column)
            {
                if(drawMazeFinished==false)
                DrawEverything();
            }
            if(visitedCells>=row*column)
            {
                drawMazeFinished=true;
                
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
                                    // Debug.LogError(fx+"  "+fy);
                                    if(time<=interval)break;
                                    time-=interval;
                                    draw(tempt);
                                    drawBetweenTwo(tempt,new MyTuple(nx,ny));
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
    #region Draw
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
    public void drawBetweenTwo(MyTuple x,MyTuple y)
    {
        Debug.LogWarning(x.x+" "+x.y+"  ->  "+y.x+" "+y.y);
        if(x.x+dx[1]==y.x&&x.y+dy[1]==y.y)drawBottom(x);
        else if(x.x+dx[2]==y.x&&x.y+dy[2]==y.y)drawRight(x);
        else if(x.x+dx[3]==y.x&&x.y+dy[3]==y.y)drawBottom(y);
        else if(x.x+dx[0]==y.x&&x.y+dy[0]==y.y)drawRight(y);
    }
    public void drawBottom(MyTuple x)
    {
        Debug.LogWarning("bottom: "+x.x+" "+x.y);

        for(int i=0;i<wallWidth;i++)
        {
            Vector3 v3=new Vector3((x.x+1)*(wallWidth+1)-1,0,x.y*(wallWidth+1)+i);
            Debug.LogError(v3.x+" "+v3.z);
            if(checkIfTilePosEmpty(v3))
                Instantiate(Tile_current,v3,Quaternion.identity);
        }
    }
    public void drawRight(MyTuple x)
    {
        Debug.LogWarning("right: "+x.x+" "+x.y);
        for(int i=0;i<wallWidth;i++)
        {
            Vector3 v3=new Vector3(x.x*(wallWidth+1)+i,0,(x.y+1)*(wallWidth+1)-1);
            Debug.LogError(v3.x+" "+v3.z);
            if(checkIfTilePosEmpty(v3))
                Instantiate(Tile_current,v3,Quaternion.identity);
        }
    }
    #endregion Draw

}
