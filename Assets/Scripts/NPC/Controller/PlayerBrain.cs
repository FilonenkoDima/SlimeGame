using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using InputReader;
using Player;
using UI;
using UI.Enum;

namespace NPC.Controller
{
    public class PlayerBrain : Entity, IDisposable
    {
        private readonly PlayerEntity _playerEntity;
        private readonly List<IEntityInputSource> _inputSources;
        private readonly UIContext _uiContext;

        public PlayerBrain(UIContext uiContext, PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
            : base(playerEntity)
        {
            _playerEntity = playerEntity;
            _uiContext = uiContext;
            _inputSources = inputSources;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        public void Dispose() => ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
        
        private void OnFixedUpdate()
        {
            _playerEntity.MoveHorizontally(GetHorizontalDirection());

            float verticalDirection = GetVerticalDirection();
            _playerEntity.MoveVertically(verticalDirection);

            if (verticalDirection != 0)
                OnVerticalPositionChanged();

            if (IsJump)
                _playerEntity.Jump();

            if (IsAttack)
            {
                _playerEntity.StartAttack();
            }

            if (IsInventoryClicked)
            {
                if (_uiContext.CurrentController?.GetScreenType() == ScreenType.Inventory)
                {
                    _uiContext.CloseScreen();
                }
                else
                {
                    _uiContext.OpenScreen(ScreenType.Inventory);
                }
            }

            foreach (var inputSources in _inputSources)
                inputSources.ResetOneTimeActions();
        }

        private float GetHorizontalDirection()
        {
            foreach (var inputSources in _inputSources)
            {
                if (inputSources.HorizontalDirection == 0)
                    continue;

                return inputSources.HorizontalDirection;
            }

            return 0;
        }

        private float GetVerticalDirection()
        {
            foreach (var inputSources in _inputSources)
            {
                if (inputSources.VerticalDirection == 0)
                    continue;

                return inputSources.VerticalDirection;
            }

            return 0;
        }

        private bool IsJump => _inputSources.Any(source => source.Jump);
        private bool IsAttack => _inputSources.Any(source => source.Attack);
        private bool IsInventoryClicked => _inputSources.Any(source => source.InventoryClicked);
    }
}