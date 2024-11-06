using DIALOGUE;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHARACTERS
{
    /// <summary>
    /// The contral hub for creating, retrieving, and managing characters in the scene.
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        private const string CHARACTER_CASTING_ID = " as ";
        private const string CHARACTER_NAME_ID = "<charname>";
        public string characterRootPathFormat => $"Characters/{CHARACTER_NAME_ID}";
        public string characterPrefabPathFormat => $"{characterRootPathFormat}/Character - [{CHARACTER_NAME_ID}]";
        public string characterPrefabNameFormat => $"Character - [{CHARACTER_NAME_ID}]";
        public static CharacterManager Instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.Instance.config.characterConfigurationAssets;

        [SerializeField] private RectTransform _characterpanel = null;
        public RectTransform characterPanel => _characterpanel;



        private void Awake()
        {
            Instance = this;
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {

            //存的名字不对
            if (characters.ContainsKey(characterName.ToLower()))
            {
                return characters[characterName.ToLower()];
            }
            else if (createIfDoesNotExist)
                return CreateCharacter(characterName);

            return null;
        }

        public Character CreateCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower())) {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the character.");
                return null;
            }

            CHARACTER_INFO info = GetCharaterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);
            characters.Add(info.name.ToLower(), character);

            return character;
        }

        private CHARACTER_INFO GetCharaterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();

            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);

            result.name = nameData[0];

            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;

            result.config = config.GetConfig(result.castingName);

            result.prefab = GetPrefabForCharacter(result.castingName) ;

            result.rootCharacterFolder = FormatCharacterPath(characterRootPathFormat, result.castingName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPathFormat, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        public string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;

            
            switch (config.characterType) {
                case Character.CharacterType.Text:
                    return new Character_Text(info.name, config);
                case Character.CharacterType.SpriteSheet:
                case Character.CharacterType.Sprite:
                    return new Character_Sprite(info.name, config, info.prefab, info.rootCharacterFolder);
                case Character.CharacterType.Live2D:
                    return new Character_Live2D(info.name, config, info.prefab, info.rootCharacterFolder);
                case Character.CharacterType.Model3D:
                    return new Character_Model3D(info.name, config, info.prefab, info.rootCharacterFolder);
                default:
                    return null;
            }
        }

        public void SortCharacters()
        {
            //排除场景中每个Active == false的角色
            List<Character> activeCharacters = characters.Values.Where( c => c.root.gameObject.activeInHierarchy && c.isVisible).ToList();
            List<Character> inactiveCharacters = characters.Values.Except(activeCharacters).ToList();

            activeCharacters.Sort((a, b) => a.priority.CompareTo(b.priority));//升序排列

            SortCharacters(activeCharacters);
        }

        public void SortCharacters(string[] characterNames)
        {

            // 通过名字排序，characterNamse数组中最前面的在屏幕最前面
            //var sortedCharacters = new List<Character>();
            var sortedCharacters = characterNames
                .Select(name => GetCharacter(name))
                .Where(character => character != null)
                .ToList();


            List<Character> remainingCharacters = characters.Values
                .Except(sortedCharacters)
                .OrderBy(character => character.priority)
                .ToList();
            //修改准备排序的Priority
            //原始队列上的最大优先级为新队列另一个角色的最大优先级 输入全部名字会清除优先级
            sortedCharacters.Reverse();
            int startingPriority = remainingCharacters.Count > 0 ? remainingCharacters.Max( character => character.priority ) : 0;
            for (int i = 0; i < sortedCharacters.Count; i++)
            {
                Character character = sortedCharacters[i];
                character.SetPriority(startingPriority + i + 1, autoSetCharactersOnUI: false);
            }
            List<Character> allCharacters = remainingCharacters.Concat(sortedCharacters).ToList();
            SortCharacters(allCharacters); 
        }

        private void SortCharacters(List<Character> charactersSortingOrder) 
        {
            //inActive的设置为最后面
            int i = 0;
            foreach (Character character in charactersSortingOrder) 
            {
                Debug.Log($"{character.name} priority is '{character.priority}'");
                character.root.SetSiblingIndex(i++);
            }
        }

        private class CHARACTER_INFO
        {
            public string name = "";
            public string castingName = "";

            public string rootCharacterFolder = "";

            public CharacterConfigData config = null;

            public GameObject prefab = null;
        }
    }
}
