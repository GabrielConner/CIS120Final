//Player directions and corresponding arrows
public enum PlayerDirection { up = '^', down = 'v', left = '<', right = '>' }

//Player class
class Player {
   public PlayerDirection looking = PlayerDirection.up;
   IVec2 inPosition = new IVec2(0);
   public List<int> keysCollected = new List<int>();

   //inPosition is private, by default
   
   //Getter and setter for x value
   public int posX {
      get => inPosition.x;
      set {
         if (value >= 0 && value < Program.curMap.mapSize)
            inPosition.x = value;
      }
   }

   //Getter and setter for y value
   public int posY {
      get => inPosition.y;
      set {
         if (value >= 0 && value < Program.curMap.mapSize)
            inPosition.y = value;
      }
   }

   //Getter for the position vector
   public IVec2 position {
      get => inPosition;
   }

   //Loads player from a file
   public void LoadPlayerFromFile() {
      StreamReader sr = new StreamReader(Program.playerSaveLoc);
      //If file doesn't exist, just set position to (0,0) and return out
      if (sr.Peek() == -1) {
         inPosition = new IVec2(0);
         sr.Close();
         return;
      }
      //Attempt to parse position
      //Set position to (0,0) if it fails
      if (!IVec2.TryParse(sr.ReadLine() ?? "", out inPosition))
         inPosition = new IVec2(0);

      //All the keys are stored on a new line with their index seperated by commas
      //Empty means this Won't do anything
      string[] sKeysCollected = (sr.ReadLine() ?? "").Split(',', StringSplitOptions.TrimEntries);
      foreach (string str in sKeysCollected) {
         int keyIndex;
         if (Int32.TryParse(str, out keyIndex) && keyIndex < Program.curMap.keys.Count()) {
            //If any indices are duplicated, this makes sure they don't continue to duplicate
            if (!Program.curMap.keys[keyIndex].alive)
               continue;
            Program.curMap.keys[keyIndex].alive = false;
            keysCollected.Add(keyIndex);
         }
      }

      sr.Close();
   }

   //Saves player to a file
   //First line is position
   //Secound line is keys
   public void SavePlayerToFile() {
      StreamWriter sr = new StreamWriter(Program.playerSaveLoc);

      sr.WriteLine(position.ToString());
      for (int i = 0; i < keysCollected.Count; i++) {
         if (i != 0)
            sr.Write(',');
         sr.Write(keysCollected[i].ToString());
      }
      sr.WriteLine();


      sr.Flush();
      sr.Close();
   }
};
