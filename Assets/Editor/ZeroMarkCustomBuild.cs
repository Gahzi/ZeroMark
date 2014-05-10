using UnityEditor;
using System.Diagnostics;

public class ZeroMarkCustomBuild //: MonoBehaviour
{
    [MenuItem("MyTools/Windows Build With Postprocess")]
    public static void BuildGame()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

        // Get levels
        string map = "Assets/Scenes/zm_MAP_floatingCity_";
        string[] levels =
        {
            "Assets/Scenes/zm_SCENE_TitleScreen.unity",
            //map + "03.unity",
            //map + "04.unity",
            //map + "05.unity",
            //map + "06.unity",
            map + "07.unity",
        };

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);

        // Copy a file from the project folder to the build folder, alongside the built game.
        //FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");

        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = path + "BuiltGame.exe";
        proc.Start();
    }
}