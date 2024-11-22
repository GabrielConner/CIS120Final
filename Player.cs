public enum playerDirection { up = '^', down = 'v', left = '<', right = '>' }

class Player {
   public playerDirection Looking = playerDirection.up;
   IVec2 inPosition = new IVec2(0);
   public List<int> keysCollected = new List<int>();

   public int PosX {
      get => inPosition.x;
      set {
         if (value >= 0 && value < Program.curMap.mapSize)
            inPosition.x = value;
      }
   }

   public int PosY {
      get => inPosition.y;
      set {
         if (value >= 0 && value < Program.curMap.mapSize)
            inPosition.y = value;
      }
   }

   public IVec2 Position {
      get => inPosition;
   }

   public void loadPlayerFromFile() {
      StreamReader sr = new StreamReader(Program.playerSaveLoc);
      if (sr.Peek() == -1) {
         inPosition = new IVec2(0);
         sr.Close();
         return;
      }

      if (!IVec2.TryParse(sr.ReadLine() ?? "", out inPosition))
         inPosition = new IVec2(0);

      string[] sKeysCollected = (sr.ReadLine() ?? "").Split(',', StringSplitOptions.TrimEntries);
      foreach (string str in sKeysCollected) {
         int keyIndex;
         if (Int32.TryParse(str, out keyIndex) && keyIndex < Program.curMap.Keys.Count()) {
            if (!Program.curMap.Keys[keyIndex].Alive)
               continue;
            Program.curMap.Keys[keyIndex].Alive = false;
            keysCollected.Add(keyIndex);
         }
      }

      sr.Close();
   }

   public void savePlayerToFile() {
      StreamWriter sr = new StreamWriter(Program.playerSaveLoc);

      sr.WriteLine(Position.ToString());
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
