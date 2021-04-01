using System.Text;

namespace Matrix_Parallel_1
{
    class Matrix
    {
        private int[,] _data;
        public int Size { get; set; }

        public Matrix(int size)
        {
            Size = size;
            _data = new int[size, size];
        }

        public int this[int x, int y]
        {
            get => _data[x, y];
            set => _data[x, y] = value;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                stringBuilder.Append("[ ");
                for (int j = 0; j < Size; j++)
                {
                    stringBuilder.Append(_data[i, j]);
                    stringBuilder.Append(j == Size - 1 ? " " : ", ");
                }
                stringBuilder.AppendLine("]");
            }
            return stringBuilder.ToString();
        }
    }
}
