using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public interface IGridShape
    {
        void GetEdges(List<Vector3> ret);
    }
}