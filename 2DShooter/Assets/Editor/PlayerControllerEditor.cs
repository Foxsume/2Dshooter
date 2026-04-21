using UnityEditor;
using UnityEngine;

// This editor adds debugging tools to test gameplay without input and enforces valid values

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerController controller = (PlayerController)target;
        PlayerState state = controller.GetComponent<PlayerState>();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Actions", EditorStyles.boldLabel);

        if (GUILayout.Button("Force Jump"))
        {
            state.isJumping = true;
        }

        if (GUILayout.Button("Force Dash"))
        {
            state.isDashing = true;
        }

        if (GUILayout.Button("Toggle Crouch"))
        {
            state.isCrouched = !state.isCrouched;
        }

        if (GUILayout.Button("Reset Dash Cooldown"))
        {
            var field = typeof(PlayerController).GetField("dashCooldownTime",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            field.SetValue(controller, 0f);
        }
    }
}
