using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Memoria.Bloomtown.Framework.Reverse;

public sealed class SerializedDataAnalyzer
{
    public delegate void OnDataDelegate(String name, Object data);

    private readonly Dictionary<Type, Queue<OnDataDelegate>> _delegates = new();
    private readonly Dictionary<Type, IReadOnlyList<FieldInfo>> _serializedFields = new();
    private readonly LinkedList<(String name, Object data)> _queue = new();

    public SerializedDataAnalyzer()
    {
    }

    public void Register<T>(OnDataDelegate onData)
    {
        Type type = typeof(T);
        if (type.IsAbstract || type.IsInterface)
            throw new NotSupportedException("Registration by parent type is not yet supported.");

        if (!_delegates.TryGetValue(type, out Queue<OnDataDelegate> queue))
        {
            queue = new Queue<OnDataDelegate>();
            _delegates.Add(type, queue);
        }

        queue.Enqueue(onData);
    }

    public void VisitAllData(String name, Object data)
    {
        _queue.AddLast((name, data));

        while (_queue.Count > 0)
        {
            LinkedListNode<(String name, Object data)> first = _queue.First;
            (name, data) = first.Value;
			
            Type type = data.GetType();
            if (_delegates.TryGetValue(type, out Queue<OnDataDelegate> callbacks))
            {
                foreach (OnDataDelegate callback in callbacks)
                    callback(name, data);
            }

            if (!_serializedFields.TryGetValue(type, out IReadOnlyList<FieldInfo> visitableFields))
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(field => CustomAttributeExtensions.GetCustomAttribute<SerializeField>((MemberInfo)field) != null)
                    .Where(field => !field.GetType().IsPrimitive)
                    .ToArray();

                visitableFields = fields;
                _serializedFields.Add(type, visitableFields);
            }

            foreach (FieldInfo field in visitableFields)
            {
                Object children = field.GetValue(data);
                if (children is not null)
                    _queue.AddBefore(first, (field.Name, children));
            }

            _queue.Remove(first);
        }
    }
}