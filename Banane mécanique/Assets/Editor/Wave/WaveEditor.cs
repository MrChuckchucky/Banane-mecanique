using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SimpleJSON;
using System;
using System.IO;

public class WaveEditor : EditorWindow
{
    protected static WaveEditor window;

    public List<Wave> waves;
    public List<float> timers;

    public bool isToggledTimers = false;
    public bool isToggledWave = false;

    public string fileName = "";

    protected void AddWave()
    {
        Wave wave = new Wave();
        wave.units = new List<Unit>();
        waves.Add(wave);
    }

    protected void RemoveWave(int index)
    {
        if (waves.Count > index)
        {
            waves.RemoveAt(index);
        }
    }

    protected void AddTimer(float t = 0.0f)
    {
        timers.Add(t);
    }

    protected void RemoveTimer(int index)
    {
        if (timers.Count > index)
        {
            timers.RemoveAt(index);
        }
    }

    protected void Load()
    {
        waves = new List<Wave>();
        timers = new List<float>();

        JSONClass json = JSONNode.LoadFromFile(Application.persistentDataPath + "/WaveUnits/" + fileName + ".wave").AsObject;

        for (int i = 0; i < json["timers"].Count; i++)
        {
            timers.Add(json["timers"][i].AsFloat);
        }

        for (int i = 0; i < json["waves"].Count; i++)
        {
            Wave wave = new Wave();
            wave.units = new List<Unit>();
            JSONClass jsonNodeOne = json["waves"][i].AsObject;
            for (int j = 0; j < jsonNodeOne["unit"].Count; j++)
            {
                Unit unit = new Unit();
                JSONClass nodeChild = jsonNodeOne["unit"][j].AsObject;
                if(nodeChild["entity"] != null)
                {
                    unit.entity = Resources.Load("Prefabs/" + nodeChild["entity"]) as GameObject;
                }
                unit.spawnPoint.x = nodeChild["x"].AsFloat;
                unit.spawnPoint.y = nodeChild["y"].AsFloat;
                unit.spawnPoint.z = nodeChild["z"].AsFloat;
                wave.units.Add(unit);
            }
            wave.timeDelay = jsonNodeOne["time"].AsFloat;

            waves.Add(wave);
        }

        isToggledTimers = json["toggleTimer"].AsBool;
        isToggledWave = json["toggleWave"].AsBool;
        /*JSONNode j = JSONNode.LoadFromFile(Application.persistentDataPath + "/WaveUnits/" + fileName + ".wave");

        window = JsonUtility.FromJson(j.ToString(), typeof(WaveEditor)) as WaveEditor;*/
    }

    protected void Save()
    {
        JSONClass json = new JSONClass();

        for (int i = 0; i < timers.Count; i++)
        {
            json["timers"][i].AsFloat = timers[i];
        }

        for (int i = 0; i < waves.Count; i++)
        {
            Wave wave = waves[i];
            JSONClass jsonNodeOne = json["waves"][i].AsObject;
            for (int j = 0; j < wave.units.Count; j++)
            {
                Unit unit = wave.units[j];
                JSONClass nodeChild = jsonNodeOne["unit"][j].AsObject;
                if(unit.entity)
                {
                    nodeChild["entity"] = unit.entity.name;
                }
                else
                {
                    nodeChild["entity"] = "null";
                }
                nodeChild["x"].AsFloat = unit.spawnPoint.x;
                nodeChild["y"].AsFloat = unit.spawnPoint.y;
                nodeChild["z"].AsFloat = unit.spawnPoint.z;
            }
            jsonNodeOne["time"].AsFloat = wave.timeDelay;
        }

        json["toggleTimer"].AsBool = isToggledTimers;
        json["toggleWave"].AsBool = isToggledWave;

        Debug.Log(json.ToString());

        json.SaveToFile(Application.persistentDataPath + "/WaveUnits/" + fileName + ".wave");

        /*JSONData j = new JSONData(JsonUtility.ToJson(window));

        j.SaveToFile(Application.persistentDataPath + "/WaveUnits/" + fileName + ".wave");*/
    }
}

[Serializable]
public class Unit
{
    public GameObject entity;
    public Vector3 spawnPoint;
}

[Serializable]
public class Wave
{
    public List<Unit> units = new List<Unit>();
    public float timeDelay = 0.0f;

    public void AddUnit()
    {
        units.Add(new Unit());
    }

    public void RemoveUnit(int index)
    {
        if(units.Count > index)
        {
            units.RemoveAt(index);
        }
    }
}

public class WaveEditorCreate : WaveEditor
{
    [MenuItem("Wave/Create")]
    static void CreateInit()
    {
        window = (WaveEditorCreate)EditorWindow.GetWindow(typeof(WaveEditorCreate));
        window.waves = new List<Wave>();
        window.timers = new List<float>();
        window.Show();
    }

    public static void CreateInit(WaveEditor editor)
    {
        CreateInit();
        window.fileName = editor.fileName;
        window.waves = editor.waves;
        window.timers = editor.timers;
        window.isToggledTimers = editor.isToggledTimers;
        window.isToggledWave = editor.isToggledWave;
    }

    void OnGUI()
    {
        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("Save") && fileName != "")
        {
            Save();
        }

        isToggledTimers = EditorGUILayout.BeginToggleGroup("Edit Timers", isToggledTimers);
        EditorGUILayout.BeginFadeGroup(isToggledTimers ? 1 : 0);

        if(isToggledTimers)
        {
            int waveIterator = 0;

            for (int i = 0; i < timers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (timers[i] < -float.Epsilon)
                {
                    if (waves.Count > waveIterator && waves[waveIterator] != null)
                    {
                        EditorGUILayout.LabelField("Wave n°" + (waveIterator + 1));
                    }
                    if (GUILayout.Button("-"))
                    {
                        RemoveWave(waveIterator);
                        RemoveTimer(i);
                    }
                    waveIterator++;
                }
                else
                {
                    timers[i] = EditorGUILayout.FloatField(timers[i]);

                    if (GUILayout.Button("-"))
                    {
                        RemoveTimer(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.LabelField("Add Values : ");

        if (GUILayout.Button("Add Timer"))
        {
            AddTimer();
        }

        if (GUILayout.Button("Add Wave"))
        {
            AddTimer(-5.0f);
            AddWave();
        }

        isToggledWave = EditorGUILayout.BeginToggleGroup("Edit Waves", isToggledWave);
        EditorGUILayout.BeginFadeGroup(isToggledWave ? 1 : 0);

        if(isToggledWave)
        {
            for (int i = 0; i < waves.Count; i++)
            {
                EditorGUILayout.LabelField("Wave n°" + (i + 1));
                Wave w = waves[i];
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
        window.Show();
    }

    void OnGUI()
    {
        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("Load") && fileName != "")
        {
            Load();
            WaveEditorCreate.CreateInit(this);
        }
    }
}
