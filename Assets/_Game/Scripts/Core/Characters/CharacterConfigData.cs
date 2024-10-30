using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    /// <summary>
    /// A data container that defines the configuration parameters for a character in the visual novel
    /// </summary>
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;


        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;

        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;

            result.nameFont = nameFont;
            result.dialogueFont = dialogueFont;
            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            return result;
        }

        private static Color defaultColor => DialogueSystem.Instance.config.defaultTextColor;
        private static TMP_FontAsset defaultFont => DialogueSystem.Instance.config.defaultFont;

        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = string.Empty;
                result.alias = string.Empty;
                result.characterType = Character.CharacterType.Text;

                result.nameFont = defaultFont;
                result.dialogueFont = defaultFont;
                result.nameColor = defaultColor;
                result.dialogueColor = defaultColor;

                return result;
            }
        }
    }
}
