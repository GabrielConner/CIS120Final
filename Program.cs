/*
 * Pascal case for classes and functions
 * Camel case for variables
 */


using System.Diagnostics;
using SFML.Audio;

class Program {

   //File and folder locations
   public const string mapSaveFolder = "./maps/";
   public const string mapSaveLoc = "./maps/map.KEYMAP";
   public const string playerSaveFolder = "./saves/";
   public const string playerSaveLoc = "./saves/save.KEYGAMESAVE";
   public const string beepLoc = "./beep.wav";
   public const string wonSoundLoc = "./wonSound.wav";
   public const string logLoc = "./log.txt";

   //map and player
   static public Player curPlayer = new Player();
   static public CMap curMap = new CMap();

   //Actions per second controlling
   public const int APS = 5;
   public const float timeBetweenActions = 1000 / APS;

   //Returns 0 if an error happened and 1 if everything ran fine
   static int Main() {
      //Wrap everything in try-catch-finally to allow proper garbage collection in case of crash
      try {
         //map has to exist
         if (!Directory.Exists(mapSaveFolder)) {
            Directory.CreateDirectory(mapSaveFolder);
            throw new IOException("No map folder");
         } else if (File.Exists(mapSaveLoc)) {
            curMap.LoadMapFromFile();
         } else {
            throw new IOException("No map file");
         }

         //save file doesn't have to exist
         if (!Directory.Exists(playerSaveFolder))
            Directory.CreateDirectory(playerSaveFolder);
         else if (File.Exists(playerSaveLoc))
            curPlayer.LoadPlayerFromFile();


         //Use Stopwatch to track elapsed time
         Stopwatch clock = new Stopwatch();
         clock.Start();

         //Main loop
         while (true) {

            //Show map
            Console.Clear();
            Console.WriteLine("Press 'e' to exit\nWASD to move\n");
            curMap.ShowMap();

            //Pause until minimum time has elapsed
            while (clock.ElapsedMilliseconds < timeBetweenActions) { }

            //Reset then start clock so that time taken doing stuff is included in counted time
            clock.Reset();
            clock.Start();

            //Update loop
            //Runs until it returns false
            if (!Update())
               break;

            //Prevent buffering of inputs
            //
            //Causes player to get stuck going in a direction for a while if
            //direction button is held and APS is low
            ClearConsoleInputBuffer();
         }

         //Save player data to file
         curPlayer.SavePlayerToFile();
         return 1;
      }

      //Catch all exceptions then write to a log file
      catch (Exception ex) {
         LogError(ex.Message);
         return 0;
      }

      //Runs regardless of if there was an exception or not
      finally {
         Console.Clear();
         AudioController.ClearSounds();
      }
   }

   static bool Update() {
      //Read the key input and get the character that matches it
      //Waits until key is pressed to continue from here
      char input = Console.ReadKey().KeyChar;
      
      if (input != 0) {
         input = char.ToLower(input);
         if (input == 'e')
            //Exit on 'e' input
            return false;

         //Only WASD input
         switch (input) {
            case 'w':
               curPlayer.posY -= 1;
               curPlayer.looking = PlayerDirection.up;
               break;
            case 'a':
               curPlayer.posX -= 1;
               curPlayer.looking = PlayerDirection.left;
               break;
            case 's':
               curPlayer.posY += 1;
               curPlayer.looking = PlayerDirection.down;
               break;
            case 'd':
               curPlayer.posX += 1;
               curPlayer.looking = PlayerDirection.right;
               break;
         }
      }

      //Check if the player is on top of a key
      for (int i = 0; i < curMap.keys.Count; i++) {
         if (curMap.keys[i].position == curPlayer.position) {
            //Make sure it isn't collected yet
            if (!curMap.keys[i].alive)
               continue;

            curMap.keys[i].alive = false;
            curPlayer.keysCollected.Add(i);

            //Play sounds on collecting key
            //Beep if it is normal key
            //Small tune if it is the last key
            if (curPlayer.keysCollected.Count < curMap.keys.Count)
               CollectedKey();
            else
               Won();
         }
      }

      //Continue looping
      return true;
   }

   //Probably don't need 'CollectedKey' or 'Won' functions

   //Play beep sound
   static void CollectedKey() {
      AudioController.AddSound(beepLoc);
   }

   //Play winning sound
   static void Won() {
      AudioController.AddSound(wonSoundLoc);
   }
   
   //Clears console input buffer
   static void ClearConsoleInputBuffer() {
      while (Console.KeyAvailable) {
         Console.ReadKey(false);
      }
   }

   //Logs error in log.txt
   public static void LogError(string er) {
      StreamWriter sw = new StreamWriter(logLoc, append: true);

      sw.WriteLine($"{DateTime.Now:g}\tError : {er}");

      sw.Flush();
      sw.Close();
   }
}