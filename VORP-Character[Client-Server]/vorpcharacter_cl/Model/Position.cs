namespace VorpCharacter.Model
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float H { get; set; }

        public Position()
        {
        }

        public Position(float x, float y, float z, float heading)
        {
            X = x;
            Y = y;
            Z = z;
            H = heading;
        }

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position Subtract(Position position)
        {
            X = X - position.X;
            Y = Y - position.Y;
            Z = Z - position.Z;
            H = H - position.H;

            return this;
        }

        public Position Add(Position position)
        {
            X = X + position.X;
            Y = Y + position.Y;
            Z = Z + position.Z;
            H = H + position.H;

            return this;
        }

        public Position Clone()
        {
            return new Position(X, Y, Z, H);
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Z} ({H})";
        }
    }
}
