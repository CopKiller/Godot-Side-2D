using System;
using System.Collections.Generic;
using Godot;
using Side2D.Logger;
using Side2D.scripts;
using Side2D.scripts.Alert;

namespace Side2D.scripts.Host;

public sealed partial class ApplicationHost : Node {
    // Singleton
    public static ApplicationHost Instance { get; private set; }
    
    private readonly Dictionary<Type, Node> _singletons = new();
    
    public ApplicationHost() {
        Instance = this;
    }

    public override void _EnterTree()
    {
        Log.LogInstance = new Alert.Logger();
        
        AddSingleton(new ClientManager());
        AddSingleton(new AlertManager());
    }

    public void AddSingleton<T>(T node) where T : Node {
        _singletons.Add(typeof(T), node);
        GetTree().Root.CallDeferred("add_child", node);
    }
    public void RemoveSingleton<T>() where T : Node {
        var singleton = _singletons[typeof(T)];
        singleton.QueueFree();
        _singletons.Remove(typeof(T));
    }
    public T GetSingleton<T>() where T : Node {
        return (T)_singletons[typeof(T)];
    }
    public SceneTree GetSceneTree() {
        return GetTree();
    }
    public override void _ExitTree() {
        foreach (var singleton in _singletons.Values) {
            singleton.QueueFree();
        }
    }
}