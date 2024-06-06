using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildReportLogger : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("Build Summary:");
        Debug.Log("Build Result: " + report.summary.result);
        Debug.Log("Total Size: " + report.summary.totalSize + " bytes");

        foreach (var step in report.steps)
        {
            Debug.Log("Step: " + step.name);
            foreach (var message in step.messages)
            {
                Debug.Log(message.content);
            }
        }

        foreach (var file in report.files)
        {
            Debug.Log(file.path + ": " + file.size + " bytes");
        }
    }
}
