/**********************************************************************

***********************************************************************/

using OKZKX.UnityExtension;
using UnityEngine;


namespace OKZKX.UnityTool
{
    public abstract class AutoSetBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            AutoSetTool.SetFields(this);
        }
    }
}