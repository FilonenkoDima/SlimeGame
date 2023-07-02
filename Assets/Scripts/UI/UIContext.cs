using System;
using System.Collections.Generic;
using Items.Core;
using Player;
using UI.Core;
using UI.Enum;
using UI.Impl.Controller;
using UI.Impl.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class UIContext : IDisposable
    {
        private const string LoadPath = "Prefabs/UI/Screens/";

        private readonly Dictionary<ScreenType, IScreenController> _controllers;
        private readonly GameObject _uiContainer;
        private readonly PlayerEntity _playerEntity;
        private readonly SceneItemsSystem _sceneItemsSystem;

        public IScreenController CurrentController { private set; get; }


        public UIContext(PlayerEntity playerEntity, SceneItemsSystem sceneItemsSystem)
        {
            
            _uiContainer = new GameObject
            {
                name = nameof(UIContext)
            };

            _playerEntity = playerEntity;
            _sceneItemsSystem = sceneItemsSystem;

            _controllers = new Dictionary<ScreenType, IScreenController>();

            /*foreach (var inputSource in _inputSources)
            {
                inputSource.InventoryRequested += OpenInventory;
            }*/

            /*var container = new GameObject()
            {
                name = nameof(UIContext)
            };
            _uiContainer = container.transform;*/
        }

        public void OpenScreen(ScreenType screenType)
        {
            CurrentController?.OnCloseRequest();

            if (!_controllers.TryGetValue(screenType, out var controller))
            {
                controller = GetController(screenType);
                controller.OnInit();
                _controllers.Add(screenType, controller);
            }

            CurrentController = controller;
            CurrentController.OnOpenRequest();
        }

        public void CloseScreen()
        {
            CurrentController?.OnCloseRequest();
            CurrentController = null;
        }

        public void Dispose()
        {
        }

        private IScreenController GetController(ScreenType screenType)
        {
            return screenType switch
            {
                ScreenType.Inventory =>
                    new InventoryScreenController(
                        this, GetView<InventoryScreenView>(screenType), _playerEntity, _sceneItemsSystem
                    ),
                _ => throw new Exception()
            };
        }

        private TView GetView<TView>(ScreenType screenType) where TView : ScreenView
        {
            var path = $"{LoadPath}{screenType.ToString()}";
            var prefab = Resources.Load<TView>(path);
            return Object.Instantiate(prefab, _uiContainer.transform);
        }
    }
}