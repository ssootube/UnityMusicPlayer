# UnityMusicPlayer
Unity player which plays simple songs composed

It's not a midi file player.
I wanted to simplify the structure of the midi file because it was difficult.
So I made a player which can play at least simple music, although it is quite limited.
To operate this player, you first need an sound source file.
A sound source file means, for example, a 0.5 second-long piano sample wav file with the pitch of C2(or C#2, E2...etc).
The length of the file does not have to be 0.5 seconds.
Get your own sound files and put them in the Resources folder.

The instructions are as follows

# 1.get your own sound file
I don't provide the sound file because it may have a copyright problem. Get it yourself and put it in the resource folder.

# 2.import cs files
Include the three files below in your project asset.
-MusicPlayer.cs
-Pair.cs
-Music.cs

# 3.Create a game object.
Create an empty game object in the scene where you want to play music, and add MusicPlayer.cs as a component.

# 4. Bring the MusicPlayer component that was attached to the empty game object above, and then play Music.

        MusicPlayer m = gameObject.GoetComponent<MusicPlayer>();
        Music music = new Music();//The instructions for configuring the Music class are described below.
        m.init(music);
        m.play();
# 5. Music class can be used as below.
## (1)Create a note first.
  This player will play the music every 0.5 seconds. Notes means the sound that you want to play at once.
  
  Notes CM = new Notes();
  CM.SetPitch(new int[]{(int)NT.C2,(int)NT.E2,(int)NT.G2});
  
  The above note plays the C major code because it plays the do,mi,sol at once.
  
  NT is the enum type variable defined as follows:
  
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
  
## (2)Once you've made multiple notes, use them to create tracks.
  There can be multiple tracks, to play the same note using different instruments.
  First, I will make only one track.
  
         Track track = new Track();
         track.SetNotes(new Notes[] {CM,E,E,E,F,F,E,CM,E,E,F,CM});
  
  The above track plays {CM,E,E,E,F,F,E,CM,E,E,F,CM} in order at intervals of 0.5 seconds.
  The notes used at this time can be defined as follows:
  
         Notes CM = new Notes();
         CM.SetPitch(new int[]{(int)NT.C2,(int)NT.E2,(int)NT.G2});
         Notes E = new Notes();
          E.SetPitch(new int[] {(int)NT.E2});
          Notes F = new Notes();
        F.SetPitch(new int[] { (int)NT.F2 });
  
## (3)The track must contain information about which instruments to play.
   We load this information using a class called Sound Font.
   The structure of the sound font class is simple, but there are some things to be careful.
   The sound font plays music by controlling the pitch value with a small number of sound sources.
   But this will affect the length of the sound source. If the pitch is higher than the original sound, the length of that sound will also be shorter.
   Therefore, it is better to use different sources of music for high and low tones.
   Therefore, it works normally when you import at least two source files with different tones, and on average there are about a dozen sound sources that fall equally apart.
   In the example below, I would like to use the Resources/C.wav file and the Resources/Ab2.wav file.
   The Pitch in the Resources/C.wav file is C2, and the Pitch in the Resources/Ab2.wav file is Ab2.
   
    track.SetSoundFont(new Pair<string, int>[] { 
            new Pair<string,int>("C",(int)NT.C2),
            new Pair<string, int>("Ab2",(int)NT.Ab2)});
    
   When you add the source sound source, the sound font class automatically places the pitch in the most natural way.
   Of course it uses the closest pitch source.
   However, you cannot use only one source. Please put more than one in.
 ## (4)Put the track in the list.
 Since there can be multiple tracks, the tracks must be contained in the list object, even if you plan to play a single track.
 
         List<Track> tracks = new List<Track>();
         tracks.Add(track);
  
## (5)Put the track list object you have created into Music.
        Music music = new Music();
        music.SetTracks(tracks);

Music class simply have track list objects as member variables.
This is to allow you to modify the Music class to include various information.
For example, you can include a variable such as 'string musicTitle' as a member variable of Music.
