using System;
using System.Collections.Generic;
using UnityEngine;

namespace Memoria.Bloomtown.Framework.Reverse;

public sealed class GameObjectAnalyzer
{
	public enum Result
	{
		SkipChildren = 1,
		AnalyzeChildren = 2,
	}

	public delegate Result OnObjectDelegate(GameObject gameObject);

	public event OnObjectDelegate OnObject;

	private readonly LinkedList<GameObject> _queue = new();

	public GameObjectAnalyzer(IEnumerable<GameObject> gameObjects)
	{
		foreach (GameObject gameObject in gameObjects)
			_queue.AddLast(gameObject);
	}

	public void VisitAllObjects()
	{
		while (_queue.Count > 0)
		{
			LinkedListNode<GameObject> first = _queue.First;
			
			Result result = TriggerOnObject(first.Value);
			
			if (result == Result.AnalyzeChildren)
			{
				foreach (Transform child in first.Value.transform)
					_queue.AddBefore(first, child.gameObject);
			}

			_queue.Remove(first);
		}
	}

	private Result TriggerOnObject(GameObject gameObject)
	{
		Result result = Result.SkipChildren;
		Delegate[] delegateArray = OnObject?.GetInvocationList();
		if (delegateArray != null)
		{
			foreach (OnObjectDelegate handler in delegateArray)
			{
				Result handlerResult = handler(gameObject);
				if (handlerResult > result)
					result = handlerResult;
			}
		}
		else
		{
			throw new InvalidOperationException("You need to subscribe OnObject.");
		}
		return result;
	}
}

// ReSharper disable InconsistentNaming