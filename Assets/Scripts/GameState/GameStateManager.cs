using UnityEngine;
using Utility.Singleton;
using EventHandling;
using EventHandling.Events;
namespace GameStates
{
    public enum EGameState
    {
        FIRSTPERSONMOVEMENT,
        INTERACTION,
        COMPUTERINTERACTION,

    }


    public class GameStateManager : SingletonMonoBehavior<GameStateManager>
    {
        private EGameState currentGameState;
        [SerializeField] private EGameState gameStartState = EGameState.FIRSTPERSONMOVEMENT;


        protected override void Awake()
        {
            base.Awake();
            
            currentGameState = gameStartState;
        }
        private void Start()
        {
            EventBus<OnGameStateChange>.Raise(new OnGameStateChange()
            {

                _gameState = gameStartState
            }) ;
            
        }

    }
}