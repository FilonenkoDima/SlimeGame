using System;
using System.Collections;
using UnityEngine;

public interface IProjectUpdater
{
    event Action UpdateCalled;
    event Action FixedUpdateCalled;
    event Action LateUpdateCalled;
    Coroutine StartCoroutine(IEnumerator coroutine);
    void StopCoroutine(Coroutine coroutine);
    
}
