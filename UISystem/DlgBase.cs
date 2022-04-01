/**********************************************************************
* UI 会话基类
* 
* 在这个UI管理系统下
* 所有的UI面板都要挂载这个类
***********************************************************************/
using ZPlugin;
using UnityEngine;

public abstract class DlgBase : AutoSetBehaviour
{
    public virtual void OnCreate() { }
    public virtual void OnDestroy() { }
}

