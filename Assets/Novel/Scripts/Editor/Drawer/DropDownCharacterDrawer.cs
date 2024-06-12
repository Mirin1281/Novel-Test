using UnityEngine;
using UnityEditor;

namespace Novel.Editor
{
    /// <summary>
    /// 引数は変数名にしてください(nameof推奨)
    /// </summary>
    [CustomPropertyDrawer(typeof(DropDownCharacterAttribute))]
    public class DropDownCharacterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            DropDownCharacterAttribute commentAttribute = attribute as DropDownCharacterAttribute;
            CommandDrawerUtility.DropDownCharacterList(rect, property, commentAttribute.FieldName);
        }
    }
}
