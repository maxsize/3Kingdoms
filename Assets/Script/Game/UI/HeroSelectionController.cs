using Adic;
using System.Collections;
using System.Collections.Generic;
using ThreeK.Game.Data;
using ThreeK.Game.StateMachine;
using ThreeK.Game.UI;
using UnityEngine;

namespace ThreeK.Game.UI
{
    public class HeroSelectionController : MonoBehaviour
    {
        [Inject][HideInInspector] public MainUI MainUI;
        [Inject][HideInInspector] public PlayerVO PlayerData;

        private void Start()
        {
            this.Inject();
        }

        public void OnSelected(string heroName)
        {
            PlayerData.HeroName = heroName;
            // FIX ME LATER
            PlayerData.Abilities = new[] { "BladeFury" };
            Debug.LogError(heroName + " selected.");
            MainUI.HandleInput(new LobbyInput());
        }
    }
}
