//Just used SFML for another project so I wanted to use it again
using SFML.Audio;

//Class to hold sound and sound buffer
class JoinedSound {
   public Sound sound;
   //sound buffer can be null
   public SoundBuffer? buffer;

   //Create sound and start playing
   public JoinedSound(string soundLoc) {
      buffer = new SoundBuffer(soundLoc);
      sound = new Sound(buffer);
      sound.Play();
   }
   //Prevents deleting if the buffer is already null
   //Means that it was never made in the first place, or was deleted previously
   ~JoinedSound() {
      if (buffer == null)
         return;

      sound.Stop();
      sound.Dispose();
      buffer.Dispose();
   }
};

//Class to hold static list of 'JoinedSound' and static functions
class AudioController {
   //SFML allows a max of 256 sounds, so set list buffer to that size
   static List<JoinedSound> sounds = new List<JoinedSound>(256);

   //Deletes all sounds
   public static void ClearSounds() {
      foreach (JoinedSound sound in sounds) {
         if (sound.buffer == null)
            continue;

         //Can't explicitly call deconstructor as it is controlled by the GC
         sound.sound.Stop();
         sound.sound.Dispose();
         sound.buffer.Dispose();
      }
   }

   //Adds sound to list
   //Doesn't allow if it would go past max
   public static void AddSound(string soundLoc) {
      //Didn't want to include sounds to Update loop
      //So it only removes completed sounds when you call to add a sound
      //
      //This just sets buffer to null to let the list know it is completed
      sounds.ForEach(x => {
         if (x.sound.Status == SoundStatus.Stopped && x.buffer != null) {
            x.sound.Stop();
            x.sound.Dispose();
            x.buffer.Dispose();
            x.buffer = null;
         }
      });

      //Removed here
      sounds.RemoveAll(x => x.buffer == null);
      if (sounds.Count < 256)
         sounds.Add(new JoinedSound(soundLoc));
   }
};