#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Refsa.Collections.String;

[CustomPropertyDrawer(typeof(NativeString_32))]
public class NativeStringDrawer: PropertyDrawer 
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        EditorGUI.BeginProperty(position, label, property);

        var bufferProp = property.FindPropertyRelative("buffer");

        var targetObject = property.serializedObject.targetObject;
        // var targetNativeString = (NativeString) targetObject.GetType().GetField("TestString").GetValue(targetObject);
        string currentValue = GetString(bufferProp);

        string newValue = EditorGUI.TextField(position, "Value", currentValue);

        if (currentValue != newValue)
        {
            Clear(bufferProp);
            Write(bufferProp, newValue);

            EditorUtility.SetDirty(targetObject);
            property.serializedObject.ApplyModifiedProperties();
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }

        EditorGUI.EndProperty();
    }

    void Write(SerializedProperty bufferProp, string newValue)
    {
        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(newValue);
        try
        {
            bufferProp.FindPropertyRelative("byte0000").intValue = utf8Bytes[0];
            bufferProp.FindPropertyRelative("byte0000").intValue = utf8Bytes[0];
            bufferProp.FindPropertyRelative("byte0001").intValue = utf8Bytes[1];
            bufferProp.FindPropertyRelative("byte0002").intValue = utf8Bytes[2];
            bufferProp.FindPropertyRelative("byte0003").intValue = utf8Bytes[3];
            bufferProp.FindPropertyRelative("byte0004").intValue = utf8Bytes[4];
            bufferProp.FindPropertyRelative("byte0005").intValue = utf8Bytes[5];
            bufferProp.FindPropertyRelative("byte0006").intValue = utf8Bytes[6];
            bufferProp.FindPropertyRelative("byte0007").intValue = utf8Bytes[7];
            bufferProp.FindPropertyRelative("byte0008").intValue = utf8Bytes[8];
            bufferProp.FindPropertyRelative("byte0009").intValue = utf8Bytes[9];
            bufferProp.FindPropertyRelative("byte0010").intValue = utf8Bytes[10];
            bufferProp.FindPropertyRelative("byte0011").intValue = utf8Bytes[11];
            bufferProp.FindPropertyRelative("byte0012").intValue = utf8Bytes[12];
            bufferProp.FindPropertyRelative("byte0013").intValue = utf8Bytes[13];

            var byte14Prop = bufferProp.FindPropertyRelative("byte0014");
            byte14Prop.FindPropertyRelative("byte0000").intValue = utf8Bytes[14];
            byte14Prop.FindPropertyRelative("byte0001").intValue = utf8Bytes[15];
            byte14Prop.FindPropertyRelative("byte0002").intValue = utf8Bytes[16];
            byte14Prop.FindPropertyRelative("byte0003").intValue = utf8Bytes[17];
            byte14Prop.FindPropertyRelative("byte0004").intValue = utf8Bytes[18];
            byte14Prop.FindPropertyRelative("byte0005").intValue = utf8Bytes[19];
            byte14Prop.FindPropertyRelative("byte0006").intValue = utf8Bytes[20];
            byte14Prop.FindPropertyRelative("byte0007").intValue = utf8Bytes[21];
            byte14Prop.FindPropertyRelative("byte0008").intValue = utf8Bytes[22];
            byte14Prop.FindPropertyRelative("byte0009").intValue = utf8Bytes[23];
            byte14Prop.FindPropertyRelative("byte0010").intValue = utf8Bytes[24];
            byte14Prop.FindPropertyRelative("byte0011").intValue = utf8Bytes[25];
            byte14Prop.FindPropertyRelative("byte0012").intValue = utf8Bytes[26];
            byte14Prop.FindPropertyRelative("byte0013").intValue = utf8Bytes[27];
            byte14Prop.FindPropertyRelative("byte0014").intValue = utf8Bytes[28];
            byte14Prop.FindPropertyRelative("byte0015").intValue = utf8Bytes[29];
        }
        catch {}
    }

    void Clear(SerializedProperty bufferProp)
    {
        try
        {
            bufferProp.FindPropertyRelative("byte0000").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0000").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0001").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0002").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0003").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0004").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0005").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0006").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0007").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0008").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0009").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0010").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0011").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0012").intValue = 0b0000;
            bufferProp.FindPropertyRelative("byte0013").intValue = 0b0000;

            var byte14Prop = bufferProp.FindPropertyRelative("byte0014");
            byte14Prop.FindPropertyRelative("byte0000").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0001").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0002").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0003").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0004").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0005").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0006").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0007").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0008").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0009").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0010").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0011").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0012").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0013").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0014").intValue = 0b0000;
            byte14Prop.FindPropertyRelative("byte0015").intValue = 0b0000;
        }
        catch {}
    }

    string GetString(SerializedProperty bufferProp)
    {
        var utf8Bytes = new byte[30];
        try
        {
            utf8Bytes[0] = (byte) bufferProp.FindPropertyRelative("byte0000").intValue;
            utf8Bytes[0] = (byte) bufferProp.FindPropertyRelative("byte0000").intValue;
            utf8Bytes[1] = (byte) bufferProp.FindPropertyRelative("byte0001").intValue;
            utf8Bytes[2] = (byte) bufferProp.FindPropertyRelative("byte0002").intValue;
            utf8Bytes[3] = (byte) bufferProp.FindPropertyRelative("byte0003").intValue;
            utf8Bytes[4] = (byte) bufferProp.FindPropertyRelative("byte0004").intValue;
            utf8Bytes[5] = (byte) bufferProp.FindPropertyRelative("byte0005").intValue;
            utf8Bytes[6] = (byte) bufferProp.FindPropertyRelative("byte0006").intValue;
            utf8Bytes[7] = (byte) bufferProp.FindPropertyRelative("byte0007").intValue;
            utf8Bytes[8] = (byte) bufferProp.FindPropertyRelative("byte0008").intValue;
            utf8Bytes[9] = (byte) bufferProp.FindPropertyRelative("byte0009").intValue;
            utf8Bytes[10] = (byte) bufferProp.FindPropertyRelative("byte0010").intValue;
            utf8Bytes[11] = (byte) bufferProp.FindPropertyRelative("byte0011").intValue;
            utf8Bytes[12] = (byte) bufferProp.FindPropertyRelative("byte0012").intValue;
            utf8Bytes[13] = (byte) bufferProp.FindPropertyRelative("byte0013").intValue;

            var byte14Prop = bufferProp.FindPropertyRelative("byte0014");
            utf8Bytes[14] = (byte) byte14Prop.FindPropertyRelative("byte0000").intValue;
            utf8Bytes[15] = (byte) byte14Prop.FindPropertyRelative("byte0001").intValue;
            utf8Bytes[16] = (byte) byte14Prop.FindPropertyRelative("byte0002").intValue;
            utf8Bytes[17] = (byte) byte14Prop.FindPropertyRelative("byte0003").intValue;
            utf8Bytes[18] = (byte) byte14Prop.FindPropertyRelative("byte0004").intValue;
            utf8Bytes[19] = (byte) byte14Prop.FindPropertyRelative("byte0005").intValue;
            utf8Bytes[20] = (byte) byte14Prop.FindPropertyRelative("byte0006").intValue;
            utf8Bytes[21] = (byte) byte14Prop.FindPropertyRelative("byte0007").intValue;
            utf8Bytes[22] = (byte) byte14Prop.FindPropertyRelative("byte0008").intValue;
            utf8Bytes[23] = (byte) byte14Prop.FindPropertyRelative("byte0009").intValue;
            utf8Bytes[24] = (byte) byte14Prop.FindPropertyRelative("byte0010").intValue;
            utf8Bytes[25] = (byte) byte14Prop.FindPropertyRelative("byte0011").intValue;
            utf8Bytes[26] = (byte) byte14Prop.FindPropertyRelative("byte0012").intValue;
            utf8Bytes[27] = (byte) byte14Prop.FindPropertyRelative("byte0013").intValue;
            utf8Bytes[28] = (byte) byte14Prop.FindPropertyRelative("byte0014").intValue;
            utf8Bytes[29] = (byte) byte14Prop.FindPropertyRelative("byte0015").intValue;
        }
        catch {}

        return System.Text.Encoding.UTF8.GetString(utf8Bytes);
    }
}
#endif