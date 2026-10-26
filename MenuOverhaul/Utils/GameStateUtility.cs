using UnityEngine;
using MenuOverhaul.Helpers;
using MenuOverhaul.Infrastructure.Diagnostics;

namespace MenuOverhaul.Utils
{
    public static class GameStateUtility
    {
        private static bool isInGame;

        public static void SetGameStarted(bool started)
        {
            isInGame = started;
        }

        public static bool IsInGame()
        {
            return isInGame;
        }

        public static void ConfigureDecalPlane(bool enable) { }
        public static void SetDecalPlanePosition(float xPosition, float yOffset) { }
        public static void ResetGameState()
        {
            isInGame = false;
        }
    }
}