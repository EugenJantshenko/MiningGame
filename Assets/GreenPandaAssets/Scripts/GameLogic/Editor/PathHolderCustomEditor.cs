using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathHolder), true)]
public class PathHolderCustomEditor : Editor
{

    #region Private Members
    private const int ADD_BUTTON_WIDTH = 20;
    private const int FLOAT_FIELD_MIN_WIDTH = 20;
    private const int FLOAT_FIELD_MAX_WIDTH = 50;
    private PathHolder _pathHolder;
    #endregion

    #region Public Method
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        _pathHolder = (PathHolder) target;
        DrawElements();
    }
    #endregion

    #region Private Method

    private void DrawElements()
    {
        if (_pathHolder.Points != null)
        {

            for (int i = 0; i < _pathHolder.Points.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label(_pathHolder.Points[i].name);
                EditorGUILayout.BeginHorizontal();
                _pathHolder.Points[i].position = EditorGUILayout.Vector3Field("", _pathHolder.Points[i].position);

                if (GUILayout.Button("-", GUILayout.Width(ADD_BUTTON_WIDTH)))
                {
                    _pathHolder.RemovePoint(i);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("+", GUILayout.Width(ADD_BUTTON_WIDTH)))
            {
                _pathHolder.AddPoint();
            }
        }
        else
        {
            _pathHolder.InitializePoints();
        }

        
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.blue;

        if (_pathHolder != null && _pathHolder.Points != null && _pathHolder.Points.Count > 1)
        {
            for (int i = 0; i < _pathHolder.Points.Count - 1; i++)
            {
                Handles.DrawLine(_pathHolder.Points[i].transform.position, _pathHolder.Points[i + 1].transform.position);
            }
        }
    }

    private void ApplyPrefab()
    {
        /*try
        {
            UnityEngine.Object parrent = PrefabUtility.GetCorrespondingObjectFromSource(_pathHolder.gameObject);
            UnityEngine.Object prefab = _pathHolder.gameObject;

            if (null != parrent && null != prefab)
            {
                PrefabUtility.ReplacePrefab((GameObject) prefab, parrent, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
            }
        }
        catch (Exception e) { }*/
    }
    #endregion
}
