//Key class to hold position and if it is alive
class Key {
   public bool alive;
   public IVec2 position;

   public Key() {
      alive = true;
      position = new IVec2(0);
   }

   public Key(bool alive, IVec2 pos) {
      this.alive = alive;
      position = pos;
   }
};

//character map class
class CMap {
   //2D char array
   public char[,] map;
   //List of keys
   public List<Key> keys;

   //View size is the distance in all directions that will be drawn to screen
   public int viewSize;
   public int mapSize;

   //Empty map
   public CMap() {
      keys = new List<Key>();
   }


   //Opens the map and loads data
   public void LoadMapFromFile() {
      StreamReader sr = new StreamReader(Program.mapSaveLoc);

      //Error if the map is empty
      if (sr.Peek() == -1)
         throw new IOException("map empty");

      //Read first line which includes map information
      string fLine = sr.ReadLine() ?? "";
      string[] mapVars = fLine.Split(',', StringSplitOptions.TrimEntries);

      if (mapVars.Count() == 2) {
         if (!Int32.TryParse(mapVars[0], out mapSize))
            mapSize = 5;
         if (!Int32.TryParse(mapVars[1], out viewSize))
            viewSize = 5;
      } else
         throw new ArgumentException("Invalid count of map variables");

      //keys was already made before in constructor
      map = new char[mapSize, mapSize];

      //If map sets larger size than what is there this will allow the size but with empty spaces
      bool fillRest = false;
      for (int y = 0; y < mapSize; y++) {
         string? line = sr.ReadLine();
         if (line == null)
            fillRest = true;
         for (int x = 0; x < mapSize; x++) {
            if (fillRest || x >= line.Count()) {
               map[x, y] = ' ';
               continue;
            }

            if (line[x] == '*')
               keys.Add(new Key(true, new IVec2(x, y)));

            map[x, y] = line[x];
         }
      }
      sr.Close();
   }

   //Shows map into the console
   public void ShowMap() {
      //Since view might go past map size there needs to be checks on where to start and stop writing to console

      int startX = Program.curPlayer.posX - viewSize >= 0 ? Program.curPlayer.posX - Program.curMap.viewSize : 0;
      int endX = Program.curPlayer.posX + viewSize <= mapSize ? Program.curPlayer.posX + Program.curMap.viewSize : Program.curMap.mapSize;

      int startY = Program.curPlayer.posY - viewSize >= 0 ? Program.curPlayer.posY - Program.curMap.viewSize : 0;
      int endY = Program.curPlayer.posY + viewSize <= mapSize ? Program.curPlayer.posY + Program.curMap.viewSize : Program.curMap.mapSize;


      for (int y = startY; y < endY; y++) {
         for (int x = startX; x < endX; x++) {
            if ((x, y) == Program.curPlayer.position) {
               //Player is an arrow that is the direction that was last pressed
               Console.Write((char)Program.curPlayer.looking);
               continue;
            }

            //Get key, if any, that is on position
            Key? curKey = keys.Find(key => (x, y) == key.position);

            //Write character, or key
            if (curKey == null)
               Console.Write(map[x, y]);
            else if (curKey.alive)
               Console.Write('*');
            else
               Console.Write(' ');
         }
         Console.WriteLine();
      }
   }
};