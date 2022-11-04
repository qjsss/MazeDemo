using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MyTuple{
        public int x,y;
        public MyTuple():this(0,0){}
        public MyTuple(int x,int y){
            this.x=x;
            this.y=y;
        }
    }
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