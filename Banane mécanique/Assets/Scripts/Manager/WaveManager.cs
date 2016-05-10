using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

public class WaveController
{
    public List<Wave> waves;
    public List<float> timers;

    public bool isToggledTimers = false;
    public bool isToggledWave = false;

    public string fileName = "";

    public WaveController()
    {
        waves = new List<Wave>();
        timers = new List<float>();
    }

    public void AddWave()
    {
        Wave wave = new Wave();
        wave.units = new List<Unit>();
        waves.Add(wave);
    }

    public void RemoveWave(int index)
    {
        if (waves.Count > index)
        {
            waves.RemoveAt(index);
        }
    }

    public void AddTimer(float t = 0.0f)
    {
        timers.Add(t);
    }

    public void RemoveTimer(int index)
    {
        if (timers.Count > index)
        {
            timers.RemoveAt(index);
        }
    }

    public WaveController Load()
    {
        WaveController wave;
        Load(fileName, out wave);
        return wave;
    }

    public static void Load(string fileName, out WaveController waveEdit)
    {
        waveEdit = null;
        if (File.Exists(Application.persistentDataPath + "/WaveUnits/" + fileName + ".wave"))
        {
            JSONClass json = JSONNode.LoadFromFile(Application.persistentDataPath + "/WaveUnits/" + fileName + ".wave").AsObject;

            waveEdit = new WaveController();

            waveEdit.fileName = fileName;

            waveEdit.waves = new List<Wave>();
            waveEdit.timers = new List<float>();


            for (int i = 0; i < json["timers"].Count; i++)
            {
                waveEdit.timers.Add(json["timers"][i].AsFloat);
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
                    if (nodeChild["entity"] != null)
                    {
                        unit.entity = Resources.Load("Prefabs/" + nodeChild["entity"]) as GameObject;
                    }
                    unit.spawnPoint.x = nodeChild["x"].AsFloat;
                    unit.spawnPoint.y = nodeChild["y"].AsFloat;
                    unit.spawnPoint.z = nodeChild["z"].AsFloat;
                    wave.units.Add(unit);
                }
                wave.timeDelay = jsonNodeOne["time"].AsFloat;

                waveEdit.waves.Add(wave);
            }

            waveEdit.isToggledTimers = json["toggleTimer"].AsBool;
            waveEdit.isToggledWave = json["toggleWave"].AsBool;
        }
        else
        {
            return;
        }
    }

    public void Save()
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
                if (unit.entity)
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
    }
}

public class Unit
{
    public GameObject entity;
    public Vector3 spawnPoint;
}

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
        if (units.Count > index)
        {
            units.RemoveAt(index);
        }
    }
}

public class WaveManager : MonoBehaviour {
    

    public List<string> waveNames;
    List<WaveController> waves;
    public bool isSpawning = true;
    public float timeDelayBetweenMajorWaves = 2.0f;


    void Start () {
        waves = new List<WaveController>();
        WaveController wave;
        for (int i = 0; i < waveNames.Count; i++)
        {
            WaveController.Load(waveNames[i], out wave);
            if (wave != null)
            {
                waves.Add(wave);
            }
        }
        StartCoroutine(Spawn());
	}

    IEnumerator Spawn()
    {
        while(isSpawning)
        {

            for (int i = 0; i < waves.Count; i++)
            {
                WaveController wave = waves[i];

                for (int j = 0; j < wave.timers.Count; j++)
                {
                    if(wave.timers[j] < 0.0f)
                    {
                        for (int k = 0; k < wave.waves.Count; k++)
                        {
                            Wave wav = wave.waves[k];
                            for (int l = 0; l < wav.units.Count; l++)
                            {
                                Instantiate(wav.units[l].entity, wav.units[l].spawnPoint, wav.units[l].entity.transform.rotation);
                                yield return new WaitForSeconds(wav.timeDelay);
                            }
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(wave.timers[j]);
                    }
                }
            }

            yield return new WaitForSeconds(timeDelayBetweenMajorWaves);
        }
    }
}
