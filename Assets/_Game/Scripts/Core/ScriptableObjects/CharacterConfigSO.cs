using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    [CreateAssetMenu(fileName ="Character Configuration Asset", menuName = "Dialogue System/Character Configuration Asset")]
    public class CharacterConfigSO : ScriptableObject
    {
        public CharacterConfigData[] characters;

        /// <summary>
        /// 可编辑对象的数据修改是持久化的应该返回一个副本避免对原数据的修改
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        public CharacterConfigData GetConfig(string characterName)
        {
            characterName = characterName.ToLower();

            for (int i = 0; i < characters.Length; i++)
            {
                CharacterConfigData data = characters[i];

                if (string.Equals(characterName, data.name.ToLower() )|| string.Equals(characterName, data.alias.ToLower()))
                    return data.Copy();
            }

            return CharacterConfigData.Default;

        }

    }
}
