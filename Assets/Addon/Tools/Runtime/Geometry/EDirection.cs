using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyTool
{
    public enum EDirection
    {
        Up,
        LeftUp,
        Left,
        LeftDown,
        Down,
        RightDown,
        Right,
        RightUp,
        None,
    }

    public static class EDirectionTool
    {
        private static readonly Vector2Int[] Vectors = new Vector2Int[]
        {
        Vector2Int.up,
        Vector2Int.left + Vector2Int.up,
        Vector2Int.left,
        Vector2Int.left + Vector2Int.down,
        Vector2Int.down,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.right,
        Vector2Int.right + Vector2Int.up,
        Vector2Int.zero,
        };

        private static readonly Dictionary<EDirection, Vector2Int> Dict_Direction = new();
        private static readonly Dictionary<Vector2Int, EDirection> Dict_EDirection = new();

        static EDirectionTool()
        {
            for (int i = 0; i < Vectors.Length; i++)
            {
                Dict_Direction.Add((EDirection)i, Vectors[i]);
                Dict_EDirection.Add(Vectors[i], (EDirection)i);
            }
        }

        public static EDirection ToEDirection(this Vector2Int direction)
        {
            if (Dict_EDirection.ContainsKey(direction))
                return Dict_EDirection[direction];
            Debug.LogWarning($"输入的方向为{direction}，不是八种方向之一");
            return EDirection.None;
        }

        public static Vector2Int ToVector(this EDirection eDirection)
            => Dict_Direction[eDirection];

        public static float ToAngle(this EDirection eDirection)
            => (int)eDirection * 45f;

        public static bool IsOblique(this EDirection eDirection)
        {
            return eDirection switch
            {
                EDirection.Up or EDirection.Left or EDirection.Down or EDirection.Right or EDirection.None => false,
                _ => true,
            };
        }

        /// <summary>
        /// 按逆时针顺序返回八个方向
        /// </summary>
        public static List<Vector2Int> GetDirections()
        {
            List<Vector2Int> ret = new(8);
            for (int i = 0; i < 8; i++)
            {
                ret.Add(((EDirection)i).ToVector());
            }
            return ret;
        }

        /// <summary>
        /// 按顺序返回八个方向，与direction更接近的在前
        /// </summary>
        /// <param name="n">结果保留前n个值</param>
        public static List<Vector2Int> NearerDirections(Vector2 direction, int n = 8)
        {
            n = Mathf.Clamp(n, 0, 8);
            List<Vector2Int> ret = GetDirections();
            GeometryTool.Comparer_Vector2_Nearer comparer = new(direction);
            ret.Sort(comparer);
            return ret.GetRange(0, n);
        }

        /// <summary>
        /// 返回八个方向中与所给方向最接近的
        /// </summary>
        public static Vector2Int NearestDirection(Vector2 direciton)
        {
            int sign = Mathf.RoundToInt(direciton.ToAngle() / 45f) % 8;
            return Vectors[sign];
        }
        /// <summary>
        /// 返回四个方向中与所给方向最接近的
        /// </summary>
        public static Vector2Int NearestDirection4(Vector2 direciton)
        {
            int sign = Mathf.RoundToInt(direciton.ToAngle() / 90f) % 4 * 2;
            return Vectors[sign];
        }

        /// <summary>
        /// 将位移分解成八向移动的组合，尽量使结果接近直线
        /// </summary>
        /// <returns>路径上的每一点（包括起点）</returns>
        public static List<Vector2Int> DivideDisplacement(Vector2Int origin, Vector2Int target, int maxCount = 20)
        {
            Vector2Int offset = target - origin;
            int sign = (int)(((Vector2)offset).ToAngle() / 45f);
            Vector2Int base1, base2;
            int c1, c2;
            switch (sign)
            {
                case 0:
                    base1 = Vector2Int.up;
                    base2 = Vector2Int.up + Vector2Int.left;
                    break;
                case 1:
                    base1 = Vector2Int.left;
                    base2 = Vector2Int.up + Vector2Int.left;
                    break;
                case 2:
                    base1 = Vector2Int.left;
                    base2 = Vector2Int.down + Vector2Int.left;
                    break;
                case 3:
                    base1 = Vector2Int.down;
                    base2 = Vector2Int.down + Vector2Int.left;
                    break;
                case 4:
                    base1 = Vector2Int.down;
                    base2 = Vector2Int.down + Vector2Int.right;
                    break;
                case 5:
                    base1 = Vector2Int.right;
                    base2 = Vector2Int.down + Vector2Int.right;
                    break;
                case 6:
                    base1 = Vector2Int.right;
                    base2 = Vector2Int.up + Vector2Int.right;
                    break;
                case 7:
                    base1 = Vector2Int.up;
                    base2 = Vector2Int.up + Vector2Int.right;
                    break;
                default:
                    base1 = Vector2Int.up;
                    base2 = Vector2Int.one;
                    break;
            }
            int x = Mathf.Abs(offset.x);
            int y = Mathf.Abs(offset.y);
            c1 = Mathf.Abs(x - y);
            c2 = Mathf.Min(x, y);
            int count = c1 + c2;
            if (count > maxCount)
            {
                c1 = Mathf.RoundToInt((float)c1 / count * maxCount);
                c2 = Mathf.RoundToInt((float)c2 / count * maxCount);
            }
            count = c1 + c2;
            List<int> mix = MathTool.MixList(c1, c2);
            Vector2Int[] bases = new Vector2Int[] { base1, base2 };
            Vector2Int current = origin;
            List<Vector2Int> ret = new() { origin };
            for (int i = 0; i < count; i++)
            {
                current += bases[mix[i]];
                ret.Add(current);
            }
            return ret;
        }

        /// <summary>
        /// 计算线段覆盖的坐标为整数的方格（线段从from的中心点到to的中心点）
        /// </summary>
        public static List<Vector2Int> OverlapInt(Vector2Int from, Vector2Int to)
        {
            List<Vector2Int> ret = new() { from };
            OverlapInt(to - from, ret);
            return ret;
        }

        private static void OverlapInt(Vector2Int v, List<Vector2Int> ret)
        {
            void Add(Vector2Int v)
            {
                ret.Add(ret[^1] + v);
            }

            int deltaX = v.x;
            int deltaY = v.y;
            if (deltaX == 0)
            {
                int n = Mathf.Abs(deltaY);
                Vector2Int delta = new(0, deltaY / n);
                for (int i = 0; i < n; i++)
                    Add(delta);
            }
            else if(deltaY == 0)
            {
                int n = Mathf.Abs(deltaX);
                Vector2Int delta = new(deltaX / n, 0);
                for (int i = 0; i < n; i++)
                    Add(delta);
            }
            else if ((deltaX & 1) == 0 || (deltaY & 1) == 0)
            {
                //TODO
            }
            else
            {
                OverlapInt(new Vector2Int(deltaX / 2, deltaY / 2), ret);
                Add(new Vector2Int(deltaX / Mathf.Abs(deltaX), deltaY / Mathf.Abs(deltaY)));
                OverlapInt(new Vector2Int(deltaX / 2, deltaY / 2), ret);
            }
        }
    }
}
