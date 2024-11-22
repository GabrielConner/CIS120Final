using SFML.Audio;

class JoinedSound {
   public Sound sound;
   public SoundBuffer? buffer;

   public JoinedSound(string soundLoc) {
      buffer = new SoundBuffer(soundLoc);
      sound = new Sound(buffer);
      sound.Play();
   }
   ~JoinedSound() {
      if (buffer == null)
         return;

      sound.Stop();
      sound.Dispose();
      buffer.Dispose();
   }
};

class AudioController {
   static List<JoinedSound> sounds = new List<JoinedSound>(255);

   public static void clearSounds() {
      foreach (JoinedSound sound in sounds) {
         if (sound.buffer == null)
            continue;

         sound.sound.Stop();
         sound.sound.Dispose();
         sound.buffer.Dispose();
      }
   }
   public static void addSound(string soundLoc) {
      sounds.ForEach(x => {
         if (x.sound.Status == SoundStatus.Stopped && x.buffer != null) {
            x.sound.Stop();
            x.sound.Dispose();
            x.buffer.Dispose();
            x.buffer = null;
         }
      });
      sounds.RemoveAll(x => x.buffer == null);
      if (sounds.Count < 255)
         sounds.Add(new JoinedSound(soundLoc));
   }
};