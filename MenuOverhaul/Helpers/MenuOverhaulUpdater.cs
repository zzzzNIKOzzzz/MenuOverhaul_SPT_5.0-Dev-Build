using UnityEngine;
using MenuOverhaul.Helpers.Services;
using MenuOverhaul.Utils;
using EFT.UI;
using System.Collections.Concurrent;

namespace MenuOverhaul.Helpers
{
    internal class MenuOverhaulUpdater : MonoBehaviour
    {
        private static MenuOverhaulUpdater _instance;
        private readonly ConcurrentQueue<System.Action> _mainThreadActions = new ConcurrentQueue<System.Action>();

        private float _timer = 0f;
        private bool _hasUpdatedButtons = false;
        private bool _factionLogoApplied = false;

        private void Awake()
        {
            _instance = this;
        }

        public static void ExecuteOnMainThread(System.Action action)
        {
            if (_instance != null)
            {
                _instance._mainThreadActions.Enqueue(action);
            }
            else
            {
                action?.Invoke();
            }
        }

        private void Update()
        {
            while (_mainThreadActions.TryDequeue(out var action))
            {
                try 
                { 
                    action?.Invoke(); 
                } 
                catch (System.Exception ex) 
                { 
                    Debug.LogError(ex.ToString()); 
                }
            }

            if (!MenuScreenVisibilityPolicy.IsMainMenuActive) 
            {
                _factionLogoApplied = false;
                _hasUpdatedButtons = false;
                _timer = 0f;
                return;
            }

            if (!_factionLogoApplied)
            {
                _factionLogoApplied = MainMenuLayoutService.UpdateFactionLogo();
            }

            if (!_hasUpdatedButtons)
            {
                _timer += Time.deltaTime;
                
                GameObject menuScreenObject = GameObject.Find(MenuOverhaulConstants.MenuScreen.ScenePath);
                if (menuScreenObject != null)
                {
                    Canvas.ForceUpdateCanvases(); 
                    
                    var animations = menuScreenObject.GetComponentsInChildren<DefaultUIButtonAnimation>(true);
                    foreach (var anim in animations)
                    {
                        if (anim != null)
                        {
                            MainMenuButtonAnimationService.ApplyIdle(anim, false);
                        }
                    }
                }

                if (_timer > 1.5f)
                {
                    _hasUpdatedButtons = true;
                }
            }
        }
    }
}