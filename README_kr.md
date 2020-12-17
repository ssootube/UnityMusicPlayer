# UnityMusicPlayer
작곡된 간단한 노래를 연주하는 Unity Player입니다.

이 것은 미디 파일 플레이어가 아닙니다. 미디 파일의 구조가 어려웠기 때문에 단순화 하고자 만들었습니다.
그래서 비록 매우 제한적이지만 최소한 간단한 음악을 연주할 수 있는 플레이어를 만들었습니다.
이 플레이어를 사용하려면 먼저 사운드 소스 파일이 필요합니다.
사운드 소스 파일은 딱 한 개의 음만을 연주하는 짧은 길이의 악기 샘플 wav 파일을 의미합니다.
(예를 들어 기타의 5번줄을 튕기는 소리가 녹음된 0.6초 길이의 wav 파일)
사운드 파일을 직접 구해서 리소스 폴더에 넣어두셔야 합니다.

사용법은 아래와 같습니다.

# 1.사운드 소스 파일을 직접 구하세요
저작권 문제가 있을 수 있기 때문에, 스스로 소스 파일을 구해서 resources 폴더 아래에 두셔야 합니다.


# 2.cs 파일들을 import 하세요
아래 세 개의 파일을 프로젝트 에셋으로 포함시키세요.
-MusicPlayer.cs
-Pair.cs
-Music.cs

# 3.게임 오브젝트를 생성하세요
음악을 연주하고 싶은 씬에 빈 게임 오브젝트를 생성하고, MusicPlayer.cs 파일을 컴포넌트로 붙이세요.

# 4. 위에서 붙인 MusicPlayer 컴포넌트를 가져와서 음악을 재생하세요.

        MusicPlayer m = gameObject.GetComponent<MusicPlayer>();
        Music music = new Music();//재생할 음악의 정보가 담긴 Music 클래스의 구성방법에 대해서는 아래에서 더욱 자세히 설명합니다.
        m.init(music);
        m.play();
        
# 5. Music 클래스는 아래와 같은 방법으로 사용할 수 있습니다.
## (1)Notes를 먼저 만드세요
  unityMusicPlayer는 0.5초마다 노트를 재생합니다. 노트는 동시에 연주하고 싶은 음을 의미합니다.
  
  Notes CM = new Notes();
  CM.SetPitch(new int[]{(int)NT.C2,(int)NT.E2,(int)NT.G2});
  
  위의 노트는 도, 미, 솔을 동시에 연주하므로 C 메이저 코드를 의미합니다.
  
  NT enum 타입은 아래와 같이 정의되어 있습니다.
  
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
  
## (2)여러 개의 노트를 만들었다면, 트랙을 만드세요.
  여러 개의 트랙이 있을 수도 있습니다. 이는 서로 다른 트랙이 다른 악기를 이용해 같은 노트를 연주하게 하기 위함입니다.
  예시로는 한 개의 트랙만을 만들어보겠습니다.
  
         Track track = new Track();
         track.SetNotes(new Notes[] {CM,E,E,E,F,F,E,CM,E,E,F,CM});
  
  위에 보이는 트랙은  {CM,E,E,E,F,F,E,CM,E,E,F,CM} 라는 노트를 0.5초 간격으로 차례대로 연주하겠다는 의미입니다.
  위에서 사용된 노트들은 아래와 같이 정의할 수 있습니다.
  
         Notes CM = new Notes();
         CM.SetPitch(new int[]{(int)NT.C2,(int)NT.E2,(int)NT.G2});
         Notes E = new Notes();
          E.SetPitch(new int[] {(int)NT.E2});
          Notes F = new Notes();
        F.SetPitch(new int[] { (int)NT.F2 });
  
## (3)트랙은 연주할 악기 정보또한 포함하고 있어야 합니다.
  우리는 이 정보를 사운드 폰트라고 부르기로 합니다.
  사운드 폰트 클래스의 구조는 간단하나, 주의해야할 점이 있습니다.
  사운드 폰트는 작은 양의 음원 소스들의 음높이를 조절하여 다양한 음높이의 소리를 만들어 냅니다.
  하지만 원본파일에 비해 너무 많이 음이 높아지거나 할 경우 음의 길이에 영향을 미칩니다.
  예를 들어 원본 파일의 음은 '도'인데 이 파일의 pitch를 더 높게 변경하여 '시'를 낼 경우, 음이 굉장히 짧아집니다.
   그래서 음높이 별로 서로 다른 여러 개의 음원 소스를 사용하는 것이 더 좋습니다.
   모든 음마다 전부 다른 음원 소스를 사용할 필요까지는 없고, 한 개의 음원소스가 6개 정도의 음을 담당하도록 하면 좋습니다.
   C1 부터 B7까지 84개 정도의 음이 있으므로 13~14개 정도의 음원파일을 사용하면 적당합니다.
   이러한 음원파일을 load하면 사운드 폰트 클래스가 알아서 자동으로 특정 pitch의 음을 어떤 소스파일을 변형하여 재생할 것인지 결정해줍니다.
   여러분은 등록만 하면 됩니다.
   예를 들어  Resources/C.wav 파일과 Resources/Ab2.wav 파일을 등록하려 합니다.
    Resources/C.wav의 원본음은 C2이고 Resources/Ab2.wav 는 Abs입니다.
   
    track.SetSoundFont(new Pair<string, int>[] { 
            new Pair<string,int>("C",(int)NT.C2),
            new Pair<string, int>("Ab2",(int)NT.Ab2)});
    
   위 처럼 등록을 하고 나면, 이 두 개의 소스파일을 이용해 84개의 음을 최대한 자연스럽게 재생할 것입니다.
   하지만 단 한개의 음원만을 등록해서 사용하려고 하면 오류가 발생할 것입니다.
   적어도 2개의 음원파일이 필요합니다.
 ## (4)트랙을 리스트에 담으세요
 트랙은 여러개가 있을 수 있기 때문에 리스트에 담아야 합니다. 단 한개의 트랙을 연주할 계획이라도, 리스트에 담아야 합니다.
 
         List<Track> tracks = new List<Track>();
         tracks.Add(track);
  
## (5)트랙을 Music클래스에 넣으세요
        Music music = new Music();
        music.SetTracks(tracks);
        
        Music클래스는 단순히 트랙 리스트 만을 멤버 변수로 가지고 있습니다.
        이는 여러분이 Music 클래스를 수정하여 다양한 정보를 추가적으로 넣게 하기 위함입니다.
        예를 들어 string musicTitle 같은 정보를 넣을 수도 있겠습니다.
