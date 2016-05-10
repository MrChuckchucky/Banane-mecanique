using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaveEditor : EditorWindow
{
    protected static WaveEditor window;

    public WaveController wave;

}

public class WaveEditorCreate : WaveEditor
{
    [MenuItem("Wave/Create")]
    static void CreateInit()
    {
        window = (WaveEditorCreate)EditorWindow.GetWindow(typeof(WaveEditorCreate));
        window.wave = new WaveController();
        window.Show();
    }

    public static void CreateInit(WaveEditor editor)
    {
        CreateInit();
        window.wave = new WaveController();
        window.wave.fileName = editor.wave.fileName;
        window.wave.waves = editor.wave.waves;
        window.wave.timers = editor.wave.timers;
        window.wave.isToggledTimers = editor.wave.isToggledTimers;
        window.wave.isToggledWave = editor.wave.isToggledWave;
    }

    void OnGUI()
    {
        wave.fileName = EditorGUILayout.TextField("File Name", wave.fileName);

        if (GUILayout.Button("Save") && wave.fileName != "")
        {
            wave.Save();
        }

        wave.isToggledTimers = EditorGUILayout.BeginToggleGroup("Edit Timers", wave.isToggledTimers);
        EditorGUILayout.BeginFadeGroup(wave.isToggledTimers ? 1 : 0);

        if(wave.isToggledTimers)
        {
            int waveIterator = 0;

            for (int i = 0; i < wave.timers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (wave.timers[i] < -float.Epsilon)
                {
                    if (wave.waves.Count > waveIterator && wave.waves[waveIterator] != null)
                    {
                        EditorGUILayout.LabelField("Wave n°" + (waveIterator + 1));
                    }
                    if (GUILayout.Button("-"))
                    {
                        wave.RemoveWave(waveIterator);
                        wave.RemoveTimer(i);
                    }
                    waveIterator++;
                }
                else
                {
                    wave.timers[i] = EditorGUILayout.FloatField(wave.timers[i]);

                    if (GUILayout.Button("-"))
                    {
                        wave.RemoveTimer(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.LabelField("Add Values : ");

        if (GUILayout.Button("Add Timer"))
        {
            wave.AddTimer();
        }

        if (GUILayout.Button("Add Wave"))
        {
            wave.AddTimer(-5.0f);
            wave.AddWave();
        }

        wave.isToggledWave = EditorGUILayout.BeginToggleGroup("Edit Waves", wave.isToggledWave);
        EditorGUILayout.BeginFadeGroup(wave.isToggledWave ? 1 : 0);

        if(wave.isToggledWave)
        {
            for (int i = 0; i < wave.waves.Count; i++)
            {
                EditorGUILayout.LabelField("Wave n°" + (i + 1));
                Wave w = wave.waves[i];
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Timer entre chaque ennemi");
                w.timeDelay = EditorGUILayout.FloatField(w.timeDelay);
                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < w.units.Count; j++)
                {
                    Unit unit = w.units[j];
                    EditorGUILayout.BeginHorizontal();
                    unit.entity = EditorGUILayout.ObjectField(unit.entity, typeof(GameObject), false) as GameObject;

                    GUI.enabled = false;
                    unit.spawnPoint = EditorGUILayout.Vector3Field("", unit.spawnPoint);
                    GUI.enabled = true;

                    if (GUILayout.Button("Set Spawn Point"))
                    {
                        unit.spawnPoint = Selection.activeGameObject.transform.position;
                    }

                    if (GUILayout.Button("-"))
                    {
                        w.RemoveUnit(j);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Unit"))
                {
                    w.AddUnit();
                }
                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.EndToggleGroup();
    }
}

public class WaveEditorLoad : WaveEditor
{
    [MenuItem("Wave/Load")]
    static void LoadInit()
    {
        window = (WaveEditorLoad)EditorWindow.GetWindow(typeof(WaveEditorLoad));
        window.wave = new WaveController();
        window.Show();
    }

    void OnGUI()
    {
        wave.fileName = EditorGUILayout.TextField("File Name", wave.fileName);

        if (GUILayout.Button("Load") && wave.fileName != "")
        {
             wave = wave.Load();
            WaveEditorCreate.CreateInit(this);
        }
    }
}
