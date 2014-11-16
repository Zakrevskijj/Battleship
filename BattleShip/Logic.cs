using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BattleShip
{
    class Logic
    {
        #region Values
        private int[][] _you = new int[10][];
        private int[][] _Enemy = new int[10][];
        private static bool _vertical;
        private int _count1X;
        private int _count2X;
        private int _count3X;
        private int _count4X;
        private int _myShip = 20;
        private int _enemyShip = 20;
        #endregion

        #region Properties
        public static bool Vertical
        {
            get { return _vertical; }
            set { _vertical = value; }
        }

        public int Count1X
        {
            get { return _count1X; }
            set { _count1X = value; }
        }

        public int Count2X
        {
            get { return _count2X; }
            set { _count2X = value; }
        }

        public int Count3X
        {
            get { return _count3X; }
            set { _count3X = value; }
        }

        public int Count4X
        {
            get { return _count4X; }
            set { _count4X = value; }
        }

        public int MyShip
        {
            get { return _myShip; }
            set { _myShip = value; }
        }

        public int EnemyShip
        {
            get { return _enemyShip; }
            set { _enemyShip = value; }
        }
        #endregion

        public Logic()
        {
            for (int i = 0; i < _you.Count(); i++)
            {
                _you[i] = new int[10];
                _Enemy[i] = new int[10];
                for (int j = 0; j < _you[i].Count(); j++)
                {
                    _you[i][j] = 0;
                    _Enemy[i][j] = 0;
                }
            }
            _vertical = true;
        }
        //отрисовываем корабли
        public void WatchShip(int y, int x, int w, int h, int size, Panel panelBattle)
        {
            NullMas();
            CreateShip(x, y, 1, size - 1, _you);
            for (int i = 0; i < _you.Count(); i++)
                for (int j = 0; j < _you[i].Count(); j++)
                    if (Rendering(x, y, i, j, size - 1)) panelBattle.Invalidate(new Rectangle(i*w, j*h, w, h));
        }
        //убираем временные корабли с поля
        public void NullMas()
        {
            for (int i = 0; i < _you.Count(); i++)
                for (int j = 0; j < _you[i].Count(); j++)
                    if (_you[i][j] != 2)
                        _you[i][j] = 0;          
        }
        //размещаем временные и постоянные корабли на поле
        public void CreateShip(int x, int y, int value, int size, int[][] mas)
        {
            int dopPoint = mas.Count() - 1 - y - size;
            if (dopPoint > 0) dopPoint = 0;
            int iMinLimitV = 0 + dopPoint;
            int iMaxLimitV = 1 + size;
            if (_vertical)
                for (int i = iMinLimitV; i < iMaxLimitV; i++)
                    if (y + i >= 0 && y + i < 10 && x < 10)
                        if (mas[y + i][x] != 2)
                            mas[y + i][x] = value;
            dopPoint = mas.Count() - 1 - x - size;
            if (dopPoint > 0) dopPoint = 0;
            int jMinLimitH = 0 + dopPoint;
            int jMaxLimitH = 1 + size;
            if (!_vertical)
                for (int j = jMinLimitH; j < jMaxLimitH; j++)
                    if (x + j >= 0 && x + j < 10 && y < 10)
                    if (x + j >= 0 && x + j < 10)
                        if (mas[y][x + j] != 2)
                            mas[y][x + j] = value;
        }
        //находим кусочек поля, который нада обновить
        public bool Rendering(int x, int y, int i, int j, int size)
        {
            int dopPoint = 9 - y - size;
            if (dopPoint > 0) dopPoint = 0;
            int uMinLimitV = -1 + dopPoint;
            int uMaxLimitV = 2 + size;
            if (_vertical)
                for (int u = uMinLimitV; u < uMaxLimitV; u++)
                    for (int z = -1; z < 2; z++)
                        if (y + u >= 0 && y + u < 10 && x + z >= 0 && x + z < 10)
                            if (y + u == j && z + x == i)
                                return true;
            dopPoint = 9 - x - size;
            if (dopPoint > 0) dopPoint = 0;
            int zMinLimitH = -1 + dopPoint;
            int zMaxLimitH = 2 + size;
            if (!_vertical)
                for (int u = -1; u < 2; u++)
                    for (int z = zMinLimitH; z < zMaxLimitH; z++)
                        if (y + u >= 0 && y + u < 10 && x + z >= 0 && x + z < 10)
                            if (y + u == j && z + x == i)
                                return true;
            return false;
        }
        
        //проверяем квадраты на соседства
        public bool CheckSq(int x, int y, int size, int[][] mas)
        {
            int dopPoint = mas.Count() - 1 - y - size;
            if (dopPoint > 0) dopPoint = 0;
            int iMinLimitV = -1 + dopPoint;
            int iMaxLimitV = 2 + size;
            if (_vertical)
                for (int i = iMinLimitV; i < iMaxLimitV; i++)
                    for (int j = -1; j < 2; j++)
                        if (y + i >= 0 && y + i < 10 && x + j >= 0 && x + j < 10)
                            if (mas[y + i][x + j] > 0)
                                return false;
            dopPoint = mas.Count() - 1 - x - size;
            if (dopPoint > 0) dopPoint = 0;
            int jMinLimitH = -1 + dopPoint;
            int jMaxLimitH = 2 + size;
            if (!_vertical)
                for (int i = -1; i < 2; i++)
                    for (int j = jMinLimitH; j < jMaxLimitH; j++)
                        if (y + i >= 0 && y + i < 10 && x + j >= 0 && x + j < 10)
                            if (mas[y + i][x + j] > 0)
                                return false;
            return true;
        }

        //проверяем и создаем взрыв
        public void Explosion(int x, int y, int[][] mas)
        {
            int size = 0;
            bool flag = true; //пора взрывать или нет
            int bufI = y, bufJ = x;
            //проверяем клетки вверху
            while (bufI > 0 && (mas[bufI - 1][bufJ] == 3 || mas[bufI - 1][bufJ] == 2))
            {
                bufI--;
                size++;
                if (bufI >= 0 && mas[bufI][bufJ] == 2) flag = false;
            }
            bufI = y; bufJ = x;
            //проверяем клетки внизу
            while (bufI < 9 && (mas[bufI + 1][bufJ] == 3 || mas[bufI + 1][bufJ] == 2))
            {
                bufI++;
                size++;
                if (bufI < 10 && mas[bufI][bufJ] == 2) flag = false;
            }
            bufI = y; bufJ = x;
            //проверяем клетки слева
            while (bufJ > 0 && (mas[bufI][bufJ - 1] == 3 || mas[bufI][bufJ - 1] == 2))
            {
                bufJ--;
                size++;
                if (bufJ >= 0 && mas[bufI][bufJ] == 2) flag = false;
            }
            bufI = y; bufJ = x;
            //проверяем клетки справа
            while (bufJ < 9 && (mas[bufI][bufJ + 1] == 3 || mas[bufI][bufJ + 1] == 2))
            {
                bufJ++;
                size++;
                if (bufJ < 10 && mas[bufI][bufJ] == 2) flag = false;
            }       
            //взрываем
            if (flag)
            {
                bool vert;
                //находим самую верхнюю или самую левую клетку
                while (y > 0 && mas[y - 1][x] == 3) y--;
                while (x > 0 && mas[y][x - 1] == 3) x--;
                //определяем положение корабля
                if (y < 9 && mas[y + 1][x] == 3) vert = true;
                else vert = false;
                //бабах
                if (vert)
                    for (int i = -1; i < 2 + size; i++)
                        for (int j = -1; j < 2; j++)
                            if (y + i >= 0 && y + i < 10 && x + j >= 0 && x + j < 10 && mas[y + i][x + j] != 3)
                                mas[y + i][x + j] = 4;
                if (!vert)
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2 + size; j++)
                            if (y + i >= 0 && y + i < 10 && x + j >= 0 && x + j < 10 && mas[y + i][x + j] != 3)
                                mas[y + i][x + j] = 4;
            }
        }

        public int FindNextRb(RadioButton r1, RadioButton r2, RadioButton r3, RadioButton r4)
        {
            if (r1.Enabled)
            {
                r1.Checked = true;
                return 0;
            }
            if (r2.Enabled)
            {
                r2.Checked = true;
                return 0;
            }
            if (r3.Enabled)
            {
                r3.Checked = true;
                return 0;
            }
            if (r4.Enabled)
            {
                r4.Checked = true;
                return 0;
            }
            return 1;
        }

        public void Reset()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    _you[i][j] = 0;
            _count1X = 0;
            _count2X = 0;
            _count3X = 0;
            _count4X = 0;
        }
        //индексаторы, размеры и прочая хрень
        public int this[int index1, int index2]
        {
            get { return _you[index1][index2]; }
            set { _you[index1][index2] = value; }
        }

        public void ChangeVerctical()
        {
            if (_vertical) _vertical = false;
            else _vertical = true;
        }

        public int ReturnMasSize()
        {
            return _you.Count();
        }

        public int[][] GetLink()
        {
            return _you;
        }

        public int[][] GetEnemyLink()
        {
            return _Enemy;
        }

        public int GetEnemyValue(int i, int j)
        {
            return _Enemy[i][j];
        }

        public void SetEnemyValue(int i, int j, int value)
        {
            _Enemy[i][j] = value;
        }
    }
}
