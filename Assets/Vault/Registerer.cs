using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vault
{
    public abstract class Registerer : MonoBehaviour
    {

        public List<IController> Controllers = new List<IController>();
        public List<ITick> Ticks = new List<ITick>();
        public List<IFixedTick> FixedTicks = new List<IFixedTick>();
        public List<ILateTick> LateTicks = new List<ILateTick>();

        public void Awake()
        {
            OnAwake();
            InitializeListeners();
            NotifyControllers();
        }
        public void OnEnable()
        {
            Enable();
            NotifyOnActivated();
        }

        public void Start()
        {
            OnStart();
            NotifyOnStarted();

        }

        public void Update()
        {
            NotifyUpdates();

        }
        public void OnDisable()
        {
            NotifyOnDisabled();
            RemoveListeners();
            RevomeControllers();


        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #region public methods

        public abstract void OnAwake();
        public abstract void Enable();
        public abstract void OnStart();


        //call this to add a Controller or any tick
        public void AddController(IController controller)
        {
            if (!Controllers.Contains(controller))
            {
                Controllers.Add(controller);
                AddRespectiveTicks(controller);
            }
            else
            {
                Debug.LogError("Observer not added or not the right type of observer");
            }
        }

        public void AddRespectiveTicks(IController controller)
        {
            if (controller is ITick)
            {
                Ticks.Add((ITick)controller);
            }
            else if (controller is IFixedTick)
            {
                FixedTicks.Add((IFixedTick)controller);
            }
            else if (controller is ILateTick)
            {
                LateTicks.Add((ILateTick)controller);
            }
        }
        #endregion

        #region private methods


        private void InitializeListeners()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnRegisterListeners();
            }
        }

        private void RemoveListeners()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnRemoveListeners();
            }
        }

        private void NotifyControllers()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnInitialized();
            }
        }

        private void NotifyOnActivated()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnVisible();
            }
        }

        private void NotifyOnStarted()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnStarted();
            }
        }

        private void NotifyOnDisabled()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnRelease();
            }
        }

        private void NotifyUpdates()
        {
            foreach(ITick tick in Ticks)
            {
                tick.OnUpdate();
            }
        }


        private void RevomeControllers()
        {
           Controllers.Clear();
            Ticks.Clear();
            FixedTicks.Clear();
            LateTicks.Clear();
        }
        #endregion
    }

}
