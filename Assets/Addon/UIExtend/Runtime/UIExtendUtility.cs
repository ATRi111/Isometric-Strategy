using UnityEngine;

namespace UIExtend
{
    public static class UIExtendUtility
    {
        /// <summary>
        /// �ж�ĳ��UI�Ƿ���ȫλ����Ļ��Χ��(ֻ������ScreenSpace-Overlay��UI)
        /// </summary>
        /// <param name="padding">��϶(����Ļ��ȵİٷֱȱ�ʾ)</param>
        public static bool WithinScreen(RectTransform rectTransform, float padding = 0.01f)
        {
            padding *= Screen.width;
            GetBorder(rectTransform, out float left, out float right, out float bottom, out float top);
            return left >= padding  && right <= Screen.width - padding
                && bottom >= padding && top <= Screen.height - padding;
        }

        /// <summary>
        /// ��UI��������Ļ��Χ��(ֻ������ScreenSpace-Overlay��UI)
        /// </summary>
        /// <param name="padding">��϶(����Ļ��ȵİٷֱȱ�ʾ)</param>
        public static void ClampInScreen(RectTransform rectTransform, float padding = 0.01f)
        {
            padding *= Screen.width;
            float x = rectTransform.position.x;
            float y = rectTransform.position.y;
            GetBorder(rectTransform, out float left, out float right, out float bottom, out float top);
            x = Mathf.Clamp(x, padding + x - left, Screen.width - padding + x - right);
            y = Mathf.Clamp(y, padding + y - bottom, Screen.height - padding + y - top);
            rectTransform.position = new(x, y);
        }

        /// <summary>
        /// ��ȡrectTransform���ı߽磨��������ϵ��
        /// </summary>
        public static void GetBorder(RectTransform rectTransform, out float left, out float right, out float bottom, out float top)
        {
            Vector3[] coners = new Vector3[4];
            rectTransform.GetWorldCorners(coners);
            left = coners[0].x;
            bottom = coners[0].y;
            right = coners[2].x;
            top = coners[2].y;
        }
    }
}