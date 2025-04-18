using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Text类型内存池
/// </summary>
public class TestMemoryPool : BaseMemoryPool
{
    public TestMemoryPool(string memoryName, MemoryPoolType poolType) : base(memoryName, poolType)
    { 
    
    }
    public override void Init()
    {
        base.Init();
    }
}
