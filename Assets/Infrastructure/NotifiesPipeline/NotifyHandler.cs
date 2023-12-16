using System.Collections.Generic;
using UnityEngine;

public class NotifyHandler : MonoBehaviour
{
    [SerializeField] bool LogNotifies;

    IListener[] listeners;

    Queue<NotifyPackage> notifyQueue = new();

    public static NotifyHandler N { get; private set; }

    public void QueueNotify(Notifies notifyID, params object[] data)
    {
        if (LogNotifies)
        {
            Debug.LogWarning($"Notify {notifyID} added to queue.");
        }

        notifyQueue.Enqueue(new NotifyPackage(notifyID, data));
    }

    void GetAllListenersInScene()
    {
        GameObject[] gameObjectsInScene = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<IListener> listenersInScene = new();
        for (int i = 0; i < gameObjectsInScene.Length; i++)
        {
            if (gameObjectsInScene[i].TryGetComponent(out IListener listener))
                listenersInScene.Add(listener);
        }
        listeners = listenersInScene.ToArray();

        if (LogNotifies)
        {
            Debug.LogWarning($"Found {listeners.Length} listener(s).");
        }
    }

    void Awake()
    {
        N = this;
        GetAllListenersInScene();
    }

    void Update()
    {
        if (notifyQueue.Count > 0)
        {
            NotifyPackage eventPackage = notifyQueue.Dequeue();
            for (int i = 0; i < listeners.Length; i++)
                listeners[i].IOnNotify(eventPackage.notifyID, eventPackage.data);
        }
    }
}

public struct NotifyPackage
{
    public Notifies notifyID;
    public object[] data;

    public NotifyPackage(Notifies notifyID, params object[] data)
    {
        this.notifyID = notifyID;
        this.data = data;
    }
}