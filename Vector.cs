class IVec2 {
   public int x = 0;
   public int y = 0;

   public IVec2() { }
   public IVec2(int s, int t) { x = s; y = t; }
   public IVec2(int val) { x = val; y = val; }

   public static IVec2 operator +(IVec2 a, IVec2 b) => new IVec2(a.x + b.x, a.y + b.y);
   public static IVec2 operator -(IVec2 a, IVec2 b) => new IVec2(a.x - b.x, a.y - b.y);

   public static bool operator ==(IVec2 a, IVec2 b) => (a.x == b.x && a.y == b.y) ? true : false;
   public static bool operator !=(IVec2 a, IVec2 b) => !(a == b);

   public static bool operator ==(IVec2 a, (int x, int y) b) => (a.x == b.x && a.y == b.y) ? true : false;
   public static bool operator !=(IVec2 a, (int x, int y) b) => !(a == b);

   public static bool operator ==((int x, int y) a, IVec2 b) => (a.x == b.x && a.y == b.y) ? true : false;
   public static bool operator !=((int x, int y) a, IVec2 b) => !(a == b);

   public override string ToString()  {
      return $"{x},{y}";
   }

   public static bool TryParse(string input, out IVec2 output) {
      output = new IVec2(0);

      string[] values = input.Split(',', StringSplitOptions.TrimEntries);
      if (values.Count() != 2)
         return false;

      if (!Int32.TryParse(values[0], out output.x))
         return false;

      if (!Int32.TryParse(values[1], out output.y))
         return false;

      return true;
   }
};