using System;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// ��ɫ���ԣ������ڱ�ʾ�ܶ�������Ӱ�죬�����仯����
    /// </summary>
    public abstract class Property<T> where T : struct
    {
        public T defaultValue;
        public T CurrentValue { get; protected set; }

        public Action<Property<T>> DirectAdd;         //ֱ�Ӽ���
        public Action<Property<T>> DirectMultiply;    //ֱ�ӳ���
        public Action<Property<T>> FinalAdd;          //���ռ���
        public Action<Property<T>> FinalMultiply;     //���ճ���
        public Action<Property<T>> FinalClamp;        //����ȡֵ��Χ

        public abstract void Add(T value);
        public abstract void Multiply(T value);
        public abstract void Clamp(T min, T max);

        public void Refresh()
        {
            CurrentValue = defaultValue;
            DirectAdd?.Invoke(this);
            DirectMultiply?.Invoke(this);
            FinalAdd?.Invoke(this);
            FinalMultiply?.Invoke(this);
            FinalClamp?.Invoke(this);
        }
    }

    [Serializable]
    public sealed class IntProperty : Property<int>
    {
        public override void Add(int value)
        {
            CurrentValue += value;
        }

        public override void Multiply(int value)
        {
            CurrentValue *= value;
        }

        public override void Clamp(int min,int max)
        {
            CurrentValue = Mathf.Clamp(CurrentValue, min, max);
        }
    }

    [Serializable]
    public sealed class FloatProperty : Property<float>
    {
        public override void Add(float value)
        {
            CurrentValue += value;
        }

        public override void Multiply(float value)
        {
            CurrentValue *= value;
        }

        public override void Clamp(float min, float max)
        {
            CurrentValue = Mathf.Clamp(CurrentValue, min, max);
        }
    }
}