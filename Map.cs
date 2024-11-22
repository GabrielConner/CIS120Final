
class Key {
   public bool Alive;
   public IVec2 Position;

   public Key() {
      Alive = true;
      Position = new IVec2(0);
   }

   public Key(bool alive, IVec2 pos) {
      Alive = alive;
      Position = pos;
   }
};

class cMap {
   public char[,] Map;
   public List<Key> Keys;

   public int viewSize;
   public int mapSize;

   public cMap() {
      Keys = new List<Key>();
   }

   public cMap(int mapSize) {
      Map = new char[mapSize, mapSize];
      Keys = new List<Key>();
   }

   public void loadMapFromFile() {
      StreamReader sr = new StreamReader(Program.mapSaveLoc);

      if (sr.Peek() == -1)
         throw new IOException("No map save");

      string fLine = sr.ReadLine() ?? "";
      string[] mapVars = fLine.Split(',', StringSplitOptions.TrimEntries);

      if (mapVars.Count() == 2) {
         if (!Int32.TryParse(mapVars[0], out mapSize))
            mapSize = 5;
         if (!Int32.TryParse(mapVars[1], out viewSize))
            viewSize = 5;
      }

      Map = new char[mapSize, mapSize];

      bool fillRest = false;
      for (int y = 0; y < mapSize; y++) {
         string? line = sr.ReadLine();
         if (line == null)
            fillRest = true;
         for (int x = 0; x < mapSize; x++) {
            if (fillRest || x >= line.Count()) {
               Map[x, y] = ' ';
               continue;
            }

            if (line[x] == '*')
               Keys.Add(new Key(true, new IVec2(x, y)));

            Map[x, y] = line[x];
         }
      }
      sr.Close();
   }

   public void showMap() {

      int startX = Program.curPlayer.PosX - viewSize >= 0 ? Program.curPlayer.PosX - Program.curMap.viewSize : 0;
      int endX = Program.curPlayer.PosX + viewSize <= mapSize ? Program.curPlayer.PosX + Program.curMap.viewSize : Program.curMap.mapSize;

      int startY = Program.curPlayer.PosY - viewSize >= 0 ? Program.curPlayer.PosY - Program.curMap.viewSize : 0;
      int endY = Program.curPlayer.PosY + viewSize <= mapSize ? Program.curPlayer.PosY + Program.curMap.viewSize : Program.curMap.mapSize;

      int keysCreated = 0;

      for (int y = startY; y < endY; y++) {
         for (int x = startX; x < endX; x++) {
            if ((x, y) == Program.curPlayer.Position) {
               Console.Write((char)Program.curPlayer.Looking);
               if (keysCreated < Keys.Count && (x, y) == Keys[keysCreated].Position)
                  keysCreated++;
               continue;
            }

            if (keysCreated < Keys.Count && (x, y) == Keys[keysCreated].Position) {
               if (Keys[keysCreated].Alive)
                  Console.Write('*');
               else
                  Console.Write(' ');
               keysCreated++;
               continue;
            }


            Console.Write(Map[x, y]);
         }
         Console.WriteLine();
      }
   }
};