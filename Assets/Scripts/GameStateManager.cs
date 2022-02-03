using System;
using System.Collections;
using System.Collections.Generic;
using Deft;

public class GameStateManager : Manager<GameStateManager>
{
    public delegate void GameStateChangedHandler(GameState state);
    public event GameStateChangedHandler eventExitGameState;
    public event GameStateChangedHandler eventEnterGameState;

    public GameState State { get; private set; }

    // TODO: Factor out. We've lost our "Bootstrapper class".
    // GameStateManager shouldn't be setting its own state.
    public override void OnStart()
    {
        SetState(GameState.MainMenu);
    }

    // NOTE: Can re-add "forceState" if necessary, but would prefer not to.
    public void SetState(GameState newState)
    {
        if (State != newState)
        {
            GameState oldState = State;
            State = newState;

            eventExitGameState?.Invoke(oldState);
            eventEnterGameState?.Invoke(newState);
        }
    }



    //public override void OnStart()
    //{
    //    base.OnStart();
    //    StartCoroutine(JoinServer());
    //}

    //IEnumerator JoinServer()
    //{
    //    //yield return new WaitForSeconds(0.5f);
    //    yield return null;
    //    NetworkManagerClient.Get.JoinServer(hostName, port);
    //}
}
