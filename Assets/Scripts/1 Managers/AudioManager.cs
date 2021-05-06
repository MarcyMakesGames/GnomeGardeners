using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool debug;
    public AudioTrack[] tracks;

    // note: relationship between audio types (key) and audio tracks (value)
    private Hashtable audioTable;
    // note: relationship between audio types (key) and audio jobs (value) (Coroutine, IEnumerator)
    private Hashtable jobTable;

    #region Audio Classes

    [System.Serializable]
    public class AudioObject
    {
        public AudioType type;
        public AudioClip clip;
    }

    [System.Serializable]
    public class AudioTrack
    {
        public string name;
        public AudioSource source;
        public AudioObject[] audio;
    }

    private class AudioJob
    {
        public AudioAction action;
        public AudioType type;
        public bool fade;
        public WaitForSeconds delay;

        public AudioJob(AudioAction action, AudioType type, bool fade, float delay)
        {
            this.action = action;
            this.type = type;
            this.fade = fade;
            this.delay = delay > 0f ? new WaitForSeconds(delay) : null;
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Configure();
    }

    private void OnDisable()
    {
        Dispose();
    }

    #endregion

    #region Public Methods

    public void PlayAudio(AudioType type, bool fade = false, float delay = 0f)
    {
        AddJob(new AudioJob(AudioAction.START, type, fade, delay));
    }

    public void StopAudio(AudioType type, bool fade = false, float delay = 0f)
    {
        AddJob(new AudioJob(AudioAction.STOP, type, fade, delay));
    }

    public void RestartAudio(AudioType type, bool fade = false, float delay = 0f)
    {
        AddJob(new AudioJob(AudioAction.RESTART, type, fade, delay));
    }


    #endregion

    #region Private Methods

    private void Configure()
    {

        if (GameManager.Instance.AudioManager == null)
        {
            GameManager.Instance.AudioManager = this;
            audioTable = new Hashtable();
            jobTable = new Hashtable();
            GenerateAudioTable();
        }
    }

    private void Dispose()
    {
        foreach(DictionaryEntry kvp in jobTable)
        {
            Coroutine job = (Coroutine)kvp.Value;
            StopCoroutine(job);
        }
    }


    private void AddJob(AudioJob job)
    {
        RemoveConflictingJobs(job.type);

        Coroutine runningJob = StartCoroutine(RunAudioJob(job));
        jobTable.Add(job.type, runningJob);
        Log("Starting job on [" + job.type + "] with operation: " + job.action);
    }

    private void RemoveJob(AudioType type)
    {
        if (!jobTable.ContainsKey(type))
        {
            Log("Trying to stop a job [" + type + "] that is not running.");
            return;
        }
        Coroutine runningJob = (Coroutine)jobTable[type];
        StopCoroutine(runningJob);
        jobTable.Remove(type);
    }

    private void RemoveConflictingJobs(AudioType type)
    {
        // note: cancel the job if one exists with the same type
        if (jobTable.ContainsKey(type))
        {
            RemoveJob(type);
        }

        // note: cancel jobs that share the same audio track
        AudioType conflictAudio = AudioType.None;
        AudioTrack audioTrackNeeded = GetAudioTrack(type, "Get Audio Track Needed");
        foreach(DictionaryEntry entry in jobTable)
        {
            AudioType audioType = (AudioType)entry.Key;
            AudioTrack audioTrackInUse = GetAudioTrack(audioType, "Get Audio Track In Use");
            if(audioTrackInUse.source == audioTrackNeeded.source)
            {
                conflictAudio = audioType;
                break;
            }
        }
        if(conflictAudio != AudioType.None)
        {
            RemoveJob(conflictAudio);
        }
    }

    private IEnumerator RunAudioJob(AudioJob job)
    {
        if (job.delay != null) yield return job.delay;

        // note: track existence should be verified by now
        AudioTrack track = GetAudioTrack(job.type);
        track.source.clip = GetAudioClipFromAudioTrack(job.type, track);

        float initial = 0f;
        float target = 1f;
        switch (job.action)
        {
            case AudioAction.START:
                track.source.Play();
                break;
            case AudioAction.STOP when !job.fade:
                track.source.Stop();
                break;
            case AudioAction.STOP:
                initial = 1f;
                target = 0f;
                break;
            case AudioAction.RESTART:
                track.source.Stop();
                track.source.Play();
                break;
        }

        if (job.fade)
        {
            float duration = 1f;
            float timer = 0f;

            while(timer <= duration)
            {
                track.source.volume = Mathf.Lerp(initial, target, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            // note: if timer was 0.999 and Time.deltaTime was 0.01 we would not have reached the target
            // note: make sure the volume is set to the value we want
            track.source.volume = target;

            if(job.action == AudioAction.STOP)
            {
                track.source.Stop();
            }
        }

        jobTable.Remove(job.type);
        Log("Job count: " + jobTable.Count);
    }

    private AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
    {
        foreach(AudioObject obj in track.audio)
        {
            if(obj.type == type)
            {
                return obj.clip;
            }
        }
        return null;
    }


    private void GenerateAudioTable()
    {
        foreach(AudioTrack track in tracks)
        {
            foreach(AudioObject obj in track.audio)
            {
                if (audioTable.ContainsKey(obj.type))
                {
                    LogWarning("You are trying to register audio [" + obj.type + "] that has already been registered.");
                }
                else
                {
                    audioTable.Add(obj.type, track);
                    Log("Registering audio [" + obj.type + "]");
                }
            }
        }
    }

    private AudioTrack GetAudioTrack(AudioType type, string job = "")
    {
        if (!audioTable.ContainsKey(type))
        {
            LogWarning("You are trying to <color=#fff>" + job + "</color> for [" + type + "] but no track was found supporting this audio type.");
            return null;
        }
        return (AudioTrack)audioTable[type];
    }

    private void Log(string msg)
    {
        if (!debug) return;
        Debug.Log("[AudioController]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) return;
        Debug.LogWarning("[AudioController]: " + msg);
    }

    #endregion

}
