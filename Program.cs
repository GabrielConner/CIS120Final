using System.Diagnostics;
using SFML.Audio;

class Program {

   public const string mapSaveLoc = "./maps/map.KEYMAP";
   public const string playerSaveLoc = "./saves/save.KEYGAMESAVE";
   public const string playerSaveFolder = "./saves/";
   public const string mapSaveFolder = "./maps/";
   public const string beepLoc = "./beep.wav";
   public const string wonSoundLoc = "./wonSound.wav";
   public const string logLoc = "./log.txt";

   static public Player curPlayer = new Player();
   static public cMap curMap = new cMap();

   public const int APS = 5;
   public const float timeBetweenActions = 1000 / APS;

   static void Main() {
      try {
         if (!Directory.Exists(mapSaveFolder)) {
            Directory.CreateDirectory(mapSaveFolder);
            return;
         } else if (File.Exists(mapSaveLoc)) {
            curMap.loadMapFromFile();
         } else
            return;


         if (!Directory.Exists(playerSaveFolder))
            Directory.CreateDirectory(playerSaveFolder);
         else if (File.Exists(playerSaveLoc))
            curPlayer.loadPlayerFromFile();



         Stopwatch clock = new Stopwatch();
         clock.Start();
         //Main loop
         while (true) {
            Console.Clear();
            Console.WriteLine("Press 'e' to exit\nWASD to move\n");
            curMap.showMap();
            while (clock.ElapsedMilliseconds < timeBetweenActions) { }

            clock.Reset();

            //Put actions below here

            if (!update())
               break;

            clearConsoleInputBuffer();
            clock.Start();
         }

         curPlayer.savePlayerToFile();

      } catch (IOException IOex) {
         logError(IOex.Message);
      } catch (Exception ex) {
         logError(ex.Message);
      } finally {
         AudioController.clearSounds();
      }

      Console.Clear();
   }

   static bool update() {
      char input = Console.ReadKey().KeyChar;
      if (input != 0) {
         input = char.ToLower(input);
         if (input == 'e')
            return false;

         switch (input) {
            case 'w':
               curPlayer.PosY -= 1;
               curPlayer.Looking = playerDirection.up;
               break;
            case 'a':
               curPlayer.PosX -= 1;
               curPlayer.Looking = playerDirection.left;
               break;
            case 's':
               curPlayer.PosY += 1;
               curPlayer.Looking = playerDirection.down;
               break;
            case 'd':
               curPlayer.PosX += 1;
               curPlayer.Looking = playerDirection.right;
               break;
         }
      }

      for (int i = 0; i < curMap.Keys.Count; i++) {
         if (curMap.Keys[i].Position == curPlayer.Position) {
            if (!curMap.Keys[i].Alive)
               continue;

            curMap.Keys[i].Alive = false;
            curPlayer.keysCollected.Add(i);

            if (curPlayer.keysCollected.Count < curMap.Keys.Count)
               collectedKey();
            else
               won();
         }
      }

      return true;
   }

   static void collectedKey() {
      AudioController.addSound(beepLoc);
   }

   static bool playedWonSound = false;
   static void won() {
      if (!playedWonSound) {
         AudioController.addSound(wonSoundLoc);
         playedWonSound = true;
      }
   }

   static void clearConsoleInputBuffer() {
      while (Console.KeyAvailable) {
         Console.ReadKey(false);
      }
   }

   public static void logError(string er) {
      StreamWriter sw = new StreamWriter(logLoc, append: true);

      sw.WriteLine($"{DateTime.Now:g}\tError : {er}");

      sw.Flush();
      sw.Close();
   }
}