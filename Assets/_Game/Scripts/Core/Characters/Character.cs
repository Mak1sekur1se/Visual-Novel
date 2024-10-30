using DIALOGUE;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace CHARACTERS
{
    /// <summary>
    /// The base class from which all character types derive from
    /// </summary>

    public abstract class Character
    {
        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;

        public DialogueSystem dialogueSystem => DialogueSystem.Instance;

        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            displayName = name;
            this.config = config;
        }


        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationsOnScreen();
            return dialogueSystem.Say(dialogue);
        }

        //动态更改角色文字外观
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void ResetConfigurationData() => config = CharacterManager.Instance.GetCharacterConfig(name);

        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }
    }
}
