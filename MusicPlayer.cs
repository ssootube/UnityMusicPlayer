using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NT
{
    EMPTY,
    C1, Db1, D1, Eb1, E1, F1, Gb1, G1, Ab1, A1, Bb1, B1,
    C2, Db2, D2, Eb2, E2, F2, Gb2, G2, Ab2, A2, Bb2, B2,
    C3, Db3, D3, Eb3, E3, F3, Gb3, G3, Ab3, A3, Bb3, B3,
    C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4,
    C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5,
    C6, Db6, D6, Eb6, E6, F6, Gb6, G6, Ab6, A6, Bb6, B6,
    C7, Db7, D7, Eb7, E7, F7, Gb7, G7, Ab7, A7, Bb7, B7,
    Length
}

public class MusicPlayer : MonoBehaviour
{
    private Music music;
    private int trackCount;
    private float tickTime = 0.5f;
    private float musicLength;
    private IEnumerator player;
    public List<GameObject> musicPlayers;

    public void init(Music _music)
    {
        this.music = _music;
        trackCount = music.GetTracks().Count;
        musicLength = music.GetTracks()[0].GetNotes().Length;
        for (int i = 0; i < trackCount; ++i)
        {
            GameObject temp = new GameObject("MusicPlayer");
            musicPlayers.Add(temp);
            for (int j = 0; j < (int)NT.Length - 1; ++j)
                temp.AddComponent<AudioSource>();
            music.GetTracks()[i].initSoundFont();
        }
        player = tick();
    }
    public void play()
    {
        StartCoroutine(player);
    }
    public void stop()
    {
        StopCoroutine(player);
    }
    public void restart()
    {
        StopCoroutine(player);
        player = tick();
        StartCoroutine(player);
    }
    IEnumerator tick()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(tickTime);
        for (int i = 0; i < musicLength; ++i)
        {
            for (int j = 0; j < trackCount; ++j)
            {
                Track temp = music.GetTracks()[j];
                int[] pitch = temp.GetNotes()[i].GetPitch();
                for(int k = 0; k < pitch.Length; ++k)
                {
                    AudioSource[] players = musicPlayers[j].GetComponentsInChildren<AudioSource>();
                    temp.GetSoundFont().play(players[pitch[k]], pitch[k]);
                }
            }
            yield return waitForSeconds;
        }
    }
}
