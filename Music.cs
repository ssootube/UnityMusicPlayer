using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Music
{
    private List<Track> tracks;
    public void SetTracks(List<Track> l) { tracks = l; }
    public List<Track> GetTracks() { return (tracks); }
}

public class Track
{
    private Notes[] notes;
    private SoundFont soundFont = new SoundFont();
    public void SetNotes(Notes[] var) { notes = var; }
    public void SetSoundFont(Pair<string, int>[] srcs) {
        for (int i = 0; i < srcs.Length; ++i)
        {
            Debug.Log(srcs[i].Second);
            soundFont.importSrc(srcs[i].First, srcs[i].Second);
        }
    }
    public Notes[] GetNotes() { return (notes); }
    public SoundFont GetSoundFont() { return (soundFont); }
    public void initSoundFont()
    {
        soundFont.init();
    }
}

public class Notes
{
    private int[] pitch;
    public void SetPitch(int[] arr) { pitch = arr; }
    public int[] GetPitch() { return (pitch); }
}

public class SoundSample
{
    public AudioClip src;
    public int pitch;
    public SoundSample(AudioClip src, int pitch)
    {
        this.src = src;
        this.pitch = pitch;
    }
}

public class SoundFont
{
    public List<SoundSample> original = new List<SoundSample>();
    public List<Pair<int, float>> shifted;
    public void init()
    {
        shifted = new List<Pair<int, float>>();
        shifted.Add(new Pair<int, float>());
        original.Sort((x1, x2) => x1.pitch.CompareTo(x2.pitch));
        int start = 1;
        for (int i = 0; i < original.Count - 1; ++i)
        {
            int end = original[i].pitch;
            end += (original[i + 1].pitch - original[i].pitch) / 2;
            for (int j = start; j <= end; ++j)
                shifted.Add(new Pair<int, float>(i, pitchCalculator(original[i].pitch, j)));
            start = end + 1;
        }
        for (int j = start; j <= (int)NT.B7; ++j)
            shifted.Add(new Pair<int, float>(original.Count - 1, pitchCalculator(original[original.Count - 1].pitch, j)));
    }
    public void importSrc(string name, int pitch)
    {
        original.Add(new SoundSample((AudioClip)Resources.Load(name), pitch));
    }
    public void play(AudioSource player, int pitch)
    {
        if (pitch != (int)NT.EMPTY)
        {
            player.clip = original[shifted[pitch].First].src;
            player.pitch = shifted[pitch].Second;
            player.Play();
        }
    }
    float pitchCalculator(int from, int to)
    {
        double result = 1;
        if (from < to)
        {
            int q = (to - from) / 12;
            result = Math.Pow(2, q) * Math.Pow(1.059463, (to - from) % 12);
        }
        else if (from > to)
        {
            int q = (from - to) / 12;
            result = Math.Pow(1 / 2, q) / Math.Pow(1.059463, (from - to) % 12);
        }
        else if (from == to)
            result = 1;
        return (float)result;
    }
}