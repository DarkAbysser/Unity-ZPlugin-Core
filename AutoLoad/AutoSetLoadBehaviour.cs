/**********************************************************************

***********************************************************************/

using OKZKX.UnityExtension;
using UnityEngine;


namespace OKZKX.UnityTool
{
    public abstract class AutoSetLoadBehaviour : AutoSetBehaviour
    {
        protected override void Awake()
        {
            this.SetFields();
            this.LoadFields();
        }
    }
}