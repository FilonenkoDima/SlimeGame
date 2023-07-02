using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Services.Updater;
using Drawing;
using InputReader;
using Items.Core;
using NPC.Enum;
using NPC.Spawn;
using Player;
using UI;
using UnityEngine;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity playerEntity;
        [SerializeField] private GameUIInputView gameUIInputView;
        [SerializeField] private ItemRegistry itemRegistry;
        [SerializeField] private Transform _spawnPoint;

        private ExternalDevicesInputReader _externalDevicesInput;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private SceneItemsSystem _sceneItemsSystem;
        private UIContext _uiContext;

        private List<IDisposable> _disposables;
        private LevelDrawer _levelDrawer;
        private EntitySpawnerController _entitySpawnerController;

        private bool _onPause;

        private void Awake()
        {
            _disposables = new List<IDisposable>();
            InitProjectUpdater();

            _sceneItemsSystem = new SceneItemsSystem(playerEntity);
            _uiContext = new UIContext(playerEntity, _sceneItemsSystem);
            _externalDevicesInput = new ExternalDevicesInputReader(_projectUpdater);

            InitPlayerSystem();

            _disposables.Add(_sceneItemsSystem);
            _disposables.Add(_externalDevicesInput);

            _levelDrawer = new LevelDrawer(LevelId.Level1);
            _levelDrawer.RegisterElement(_playerSystem.PlayerBrain);

            _entitySpawnerController = new EntitySpawnerController(_levelDrawer);
        }

        private void Start()
        {
            Item item = itemRegistry.GetItemById(ItemId.Stick);
            playerEntity.EquipmentInventory.AddItem(new ItemContainer(item, 1), false);
        }

        private void InitProjectUpdater()
        {
            if (ProjectUpdater.Instance == null)
            {
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
                _projectUpdater.name = nameof(ProjectUpdater);
            }
            else
            {
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;
            }
        }

        private void InitPlayerSystem()
        {
            _playerSystem = new PlayerSystem(_uiContext, playerEntity, new List<IEntityInputSource>
            {
                gameUIInputView,
                _externalDevicesInput,
            });
            
            _disposables.Add(_playerSystem);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //todo single responsibility issue, fix that
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
            }

            if (Input.GetKeyDown(KeyCode.Q))
                _entitySpawnerController.SpawnEntity(EntityId.Demon, _spawnPoint.position);

            if (Input.GetKeyDown(KeyCode.U))
                _entitySpawnerController.SpawnEntity(EntityId.King, _spawnPoint.position);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}