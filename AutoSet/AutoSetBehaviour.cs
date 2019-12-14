/**********************************************************************

***********************************************************************/

using OKZKX.UnityExtension;
using UnityEngine;


namespace OKZKX.UnityTool
{
    public abstract class AutoSetBehaviour : MonoBehaviour
    {
        public virtual void Awake()
        {
            AutoSetTool.SetFileds(this);
        }
    }
}