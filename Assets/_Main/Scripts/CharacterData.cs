using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "CharacterClass", menuName = "Character Data", order = 0)]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterAvatar;

}
