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
            cpu.cpu_marking(cells, (int)cellstatus.black);//cpu_marking���\�b�h

            //MessageBox.Show("�u����ꏊ" + canput.ToString());

            cto = cpu.cpu_corner(cells, turn, cto);//���ɒu�����\�b�h

            if (cto != 0)
            {
                goto terminate;
            }

            for (int x = 0; x <= 9; x++)//cpu�}�[�N���̕ۑ�
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

            //MessageBox.Show("�R�}���ő�l" + max.ToString());

            cto = cpu.cpu_turnover(cellcpu, r, cells, turn, cto);//cpu_turnover���\�b�h

            //MessageBox.Show("���Ԃ��̃`�F�b�N" + cto.ToString());

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
        }//cpu_put���\�b�h


        public static void cpu_marking(int[,] cells, int turn)//�R�}���u����ꏊ��cpu�}�[�N��t����
        {
            int canput = 0;
            for (int x = 1; x <= 8; x++)//�S���ڂ̏����擾����B
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cells[x, y] == (int)cellstatus.nothing)
                    {
                        for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//��Ԗڂ̏���
                                {
                                    for (int i = 2; i <= 8; i++)//��Ԗڈȍ~�̏���
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

        }//cpu_marking���\�b�h

        public static int cpu_corner(int[,] cells, int turn, int cto)//���ɒu����ꍇ�͋��ɒu��
        {
          for (int x = 1; x <= 9; x += 7)
          {
            for (int y = 1; y <= 9; y += 7)
            {
              if (cells[x, y] == (int)cellstatus.cpu)
              {
                for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                {
                  for (int f = -1; f <= 1; f++)
                  {
                    if (cells[x + 1 * d, y + 1 * f] == turn)//��Ԗڂ̏���
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
                for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[1 + 1 * d, 1 + 1 * f] == turn)//��Ԗڂ̏���
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

            //���ɒu����ꍇ�͋��ɒu��
            if (cells[1, 8] == (int)cellstatus.cpu)
            {
                for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[1 + 1 * d, 8 + 1 * f] == turn)//��Ԗڂ̏���
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

            //���ɒu����ꍇ�͋��ɒu��
            if (cells[8, 1] == (int)cellstatus.cpu)
            {
                for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[8 + 1 * d, 1 + 1 * f] == turn)//��Ԗڂ̏���
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

            //���ɒu����ꍇ�͋��ɒu��
            else if (cells[8, 8] == (int)cellstatus.cpu)
            {
                for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                {
                    for (int f = -1; f <= 1; f++)
                    {
                        if (cells[8 + 1 * d, 8 + 1 * f] == turn)//��Ԗڂ̏���
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

        }//cpu_corner���\�b�h

        public static int cpu_max(int[,] cells, int turn, int[,] cellcpu, int[,] undocellcpu, int max)//��������ꏊ�ɒu��
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cells[x, y] == (int)cellstatus.cpu)//�ŏ��Ɍ������ꏊ
                    {
                        for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//��Ԗڂ̏���
                                {
                                    int count = 1;

                                    for (int i = 2; i <= 8; i++)
                                    {
                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.black)//����
                                                {
                                                    cells[x, y] = (int)cellstatus.white;//�R�}�𗠕Ԃ�
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.white;
                                                    }

                                                    cellcpu[x, y] += count;//�J�E���g�����l��cellcpu�ɉ��Z������ĕۑ�����

                                                    for (int p = 0; p <= 9; p++)//���Ԃ����R�}�����Ƃɖ߂�
                                                    {
                                                        for (int q = 0; q <= 9; q++)
                                                        {
                                                            cells[p, q] = undocellcpu[p, q];
                                                        }
                                                    }

                                                    goto a;
                                                }
                                                else if (turn == (int)cellstatus.white)//����
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)//����
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)//����
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

            foreach (int data in cellcpu)//�ő�l�̎Z�o
            {
                max = Math.Max(max, data);
            }

            return max;
        }//cpu_max���\�b�h

        public static int cpu_evaluation(int[,] cells, int turn, int[,] cellcpu, int[,] undocellcpu, int max, int[,] ecells)//�]���Ղ����Ƃɐ΂�u��
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cells[x, y] == (int)cellstatus.cpu)//�ŏ��Ɍ������ꏊ
                    {
                        int esumb = 0;
                        int esumw = 0;
                        for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//��Ԗڂ̏���
                                {
                                    int count = 1;

                                    for (int i = 2; i <= 8; i++)
                                    {
                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.black)//����
                                                {
                                                    cells[x, y] = (int)cellstatus.white;//�R�}�𗠕Ԃ�
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.white;
                                                    }

                                                    goto a;
                                                }
                                                else if (turn == (int)cellstatus.white)//����
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)//����
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)//����
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

                        //��̃}�X�ڂ̑S�����ɑ΂��ď������s������ɕ]���̏������s�� �]�����v���v�Z���āAcellcpu�Ɋi�[�A���Ԃ����R�}�����Ƃɖ߂��B
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

                        cellcpu[x, y] = esumw - esumb;//�]���l�̑��
                        /*int t = esumw - esumb;
                        MessageBox.Show(t.ToString());*/


                        for (int p = 0; p <= 9; p++)//���Ԃ����R�}�����Ƃɖ߂�
                        {
                            for (int q = 0; q <= 9; q++)
                            {
                                cells[p, q] = undocellcpu[p, q];
                            }
                        }


                    }//if max ���̒��ň�̃}�X�ڂɑ΂��đS�����̗��Ԃ����������Ă���͂��B�����̃}�X�ڂ�T���ɂ���

                }//for y

            }//for x


            foreach (int data in cellcpu)//�ő�l�̎Z�o
            {
                max = Math.Max(max, data);
            }

            return max;
        }//cpu_evaluation���\�b�h

        public static int cpu_turnover(int[,] cellcpu, int r, int[,] cells, int turn, int cto)//�w�肳�ꂽ�ꏊ�ɃR�}��u���ė��Ԃ�
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cellcpu[x, y] == r)//�ŏ��Ɍ������ꏊ
                    {
                        for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//��Ԗڂ̏���
                                {
                                    int count = 1;
                                    for (int i = 2; i <= 8; i++)//������ɕ�����̏������s��
                                    {

                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)//����
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)//����
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
                                                if (turn == (int)cellstatus.black)//����
                                                {
                                                    cells[x, y] = (int)cellstatus.white;
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.white;
                                                    }
                                                    cto++;
                                                    goto c;
                                                }
                                                else if (turn == (int)cellstatus.white)//����
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
                        goto d; //��ӏ��������I������O�ɔ����o��
                    }//if max

                }//for y

            }//for x
        d: ;

            return cto;

        }//cpu_turnover���\�b�h

        public static void undo_record(int[,] cells, int[,] undocells)
        {
            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    undocells[x, y] = cells[x, y];
                }
            }
        }//undo_record���\�b�h

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

        }//undo_turnover���\�b�h


    }//cpu�N���X
}
