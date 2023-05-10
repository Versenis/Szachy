using UnityEditor;
using UnityEngine;

public class ChesboardGridEditor : EditorWindow
{
    GameObject _chessboard;
    GameObject _field1;
    GameObject _field2;

    [MenuItem("Window/Chessboard Grid")]
    static void ShowWindow()
    {
        GetWindow<ChesboardGridEditor>("Chessboard Grid");
    }

    [System.Obsolete]
    void OnGUI()
    {
        GUILayout.Label("GameObject \"Chessboard\" must be in the hierachy", EditorStyles.boldLabel);

        _chessboard = EditorGUILayoutObject("Chessboard", _chessboard);
        _field1 = EditorGUILayoutObject("Field 1", _field1);
        _field2 = EditorGUILayoutObject("Field 2", _field2);

        if (GUILayout.Button("Make a checkerboard grid"))
        {
            GameObject newFields;
            bool isField1 = true;

            float size = 1 / 8f;
            float x = -size * 3.5f;
            float y = x;

            float resetX = x;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isField1)   newFields = Instantiate(_field1);
                    else            newFields = Instantiate(_field2);

                    isField1 = !isField1;

                    newFields.transform.parent = _chessboard.transform;
                    newFields.transform.localPosition = new Vector2(x, y);
                    newFields.transform.localScale = new Vector2(size, size);

                    x += size;
                }

                isField1 = !isField1;
                x = resetX;
                y += size;
            }
        }
    }

    [System.Obsolete]
    GameObject EditorGUILayoutObject(string text, GameObject gameObject)
    {
        return (GameObject)EditorGUILayout.ObjectField(text, gameObject, typeof(GameObject));
    }
}
