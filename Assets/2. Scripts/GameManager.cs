using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private EntityManager _entityManager;
    
    private World _world;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }
    
    private void Start()
    {
        _world = World.DefaultGameObjectInjectionWorld;
        _entityManager = _world.EntityManager;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            _entityManager.CompleteAllTrackedJobs();
            foreach (var system in _world.Systems)
            {
                system.Enabled = false;
            }

            _world.Dispose();
            DefaultWorldInitialization.Initialize("Default World", false);
            if (!ScriptBehaviourUpdateOrder.IsWorldInCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld))
            {
                ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
            }
        
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
