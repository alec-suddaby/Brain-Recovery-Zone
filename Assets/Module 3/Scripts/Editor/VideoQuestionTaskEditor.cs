using UnityEditor;

[CustomEditor(typeof(VideoQuestionTask))]
public class VideoQuestionTaskEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VideoQuestionTask task = (VideoQuestionTask)target;
        task.UpdateQuestionPlayTimes();

        base.OnInspectorGUI();
    }
}
