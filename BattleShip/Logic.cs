using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    class Logic
    {
        private int[][] _You = new int[10][];
        private int[][] _Enemy = new int[10][];
        private bool _vertical;
        private int _count1X;
        private int _count2X;
        private int _count3X;
        private int _count4X;
        private int _myShip = 20;
        private int _enemyShip = 20;


        public Logic()
        {
            for (int i = 0; i < _You.Count(); i++)
            {
                _You[i] = new int[10];
                _Enemy[i] = new int[10];
                for (int j = 0; j < _You[i].Count(); j++)
                {
                    _You[i][j] = 0;
                    _Enemy[i][j] = 0;
                }
            }
            _vertical = true;
        }
        //отрисовываем корабли, когда водим мышкой
        public void WatchShip(int y, int x, int w, int h, int size, Panel panelLogic)
        {
            NullMas();
            CreateShip(x, y, 1, size - 1, _You);
            for (int i = 0; i < _You.Count(); i++)
                for (int j = 0; j < _You[i].Count(); j++)
                    if (Rendering(x, y, i, j, size - 1)) panelLogic.Invalidate(new Rectangle(i*w, j*h, w, h));
        }
        //убираем временные корабли с поля
        public void NullMas()
        {
            for (int i = 0; i < _You.Count(); i++)
                for (int j = 0; j < _You[i].Count(); j++)
                    if (_You[i][j] != 2)
                        _You[i][j] = 0;          
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
                    if (y + i >= 0 && y + i < 10 && x<10)
                            if (mas[y + i][x] != 2)
                                mas[y + i][x] = value;
            dopPoint = mas.Count() - 1 - x - size;
            if (dopPoint > 0) dopPoint = 0;
            int jMinLimitH = 0 + dopPoint;
            int jMaxLimitH = 1 + size;
            if (!_vertical)
                for (int j = jMinLimitH; j < jMaxLimitH; j++)
                    if (x + j >= 0 && x + j < 10 && y<10)
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
            int dopPoint = mas.Count()  - y - size;
            if (dopPoint > 0) dopPoint = 0;
            int iMinLimitV = -1 + dopPoint;
            int iMaxLimitV = 2 + size;
            if (_vertical)
                for (int i = iMinLimitV; i < iMaxLimitV; i++)
                    for (int j = -1; j < 2; j++)
                        if (y + i >= 0 && y + i < 10 && x + j >= 0 && x + j < 10)
                        if (mas[y + i][x + j] > 0)
                                return false;
            dopPoint = mas.Count()  - x - size;
            if (dopPoint > 0) dopPoint = 0;
            //else return false;
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
            return 0;
        }

        public void Reset()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    _You[i][j] = 0;
            _count1X = 0;
            _count2X = 0;
            _count3X = 0;
            _count4X = 0;
        }
        public int this[int index1, int index2]
        {
            get { return _You[index1][index2]; }
            set { _You[index1][index2] = value; }
        }

        public void ChangeVerctical()
        {
            if (_vertical) _vertical = false;
            else _vertical = true;
        }

        public int ReturnMasSize()
        {
            return _You.Count();
        }

        public int[][] GetLink()
        {
            return _You;
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
    }
}
