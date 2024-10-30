using UnityEngine;
using CHARACTERS;
using TMPro;

namespace DIALOGUE
{
    [CreateAssetMenu(fileName= "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Assest")]
    /// <summary>
    /// Scriptable Object that defines the parameters for configuring the dialogue system as a whole
    /// </summary>
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigurationAssets;

        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;
    }
}
