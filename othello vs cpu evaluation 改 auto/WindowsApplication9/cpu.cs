using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication9
{
    class cpu
    {
        public static int cpu_put(int[,] cells, int turn, int cto, int[,] cellcpu, int max, int[,] undocellcpu, int[,] ecells)
        {
            cpu.cpu_marking(cells, (int)cellstatus.black);//cpu_markingメソッド

            //MessageBox.Show("置ける場所" + canput.ToString());

            cto = cpu.cpu_corner(cells, turn, cto);//隅に置くメソッド

            if (cto != 0)
            {
                goto terminate;
            }

            for (int x = 0; x <= 9; x++)//cpuマーク情報の保存
            {
                for (int y = 0; y <= 9; y++)
                {
                    undocellcpu[x, y] = cells[x, y];
                }
            }

            int total = 0;
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    if (cells[i, j] == (int)cellstatus.black || cells[i, j] == (int)cellstatus.white)
                    {
                        total++;
                    }
                }
            }

            int r = 0;
            if (total > 43)
            {
                r = cpu.cpu_max(cells, (int)cellstatus.black, cellcpu, undocellcpu, max);
            }
            else if (total <= 43)
            {
                r = cpu.cpu_evaluation(cells, (int)cellstatus.black, cellcpu, undocellcpu, max, ecells);
            }

            //MessageBox.Show("コマ数最大値" + max.ToString());

            cto = cpu.cpu_turnover(cellcpu, r, cells, turn, cto);//cpu_turnoverメソッド

            //MessageBox.Show("裏返しのチェック" + cto.ToString());

         terminate: ;

            for (int xx = 1; xx <= 8; xx++)
            {
                for (int yy = 1; yy <= 8; yy++)
                {
                    if (cells[xx, yy] == (int)cellstatus.cpu)
                    {
                        cells[xx, yy] = (int)cellstatus.nothing;
                    }
                    if (cellcpu[xx, yy] != -200)
                    {
                        cellcpu[xx, yy] = -200;
                    }
                }
            }


            if (cto != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }//cpu_putメソッド


        public static void cpu_marking(int[,] cells, int turn)//コマが置ける場所にcpuマークを付ける
        {
            int canput = 0;
            for (int x = 1; x <= 8; x++)//全升目の情報を取得する。
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cells[x, y] == (int)cellstatus.nothing)
                    {
                        for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//一番目の処理
                                {
                                    for (int i = 2; i <= 8; i++)//二番目以降の処理
                                    {
                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.black)
                                                {
                                                    cells[x, y] = (int)cellstatus.cpu;
                                                    canput++;
                                                    goto a;
                                                }
                                                else if (turn == (int)cellstatus.white)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)
                                                {
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)
                                                {
                                                    cells[x, y] = (int)cellstatus.cpu;
                                                    canput++;
                                                    goto a;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.nothing:
                                                goto a;

                                            case (int)cellstatus.cpu:
                                                goto a;
                                        }
                                    }
                                }
                            a: ;
                            }
                        }
                    }
                }
            }

        }//cpu_markingメソッド

        public static int cpu_corner(int[,] cells, int turn, int cto)//隅に置ける場合は隅に置く
        {
          for (int x = 1; x <= 9; x += 7)
          {
            for (int y = 1; y <= 9; y += 7)
            {
              if (cells[x, y] == (int)cellstatus.cpu)
              {
                for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                {
                  for (int f = -1; f <= 1; f++)
                  {
                    if (cells[x + 1 * d, y + 1 * f] == turn)//一番目の処理
                    {
                      int count = 1;

                      for (int i = 2; i <= 8; i++)
                      {
                        switch (cells[x + i * d, y + i * f])
                        {
                          case (int)cellstatus.white:
                            if (turn == (int)cellstatus.white)
                            {
                              count++;
                              break;
                            }
                            else if (turn == (int)cellstatus.black)
                            {
                              cells[x, y] = (int)cellstatus.white;
                              for (int z = 1; z <= count; z++)
                              {
                                cells[x + z * d, y + z * f] = (int)cellstatus.white;
                              }
                              cto++;
                              goto a;
                            }
                            else
                            {
                              goto a;
                            }

                          case (int)cellstatus.black:
                            if (turn == (int)cellstatus.white)
                            {
                              cells[x, y] = (int)cellstatus.black;
                              for (int z = 1; z <= count; z++)
                              {
                                cells[x + z * d, y + z * f] = (int)cellstatus.black;
                              }
                              cto++;
                              goto a;
                            }
                            else if (turn == (int)cellstatus.black)
                            {
                              count++;
                              break;
                            }
                            else
                            {
                              goto a;
                            }

                          case (int)cellstatus.nothing:
                            goto a;
                        }
                      }
                    }
                  a: ;
                  }
                }
                return cto;
              }
              else
              {
                return cto;
              }
            }
          }

          return cto;



            /*if (cells[1, 1] == (int)cellstatus.cpu)
            {
                for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[1 + 1 * d, 1 + 1 * f] == turn)//一番目の処理
                        {
                            int count = 1;

                            for (int i = 2; i <= 8; i++)
                            {
                                switch (cells[1 + i * d, 1 + i * f])
                                {
                                    case (int)cellstatus.white:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            count++;
                                            break;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            cells[1, 1] = (int)cellstatus.white;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[1 + z * d, 1 + z * f] = (int)cellstatus.white;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.black:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            cells[1, 1] = (int)cellstatus.black;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[1 + z * d, 1 + z * f] = (int)cellstatus.black;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            count++;
                                            break;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.nothing:
                                        goto a;
                                }
                            }
                        }
                    a: ;
                    }
                }
                return cto;
            }

            //隅に置ける場合は隅に置く
            if (cells[1, 8] == (int)cellstatus.cpu)
            {
                for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[1 + 1 * d, 8 + 1 * f] == turn)//一番目の処理
                        {
                            int count = 1;

                            for (int i = 2; i <= 8; i++)
                            {
                                switch (cells[1 + i * d, 8 + i * f])
                                {
                                    case (int)cellstatus.white:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            count++;
                                            break;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            cells[1, 8] = (int)cellstatus.white;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[1 + z * d, 8 + z * f] = (int)cellstatus.white;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.black:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            cells[1, 8] = (int)cellstatus.black;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[1 + z * d, 8 + z * f] = (int)cellstatus.black;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            count++;
                                            break;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.nothing:
                                        goto a;
                                }
                            }
                        }
                    a: ;
                    }
                }
                return cto;
            }

            //隅に置ける場合は隅に置く
            if (cells[8, 1] == (int)cellstatus.cpu)
            {
                for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[8 + 1 * d, 1 + 1 * f] == turn)//一番目の処理
                        {
                            int count = 1;

                            for (int i = 2; i <= 8; i++)
                            {
                                switch (cells[8 + i * d, 1 + i * f])
                                {
                                    case (int)cellstatus.white:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            count++;
                                            break;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            cells[8, 1] = (int)cellstatus.white;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[8 + z * d, 1 + z * f] = (int)cellstatus.white;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.black:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            cells[8, 1] = (int)cellstatus.black;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[8 + z * d, 1 + z * f] = (int)cellstatus.black;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            count++;
                                            break;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.nothing:
                                        goto a;
                                }
                            }
                        }
                    a: ;
                    }
                }
                return cto;
            }

            //隅に置ける場合は隅に置く
            else if (cells[8, 8] == (int)cellstatus.cpu)
            {
                for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[8 + 1 * d, 8 + 1 * f] == turn)//一番目の処理
                        {
                            int count = 1;

                            for (int i = 2; i <= 8; i++)
                            {
                                switch (cells[8 + i * d, 8 + i * f])
                                {
                                    case (int)cellstatus.white:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            count++;
                                            break;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            cells[8, 8] = (int)cellstatus.white;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[8 + z * d, 8 + z * f] = (int)cellstatus.white;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.black:
                                        if (turn == (int)cellstatus.white)
                                        {
                                            cells[8, 8] = (int)cellstatus.black;
                                            for (int z = 1; z <= count; z++)
                                            {
                                                cells[8 + z * d, 8 + z * f] = (int)cellstatus.black;
                                            }
                                            cto++;
                                            goto a;
                                        }
                                        else if (turn == (int)cellstatus.black)
                                        {
                                            count++;
                                            break;
                                        }
                                        else
                                        {
                                            goto a;
                                        }

                                    case (int)cellstatus.nothing:
                                        goto a;
                                }
                            }
                        }
                    a: ;
                    }
                }
                return cto;
            }
            else
            {
                return cto;
            }*/

        }//cpu_cornerメソッド

        public static int cpu_max(int[,] cells, int turn, int[,] cellcpu, int[,] undocellcpu, int max)//多く取れる場所に置く
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cells[x, y] == (int)cellstatus.cpu)//最初に見つけた場所
                    {
                        for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//一番目の処理
                                {
                                    int count = 1;

                                    for (int i = 2; i <= 8; i++)
                                    {
                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.black)//白番
                                                {
                                                    cells[x, y] = (int)cellstatus.white;//コマを裏返す
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.white;
                                                    }

                                                    cellcpu[x, y] += count;//カウントした値をcellcpuに加算代入して保存する

                                                    for (int p = 0; p <= 9; p++)//裏返したコマをもとに戻す
                                                    {
                                                        for (int q = 0; q <= 9; q++)
                                                        {
                                                            cells[p, q] = undocellcpu[p, q];
                                                        }
                                                    }

                                                    goto a;
                                                }
                                                else if (turn == (int)cellstatus.white)//黒番
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)//白番
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)//黒番
                                                {
                                                    cells[x, y] = (int)cellstatus.black;
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.black;
                                                    }
                                                    goto a;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.nothing:
                                                goto a;

                                            case (int)cellstatus.cpu:
                                                goto a;
                                        }
                                    }
                                }
                            a: ;
                            }
                        }

                    }//if max

                }//for y

            }//for x

            foreach (int data in cellcpu)//最大値の算出
            {
                max = Math.Max(max, data);
            }

            return max;
        }//cpu_maxメソッド

        public static int cpu_evaluation(int[,] cells, int turn, int[,] cellcpu, int[,] undocellcpu, int max, int[,] ecells)//評価盤をもとに石を置く
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cells[x, y] == (int)cellstatus.cpu)//最初に見つけた場所
                    {
                        int esumb = 0;
                        int esumw = 0;
                        for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//一番目の処理
                                {
                                    int count = 1;

                                    for (int i = 2; i <= 8; i++)
                                    {
                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.black)//白番
                                                {
                                                    cells[x, y] = (int)cellstatus.white;//コマを裏返す
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.white;
                                                    }

                                                    goto a;
                                                }
                                                else if (turn == (int)cellstatus.white)//黒番
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)//白番
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)//黒番
                                                {
                                                    cells[x, y] = (int)cellstatus.black;
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.black;
                                                    }
                                                    goto a;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.nothing:
                                                goto a;

                                            case (int)cellstatus.cpu:
                                                goto a;
                                        }
                                    }
                                }
                            a: ;
                            }
                        }

                        //一つのマス目の全方向に対して処理を行った後に評価の処理を行う 評価合計を計算して、cellcpuに格納、裏返したコマをもとに戻す。
                        for (int m = 1; m <= 8; m++)
                        {
                            for (int n = 1; n <= 8; n++)
                            {
                                if (cells[m, n] == (int)cellstatus.black)
                                {
                                    esumb += ecells[m, n];
                                }
                                else if (cells[m, n] == (int)cellstatus.white)
                                {
                                    esumw += ecells[m, n];
                                }
                            }
                        }

                        cellcpu[x, y] = esumw - esumb;//評価値の代入
                        /*int t = esumw - esumb;
                        MessageBox.Show(t.ToString());*/


                        for (int p = 0; p <= 9; p++)//裏返したコマをもとに戻す
                        {
                            for (int q = 0; q <= 9; q++)
                            {
                                cells[p, q] = undocellcpu[p, q];
                            }
                        }


                    }//if max この中で一つのマス目に対して全方向の裏返しが完了しているはず。↓次のマス目を探しにいく

                }//for y

            }//for x


            foreach (int data in cellcpu)//最大値の算出
            {
                max = Math.Max(max, data);
            }

            return max;
        }//cpu_evaluationメソッド

        public static int cpu_turnover(int[,] cellcpu, int r, int[,] cells, int turn, int cto)//指定された場所にコマを置いて裏返す
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cellcpu[x, y] == r)//最初に見つけた場所
                    {
                        for (int d = -1; d <= 1; d++)//8方向の升目情報を取得するループ
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//一番目の処理
                                {
                                    int count = 1;
                                    for (int i = 2; i <= 8; i++)//一方向に複数回の処理を行う
                                    {

                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)//白番
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)//黒番
                                                {
                                                    cells[x, y] = (int)cellstatus.black;
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.black;
                                                    }
                                                    cto++;
                                                    goto c;
                                                }
                                                else
                                                {
                                                    break;
                                                }

                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.black)//白番
                                                {
                                                    cells[x, y] = (int)cellstatus.white;
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.white;
                                                    }
                                                    cto++;
                                                    goto c;
                                                }
                                                else if (turn == (int)cellstatus.white)//黒番
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else
                                                {
                                                    break;
                                                }

                                            case (int)cellstatus.nothing:
                                                goto c;

                                            case (int)cellstatus.cpu:
                                                goto c;

                                        }//switch
                                    }//for i
                                }
                            c: ;
                            }//for f

                        }//for d
                        goto d; //一箇所処理を終えたら外に抜け出す
                    }//if max

                }//for y

            }//for x
        d: ;

            return cto;

        }//cpu_turnoverメソッド

        public static void undo_record(int[,] cells, int[,] undocells)
        {
            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    undocells[x, y] = cells[x, y];
                }
            }
        }//undo_recordメソッド

        public static int undo_turnover(int[,] cells, int[,] undocells)
        {
            int count = 0;
            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    if (cells[x, y] != undocells[x, y])
                    {
                        cells[x, y] = undocells[x, y];
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return 0;
            }
            else if (count != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }//undo_turnoverメソッド


    }//cpuクラス
}
