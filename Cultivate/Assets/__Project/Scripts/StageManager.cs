using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private readonly int MAX_ACTION_COUNT = 128;

    public StageHero _hero;
    public StageEnemy _enemy;

    public void Enter()
    {
        _hero = new StageHero(RunManager.Instance.Hero);
        _enemy = new StageEnemy(RunManager.Instance.Enemy);

        Simulate();
        // play animation

        // after animation is finished, exit
    }

    private void Simulate()
    {
        bool heroTurn = true;

        // register passive chips

        for (int i = 0; i < MAX_ACTION_COUNT; i++)
        {
            if (heroTurn)
            {
                Debug.Log("hero act");
                _hero.Execute();
            }
            else
            {
                Debug.Log("enemy act");
                _enemy.Execute();
            }

            heroTurn = !heroTurn;
        }
    }

    public void Exit()
    {
        // apply outcome to run manager
    }
}
